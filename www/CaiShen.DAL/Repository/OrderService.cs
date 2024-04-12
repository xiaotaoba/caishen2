using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class OrderService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获得生成的订单编号

        //public static string GetOrderNo(int sh_id)
        //{
        //    return DateTime.Now.ToString("yyyyMMdd") + sh_id.ToString().PadLeft(8, '0') + Assistant.GetRandomNumber(3);
        //}

        /// <summary>
        /// 获取订单编号 格式："时间(14位)|UserID(6)|随机2位|交易类型(2位:01交易02充值)" ，2017-12-04删除店铺ID,原结构：时间(14位)|ShopID(4)|UserID(6)|随机2位|交易类型(2位:01交易02充值)
        /// </summary>
        /// <param name="UserShopID"></param>
        /// <param name="UserID"></param>
        /// <param name="type">01订单,02充值,03发票税点,04提现</param>
        /// <returns></returns>
        public static string GetOrderNo(int UserShopID, int UserID, string type = "01")
        {
            //2017-12-04删除店铺ID
            //return DateTime.Now.ToString("yyyyMMddHHmmss") + UserShopID.ToString().PadLeft(4, '0') + UserID.ToString().PadLeft(6, '0') + Assistant.GetRandomNumber(2) + type;
            return DateTime.Now.ToString("yyyyMMddHHmmss") + UserID.ToString().PadLeft(6, '0') + Assistant.GetRandomNumber(2) + type;
        }

        /// <summary>
        /// 获取订单编号中用户ID
        /// </summary>
        /// <param name="order_no"></param>
        /// <returns></returns>
        public static int GetUserIDByOrderNo(string order_no)
        {
            return Convert.ToInt32(order_no.Substring(14, 6));
        }
        /// <summary>
        /// 获取订单编号中交易类型
        /// </summary>
        /// <param name="order_no"></param>
        /// <returns>01订单,02充值,03发票税点</returns>
        public static string GetOrderTypeByOrderNo(string order_no)
        {
            return order_no.Substring(order_no.Length - 2, 2);
        }

        #endregion

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static Order GetModel(int ID)
        {
            var list = work.Context.Orders.AsNoTracking().Where(m => m.ID == ID).ToList<Order>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new Order();
        }

        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static Order GetModelByOrderNo(string order_no)
        {
            var model = work.Context.Orders.AsNoTracking().Where(m => m.O_OrderNo == order_no).FirstOrDefault<Order>();
            return model;
        }

        #endregion

        #region  获取产品交易成功数

        /// <summary>
        /// 获取产品交易成功数
        /// </summary>
        /// <returns></returns>
        public static int GetOrderCount(int ShopID = 0, int GoodsID = 0)
        {
            var rs = work.Context.OrderDetails
                .Join(work.Context.GoodsSKUs, m => m.GoodsSKUID, GSKU => GSKU.ID, (m, GSKU) => new { m, GSKU });
            if (ShopID == 0)
            {
                rs = rs.Where(m => m.GSKU.GoodsID == GoodsID);
            }
            else
            {
                rs = rs.Where(m => m.m.UserShopID == ShopID && m.GSKU.GoodsID == GoodsID);
            }
            return rs.Count();
        }

        #endregion

        #region 更新订单状态

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool UpdateStatus(int OrderID, int status)
        {
            Order model = work.Context.Orders.AsNoTracking().Where(m => m.ID == OrderID).FirstOrDefault();
            if (model != null)
            {
                model.O_Status = status;
                if (status == Convert.ToInt32(DataConfig.OrderStatusEnum.已付款) || status == Convert.ToInt32(DataConfig.OrderStatusEnum.已支付定金))
                {
                    model.O_PayTime = DateTime.Now;
                }
                work.OrderRepository.Update(model);
                work.Save();
            }
            return true;
        }

        /// <summary>
        /// 更新订单发货状态
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool UpdateShippingStatus(int OrderID, int status)
        {
            Order model = work.OrderRepository.GetByID(OrderID);
            if (model != null)
            {
                model.O_ShippingStatus = status;

                work.OrderRepository.Update(model);
                work.Save();
            }
            return true;
        }

        #endregion

        #region 获得订单数量

        /// <summary>
        /// 获得订单数量
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="status">订单状态：1:待付款,2:待发货, 3:待收货, 4:待评价</param>
        /// <returns></returns>
        public static int GetCountByStatus(int UserID, int status)
        {
            if (status == 1)
            {
                int paystatus = Convert.ToInt16(DataConfig.OrderStatusEnum.待付款);

                return work.Context.Orders.Where(m => m.O_IsDelete == 0 && m.UserID == UserID && m.O_Status == paystatus).Count();
            }
            else if (status == 2)
            {
                int paystatus = Convert.ToInt16(DataConfig.OrderStatusEnum.已付款);
                int shippingstatus = Convert.ToInt16(DataConfig.OrderShippingStatusEnum.待发货);
                return work.Context.Orders
                     .Join(work.Context.OrderDetails, o => o.ID, od => od.OrderID, (o, od) => new { o, od })
                    .Where(m => m.o.O_IsDelete == 0 && m.o.UserID == UserID && m.o.O_Status == paystatus && m.od.OD_ShippingStatus == shippingstatus)
                    .Select(m => m.o).Distinct()
                    .Count();
            }
            else if (status == 3)
            {
                int paystatus = Convert.ToInt16(DataConfig.OrderStatusEnum.已付款);
                int shippingstatus = Convert.ToInt16(DataConfig.OrderShippingStatusEnum.已发货);
                return work.Context.Orders
                     .Join(work.Context.OrderDetails, o => o.ID, od => od.OrderID, (o, od) => new { o, od })
                    .Where(m => m.o.O_IsDelete == 0 && m.o.UserID == UserID && m.o.O_Status == paystatus && m.od.OD_ShippingStatus == shippingstatus)
                     .Select(m => m.o).Distinct()
                    .Count();
            }
            else if (status == 4)
            {
                int paystatus = Convert.ToInt16(DataConfig.OrderStatusEnum.交易成功);
                return work.Context.Orders.Where(m => m.O_IsDelete == 0 && m.UserID == UserID && m.O_Status == paystatus && m.O_IsComment == 0).Count();
            }
            return 0;
        }

        #endregion

        #region 获得店铺订单数量

        /// <summary>
        /// 获得店铺订单数量
        /// </summary>
        /// <param name="UserShopID">店铺ID</param>
        /// <param name="status">订单状态：1:待付款,2:待发货, 3:待收货, 4:待评价</param>
        /// <returns></returns>
        public static int GetShopCountByStatus(int UserShopID, int status)
        {
            if (status == 1)
            {
                int paystatus = Convert.ToInt16(DataConfig.OrderStatusEnum.待付款);

                return work.Context.Orders
                    //.Join(work.Context.Users, o => o.UserID, u => u.ID, (o, u) => new { o, u })
                    .Where(m => m.O_IsDelete == 0 && m.UserShopID == UserShopID && m.O_Status == paystatus && m.O_IsPurchase == 0).Count();
            }
            else if (status == 2)
            {
                int paystatus = Convert.ToInt16(DataConfig.OrderStatusEnum.已付款);
                int shippingstatus = Convert.ToInt16(DataConfig.OrderShippingStatusEnum.待发货);
                return work.Context.Orders
                     .Join(work.Context.OrderDetails, o => o.ID, od => od.OrderID, (o, od) => new { o, od })
                    //.Join(work.Context.Users, o => o.o.UserID, u => u.ID, (o, u) => new { o.o, o.od, u })
                    .Where(m => m.o.O_IsDelete == 0 && m.o.UserShopID == UserShopID && m.o.O_Status == paystatus && m.od.OD_ShippingStatus == shippingstatus && m.o.O_IsPurchase == 0)
                    .Select(m => m.o).Distinct()
                    .Count();
            }
            else if (status == 3)
            {
                int paystatus = Convert.ToInt16(DataConfig.OrderStatusEnum.已付款);
                int shippingstatus = Convert.ToInt16(DataConfig.OrderShippingStatusEnum.已发货);
                return work.Context.Orders
                     .Join(work.Context.OrderDetails, o => o.ID, od => od.OrderID, (o, od) => new { o, od })
                    //.Join(work.Context.Users, o => o.o.UserID, u => u.ID, (o, u) => new { o.o, o.od, u })
                    .Where(m => m.o.O_IsDelete == 0 && m.o.UserShopID == UserShopID && m.o.O_Status == paystatus && m.od.OD_ShippingStatus == shippingstatus && m.o.O_IsPurchase == 0)
                     .Select(m => m.o).Distinct()
                    .Count();
            }
            else if (status == 4)
            {
                int paystatus = Convert.ToInt16(DataConfig.OrderStatusEnum.交易成功);
                return work.Context.Orders
                    //.Join(work.Context.Users, o => o.UserID, u => u.ID, (o, u) => new { o, u })
                    .Where(m => m.O_IsDelete == 0 && m.UserShopID == UserShopID && m.O_Status == paystatus && m.O_IsPurchase == 0).Count();
            }
            return 0;
        }

        #endregion

        #region 获得供应商订单数量

        /// <summary>
        /// 获得供应商订单数量
        /// </summary>
        /// <param name="SupplierID">供应商ID</param>
        /// <param name="status">订单状态：1:待付款,2:待发货, 3:待收货, 4:待评价</param>
        /// <returns></returns>
        public static int GetSupplierCountByStatus(int SupplierID, int status)
        {
            if (status == 1)
            {
                int paystatus = Convert.ToInt16(DataConfig.OrderStatusEnum.待付款);

                return work.Context.Orders
                     .Join(work.Context.OrderDetails, o => o.ID, od => od.OrderID, (o, od) => new { o, od })
                    .Where(m => m.o.O_IsDelete == 0 && m.od.SupplierID == SupplierID && m.o.O_Status == paystatus && m.o.O_Status == paystatus)
                    .Select(m => m.o).Distinct()
                    .Count();
            }
            else if (status == 2)
            {
                int paystatus = Convert.ToInt16(DataConfig.OrderStatusEnum.已付款);
                int shippingstatus = Convert.ToInt16(DataConfig.OrderShippingStatusEnum.待发货);
                return work.Context.Orders
                     .Join(work.Context.OrderDetails, o => o.ID, od => od.OrderID, (o, od) => new { o, od })
                    .Where(m => m.o.O_IsDelete == 0 && m.od.SupplierID == SupplierID && m.o.O_Status == paystatus && m.od.OD_ShippingStatus == shippingstatus)
                    .Select(m => m.o).Distinct()
                    .Count();
            }
            else if (status == 3)
            {
                int paystatus = Convert.ToInt16(DataConfig.OrderStatusEnum.已付款);
                int shippingstatus = Convert.ToInt16(DataConfig.OrderShippingStatusEnum.已发货);
                return work.Context.Orders
                     .Join(work.Context.OrderDetails, o => o.ID, od => od.OrderID, (o, od) => new { o, od })
                    .Where(m => m.o.O_IsDelete == 0 && m.od.SupplierID == SupplierID && m.o.O_Status == paystatus && m.od.OD_ShippingStatus == shippingstatus)
                     .Select(m => m.o).Distinct()
                    .Count();
            }
            else if (status == 4)
            {
                int paystatus = Convert.ToInt16(DataConfig.OrderStatusEnum.交易成功);
                return work.Context.Orders
                     .Join(work.Context.OrderDetails, o => o.ID, od => od.OrderID, (o, od) => new { o, od })
                    .Where(m => m.o.O_IsDelete == 0 && m.od.SupplierID == SupplierID && m.o.O_Status == paystatus && m.o.O_Status == paystatus)
                    .Select(m => m.o).Distinct()
                    .Count();
            }
            return 0;
        }

        #endregion

        #region 获取（计算）本次订单邮费 --已作废（20171030）

        //public static decimal GetPostFee(int userAddressID, Int32[] cartids)
        //{
        //    decimal firstFee = 0;
        //    decimal totalPostFee = 0;
        //    var rst = work.Context.Carts.Where(m => cartids.Contains(m.ID)).ToList();
        //    foreach (var item in rst)
        //    {
        //        totalPostFee += ShippingTemplateService.GetShippingFee(userAddressID, item.GoodsID, item.Count, item.CartTotalPrice, out firstFee);
        //    }
        //    return totalPostFee;
        //}

        #endregion

        #region 订单支付成功后处理

        /// <summary>
        /// 订单支付成功后处理
        /// </summary>
        /// <param name="out_trade_no">网站订单或充值单号</param>
        /// <param name="trade_no">第三方交易单号</param>
        /// <param name="total_fee">交易总金额(元)</param>
        /// <param name="payway">支付方式：1支付宝 2微信 3余额</param>
        public static bool PaySuccessToDo(string out_trade_no, string trade_no, decimal total_fee, int payway)
        {
            int pay_user_id = OrderService.GetUserIDByOrderNo(out_trade_no);
            string order_type = OrderService.GetOrderTypeByOrderNo(out_trade_no);
            string payway_name = "";
            switch (payway)
            {
                case 1: payway_name = "支付宝"; break;
                case 2: payway_name = "微信"; break;
                case 3: payway_name = "余额"; break;
                default: break;
            }

            //pay_user_id = Convert.ToInt32(out_trade_no.Split('_')[1]);
            if (order_type == "03")//支付发票税点
            {
                InvoiceLog invoiceModel = InvoiceLogService.GetModelByNumber(out_trade_no);
                if (invoiceModel != null)
                {
                    //更新状态
                    InvoiceLogService.UpdateStatus(invoiceModel.ID, Convert.ToInt16(DataConfig.InvoiceStatusEnum.待处理));

                    User user = UserService.GetModel(pay_user_id);
                    if (user != null)
                    {
                        decimal shenyu = user.U_Amount;
                        //新增资金流动记录
                        UserAmountHistoryService.Insert(user.ID, total_fee, shenyu, 0, user.U_LockAmount, Convert.ToInt16(DataConfig.AmountHistoryTypeEnum.减少), "发票税点", payway_name + "支付" + out_trade_no, 0, user.U_UserName, trade_no);
                    }
                }
            }
            else if (order_type == "02")//充值
            {
                //不存在处理记录
                if (!UserAmountHistoryService.ExistThing(out_trade_no))
                {
                    decimal shenyu;
                    User user = UserService.GetModel(pay_user_id);
                    if (user != null)
                    {
                        shenyu = user.U_Amount;
                        //更新余额
                        UserService.UpdateAmount(user, total_fee);
                        //更新资金流动记录
                        UserAmountHistoryService.Insert(user.ID, total_fee, (total_fee + shenyu), 0, user.U_LockAmount, Convert.ToInt16(DataConfig.AmountHistoryTypeEnum.增加), "在线充值", payway_name + "充值" + out_trade_no, 0, user.U_UserName, trade_no);
                    }
                }
            }
            else if (order_type == "01")//购物下单
            {
                decimal shenyu;
                User user = UserService.GetModel(pay_user_id);
                if (user != null)
                {
                    shenyu = user.U_Amount;
                    Order orderModel = OrderService.GetModelByOrderNo(out_trade_no);
                    string category = "在线支付";
                    bool isFirstPay = true;//首款或全款，用于短信下单通知

                    //余额支付
                    if (orderModel != null && payway == 3)
                    {
                        #region 余额支付

                        if (shenyu < total_fee)
                        {
                            Log.WriteLog(string.Format("订单支付失败，余额不足！{0}-{1}", user.U_UserName, orderModel.O_OrderNo), "order", DateTime.Now.ToString("yyyyMMdd"));
                            return false;
                        }
                        else
                        {
                            //存在订单,且为“未付款”
                            if (orderModel.O_Status == Convert.ToInt32(DataConfig.OrderStatusEnum.待付款))
                            {
                                if (orderModel.O_PayWay == Convert.ToInt32(DataConfig.OrderPayWayEnum.预付定金))
                                {
                                    //更新订单状态
                                    OrderService.UpdateStatus(orderModel.ID, Convert.ToInt32(DataConfig.OrderStatusEnum.已支付定金));
                                    category = "支付定金";
                                }
                                else if (orderModel.O_IsPurchase == 1)
                                {
                                    //更新订单状态
                                    OrderService.UpdateStatus(orderModel.ID, Convert.ToInt32(DataConfig.OrderStatusEnum.已支付货款));
                                    category = "支付货款";
                                }
                                else
                                {
                                    //更新订单状态
                                    OrderService.UpdateStatus(orderModel.ID, Convert.ToInt32(DataConfig.OrderStatusEnum.已付款));
                                    OrderDetailService.UpdatePayStatusByOrderID(orderModel.ID, Convert.ToInt32(DataConfig.OrderStatusEnum.已付款));

                                    category = "余额支付";
                                }

                            }
                            else if (orderModel.O_Status == Convert.ToInt32(DataConfig.OrderStatusEnum.已支付定金))
                            {
                                //更新订单状态
                                OrderService.UpdateStatus(orderModel.ID, Convert.ToInt32(DataConfig.OrderStatusEnum.已付款));
                                OrderDetailService.UpdatePayStatusByOrderID(orderModel.ID, Convert.ToInt32(DataConfig.OrderStatusEnum.已付款));
                                category = "支付尾款";
                                isFirstPay = false;
                            }
                            else if (orderModel.O_Status == Convert.ToInt32(DataConfig.OrderStatusEnum.已支付货款))
                            {
                                //更新订单状态
                                OrderService.UpdateStatus(orderModel.ID, Convert.ToInt32(DataConfig.OrderStatusEnum.已付款));
                                OrderDetailService.UpdatePayStatusByOrderID(orderModel.ID, Convert.ToInt32(DataConfig.OrderStatusEnum.已付款));
                                category = "支付进货运费";
                                isFirstPay = false;
                            }
                            else
                            {
                                return false;
                            }
                            UserService.UpdateAmount(user.ID, -total_fee);
                            //更新资金流动记录
                            UserAmountHistoryService.Insert(user.ID, total_fee, shenyu - total_fee, 0, user.U_LockAmount, Convert.ToInt16(DataConfig.AmountHistoryTypeEnum.减少), category, payway_name + "支付" + out_trade_no, orderModel.ID, user.U_UserName, trade_no);
                        }

                        #endregion
                    }
                    else if (orderModel != null)//在线支付
                    {
                        #region 在线支付

                        if (orderModel.O_Status == Convert.ToInt32(DataConfig.OrderStatusEnum.待付款))//存在订单,且为“未付款”
                        {
                            if (orderModel.O_PayWay == Convert.ToInt32(DataConfig.OrderPayWayEnum.预付定金))
                            {
                                //更新订单状态
                                OrderService.UpdateStatus(orderModel.ID, Convert.ToInt32(DataConfig.OrderStatusEnum.已支付定金));
                                category = "支付定金";
                            }
                            else if (orderModel.O_IsPurchase == 1)
                            {
                                //更新订单状态
                                OrderService.UpdateStatus(orderModel.ID, Convert.ToInt32(DataConfig.OrderStatusEnum.已支付货款));
                                category = "支付货款";
                            }
                            else
                            {
                                //更新订单状态
                                OrderService.UpdateStatus(orderModel.ID, Convert.ToInt32(DataConfig.OrderStatusEnum.已付款));
                                OrderDetailService.UpdatePayStatusByOrderID(orderModel.ID, Convert.ToInt32(DataConfig.OrderStatusEnum.已付款));
                            }
                            ////推送微信消息
                            //WangZhicn.Control.ShopHistory.SendTemplateMessage(openid, shModel.Sh_Id);
                            ////优惠券支付成功，修改为已使用
                            //WangZhicn.Control.UserQuan.UpdateStatusByOrderNo(user.UserId, 1, shModel.Sh_orderNo);
                        }
                        else if (orderModel.O_Status == Convert.ToInt32(DataConfig.OrderStatusEnum.已支付定金))//存在订单,且为“已支付定金”
                        {
                            //更新订单状态
                            OrderService.UpdateStatus(orderModel.ID, Convert.ToInt32(DataConfig.OrderStatusEnum.已付款));
                            OrderDetailService.UpdatePayStatusByOrderID(orderModel.ID, Convert.ToInt32(DataConfig.OrderStatusEnum.已付款));
                            category = "支付尾款";
                            isFirstPay = false;
                        }
                        else if (orderModel.O_Status == Convert.ToInt32(DataConfig.OrderStatusEnum.已支付货款))//存在订单,且为“已支付货款”
                        {
                            //更新订单状态
                            OrderService.UpdateStatus(orderModel.ID, Convert.ToInt32(DataConfig.OrderStatusEnum.已付款));
                            OrderDetailService.UpdatePayStatusByOrderID(orderModel.ID, Convert.ToInt32(DataConfig.OrderStatusEnum.已付款));
                            category = "支付进货运费";
                            isFirstPay = false;
                        }
                        else
                        {
                            return false;
                        }
                        //更新资金流动记录
                        UserAmountHistoryService.Insert(user.ID, total_fee, shenyu, 0, user.U_LockAmount, Convert.ToInt16(DataConfig.AmountHistoryTypeEnum.减少), category, payway_name + "支付" + out_trade_no, orderModel.ID, user.U_UserName, trade_no);

                        #endregion
                    }

                    #region 赠送积分

                    //赠送积分
                    int score = SiteService.GetScoreByAmount(total_fee);
                    UserService.UpdateScore(user.ID, score);
                    //添加积分记录
                    UserScoreHistoryService.Insert(user.ID, score, user.U_Score + score, 0, user.U_LockScore, 1, "赠送", "交易单号：" + out_trade_no, user.ID, "系统");

                    #endregion

                    #region 发送下单成功短信通知

                    //发送下单成功短信通知
                    if (isFirstPay)
                    {
                        UserShop shopModel = UserShopService.GetModel(orderModel.UserShopID);
                        if (shopModel != null)
                        {
                            SmsService.SendOrderSuccessSms(ConfigHelper.GetConfigString("SmsOrderMobile"), shopModel.Shop_Name, orderModel.O_OrderNo);
                        }
                        else
                        {
                            SmsService.SendOrderSuccessSms(ConfigHelper.GetConfigString("SmsOrderMobile"), "0", orderModel.O_OrderNo);
                        }
                    }

                    #endregion
                }

            }
            return true;
        }


        #endregion

        #region 判断订单是否已支付

        public static bool IsPay(string out_trade_no)
        {
            bool isPay = false;
            string order_type = OrderService.GetOrderTypeByOrderNo(out_trade_no);
            if (order_type == "02")//充值
            {
                //存在处理记录
                if (UserAmountHistoryService.ExistThing(out_trade_no))
                {
                    isPay = true;
                }
            }
            else//支付
            {
                Order orderModel = OrderService.GetModelByOrderNo(out_trade_no);
                if (orderModel != null)
                {
                    if (orderModel.O_Status == Convert.ToInt32(DataConfig.OrderStatusEnum.交易成功) || orderModel.O_Status == Convert.ToInt32(DataConfig.OrderStatusEnum.已付款) || orderModel.O_Status == Convert.ToInt32(DataConfig.OrderStatusEnum.已支付定金))
                    {
                        isPay = true;
                    }
                }
            }
            return isPay;
        }

        #endregion

        #region 整个订单 —— 分配仓库及供应商  +  计算运费

        /// <summary>
        /// 整个订单 ——分配仓库及供应商 + 计算运费
        /// </summary>
        /// <param name="addressid">配送地址</param>
        /// <param name="cartids">下单产品</param>
        /// <returns></returns>
        public static decimal GetShippingFee(int addressid, Int32[] cartids, UserShop shopModel)
        {
            return 0;
        }

        /// <summary>
        /// 单个分单地区-运费
        /// </summary>
        /// <param name="cartVList_weight">重货集合</param>
        /// <param name="cartVList_light">泡货集合</param>
        /// <param name="areaModel">地区模板</param>
        /// <returns></returns>
        private static decimal GetSingleShippingFee(List<CartVModel> cartVList_weight, List<CartVModel> cartVList_light, ShippingArea areaModel)
        {
            return 0;
        }


        /// <summary>
        /// 购物车集合中，总的商品隐藏运费和
        /// </summary>
        /// <param name="cartvList"></param>
        /// <returns></returns>
        private static decimal GetTotalHiddenShippingFee(List<CartVModel> cartvList)
        {
            return 0;
        }

        #endregion

        #region 获取单品-配送至-收货地运费

        /// <summary>
        /// 获取单品-配送至-收货地运费
        /// </summary>
        /// <param name="goodsID">产品ID</param>
        /// <param name="goodsCount">单品数量(数量*sku数量)</param>
        /// <param name="addressid"></param>
        /// <param name="shopModel"></param>
        /// <returns></returns>
        public static decimal GetGoodsShippingFee(int goodsID, int goodsCount, int addressid, UserShop shopModel)
        {
            decimal totalFee = 0;
            double w_totalWeight = 0;//重货总重量(KG)
            double l_totalWeight = 0;//泡货总重量(KG)
            double l_totalVolume = 0;//泡货总体积(m³)
            int warehouseID = 0;//仓库
            int supplierID = 0;//供应商
            ShippingArea areaModel = null;
            Goods goodsModel = null;

            if (goodsID != 0)
            {
                goodsModel = work.Context.Goods.AsNoTracking().Where(m => m.ID == goodsID).FirstOrDefault();
                if (goodsModel == null || goodsModel.G_IsFreeShipping == 1)
                    return 0;
            }
            else
            {
                return 0;
            }

            //分配仓库 + 供应商
            if (goodsModel.G_IsExist == 1)
            {
                warehouseID = WarehouseService.GetWarehouseID(shopModel, goodsID);
                Warehouse warehouseModel = work.WarehouseRepository.GetByID(warehouseID);

                areaModel = ShippingAreaService.GetShippingArea(warehouseModel.ShippingTemplateID, addressid);
            }
            else
            {
                supplierID = SupplierService.GetSupplierID(shopModel, goodsModel);
                Supplier supplierModel = work.Context.Suppliers.AsNoTracking().Where(m => m.ID == supplierID).First();

                areaModel = ShippingAreaService.GetShippingArea(supplierModel.ShippingTemplateID, addressid);
            }

            if (areaModel == null)
            {
                //Log.WriteLog("未查询到运费区域", "fee", DateTime.Now.ToString("yyyyMMdd"));
                return 0;
            }

            return totalFee;
        }

        #endregion

        #region 获取单品- 隐藏运费（门店和仓库同区域）仓库配送至门店的费用
        /// <summary>
        /// 获取单品- 隐藏运费（门店和仓库同区域）仓库配送至门店的费用
        /// </summary>
        /// <param name="goodsID">产品ID</param>
        /// <param name="goodsCount">单品数量(数量*sku数量)</param>
        /// <param name="addressid"></param>
        /// <param name="shopModel"></param>
        /// <returns></returns>
        public static decimal GetGoodsHiddenShippingFee(int goodsID, int goodsCount, UserShop shopModel)
        {
            return 0;
        }

        #endregion

        #region 获取指定多个订单购买的产品记录

        /// <summary>
        /// 获取指定多个订单购买的产品记录
        /// </summary>
        /// <param name="orderIds">订单ID集合</param>
        /// <returns></returns>
        public static List<OrderDetailVModel> GetOrderDetails(List<int> orderIds)
        {
            if (orderIds == null || orderIds.Count < 1)
            {
                return null;
            }
            //购买产品记录
            List<OrderDetailVModel> listOrderDetailV2 = work.Context.OrderDetails
               .Join(work.Context.Goods, od => od.GoodsID, g => g.ID, (od, g) => new { od, g })
               .Where(m => orderIds.Contains(m.od.OrderID)).Select(m => new OrderDetailVModel
               {
                   OrderDetail = m.od,
                   PhotoUrl = m.g.G_Image
               }).Distinct().ToList();

            return listOrderDetailV2;
        }

        #endregion

        #region 获取(计算)订单统计信息

        /// <summary>
        ///  获取(计算)订单统计信息
        /// </summary>
        /// <param name="currentShop"></param>
        /// <param name="addressid"></param>
        /// <param name="cartids"></param>
        /// <param name="shipping_way"></param>
        /// <param name="payway"></param>
        /// <param name="coupon">优惠券金额</param>
        /// <param name="hongbao">红包金额</param>
        /// <returns></returns>
        public static UserOrderTJVModel GetOrderTongJi(UserShop currentShop, int addressid, Int32[] cartids, int shipping_way, int payway, decimal coupon, decimal hongbao)
        {
            UserOrderTJVModel orderTjModel = new UserOrderTJVModel();

            var rst = work.Context.Carts.AsNoTracking().Join(work.Context.Goods, c => c.GoodsID, g => g.ID, (c, g) => new
            {
                c,
                g.G_Name,
                g.G_Image
            }).Where(m => cartids.Contains(m.c.ID))
           .Select(m => new CartVModel
           {
               Cart = m.c,
               Title = m.G_Name,
               PhotoUrl = m.G_Image
           }).OrderByDescending(m => m.Cart.ID).ToList();

            foreach (var item in rst)
            {
                orderTjModel.total_amount = orderTjModel.total_amount + item.Cart.CartTotalPrice;
                orderTjModel.total_count = orderTjModel.total_count + item.Cart.Count;
                orderTjModel.addition_amount = orderTjModel.addition_amount + item.Cart.DesignFee;
                orderTjModel.total_shop_price = orderTjModel.total_shop_price + item.Cart.TotalShopPrice;
                orderTjModel.total_cost_price = orderTjModel.total_cost_price + item.Cart.TotalCostPrice;

                //if (shipping_way != Convert.ToInt32(DataConfig.OrderShippingWayEnum.上门自提))
                //{
                //    orderTjModel.post_fee = orderTjModel.post_fee + ShippingTemplateService.GetShippingFee(addressid, item.Cart.GoodsID, item.Cart.Count * item.Cart.GoodsCount, item.Cart.CartTotalPrice, out firstFee);
                //}
                //else
                //{
                //    orderTjModel.post_fee = 0;
                //}
            }

            if (shipping_way == Convert.ToInt32(DataConfig.OrderShippingWayEnum.上门自提))
            {
                orderTjModel.post_fee = 0;
            }
            else
            {
                orderTjModel.post_fee = 0; 
            }

            orderTjModel.discount_amount = coupon + hongbao;
            orderTjModel.total_amount = orderTjModel.total_amount + orderTjModel.post_fee;
            orderTjModel.needpay_amount = orderTjModel.total_amount - orderTjModel.discount_amount;

            if (payway == Convert.ToInt32(DataConfig.OrderPayWayEnum.预付定金))
            {
                orderTjModel.pay_amount = Convert.ToDecimal(orderTjModel.needpay_amount * Convert.ToDecimal(UserShopService.GetCurrentShopYufuPercent()));
            }
            else if (payway == Convert.ToInt32(DataConfig.OrderPayWayEnum.货到付款))
            {
                orderTjModel.pay_amount = 0;
            }
            else
            {
                orderTjModel.pay_amount = orderTjModel.needpay_amount;
            }
            return orderTjModel;
        }

        #endregion

        #region 获取(计算)订单价格统计信息 -json

        /// <summary>
        /// 获取(计算)订单价格统计信息json 对象
        /// </summary>
        /// <param name="LoginedUserModel"></param>
        /// <param name="currentShop"></param>
        /// <returns></returns>
        public static object GetOrderTongJiJsonObject(User LoginedUserModel, UserShop currentShop, int addressid, int shipping_way, int payway, decimal coupon = 0, decimal hongbao = 0)
        {
            if (LoginedUserModel == null)
            {
                return new { status = "-1", msg = "请先登录!" };
            }

            string cart = CookieHelper.GetValue("orderCartIds");
            if (string.IsNullOrEmpty(cart))
            {
                return new { status = "-1", msg = "请先选购商品!" };
            }
            else
            {
                UserOrderTJVModel orderTjModel = new UserOrderTJVModel();

                Int32[] cartids = CartService.GetCartIds(cart);
                orderTjModel = GetOrderTongJi(currentShop, addressid, cartids, shipping_way, payway, coupon, hongbao);

                return new { status = "success", msg = "", total_amount = orderTjModel.total_amount, total_count = orderTjModel.total_count, discount_amount = orderTjModel.discount_amount, needpay_amount = orderTjModel.needpay_amount, pay_amount = orderTjModel.pay_amount, post_fee = orderTjModel.post_fee };

            }
        }

        #endregion

        #region 下单所有产品是否支持自提

        /// <summary>
        /// 门店和所有产品都支持自提，才允许自提
        /// </summary>
        /// <param name="cartids"></param>
        /// <returns></returns>
        public static bool IsZiti(Int32[] cartids, UserShop cuurentUserShopModel)
        {
            if (cuurentUserShopModel.Shop_IsZiti == 0)
            {
                return false;
            }
            if (GoodsService.IsZiti(cartids))
            {
                return false;
            }

            return true;
        }

        #endregion

        #region 创建订单

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="LoginedUserModel"></param>
        /// <param name="CurrentShopModel"></param>
        /// <param name="newVOrder"></param>
        /// <param name="cart">下单产品购物车id字符串，逗号隔开</param>
        /// <param name="user_address_selected"></param>
        /// <param name="discount_amount_coupon"></param>
        /// <param name="discount_amount_hongbao"></param>
        /// <param name="platform">pc或mobile等</param>
        /// <returns>订单实体</returns>
        public static Order CreateOrder(User LoginedUserModel, UserShop CurrentShopModel, OrderPayModel newVOrder, string cart, string user_address_selected = "", decimal discount_amount_coupon = 0, decimal discount_amount_hongbao = 0, string platform = "pc", int ispurchase = 0)
        {
            string ip = IPHelper.GetIP();
            int userId = LoginedUserModel.ID;
            int shopId = CurrentShopModel.ID;
            string order_no = OrderService.GetOrderNo(shopId, userId);

            #region 从新计算的总价

            //从新计算的总价
            UserOrderTJVModel orderTjModel = new UserOrderTJVModel();
            Int32[] cartids = CartService.GetCartIds(cart);
            orderTjModel = OrderService.GetOrderTongJi(CurrentShopModel, newVOrder.addressid, cartids, newVOrder.shipping_way, newVOrder.payway, discount_amount_coupon, discount_amount_hongbao);

            #endregion

            #region 创建订单

            Order order = new Order();

            order.O_Address = user_address_selected;
            order.O_BusinessTax = newVOrder.invoice_number;
            order.O_CreateTime = DateTime.Now;
            order.O_DiscountAmount = orderTjModel.discount_amount;
            order.O_AdditionAmount = orderTjModel.addition_amount;
            order.O_GiveScore = SiteService.GetScoreByAmount(orderTjModel.needpay_amount);
            order.O_InvoiceTitle = newVOrder.invoice_title;
            order.O_IP = ip;
            order.O_IsComment = 0;
            order.O_IsInvoice = newVOrder.invoice;
            order.O_OrderNo = order_no;
            order.O_PayAmount = orderTjModel.pay_amount;
            order.O_NeedPayAmount = orderTjModel.needpay_amount;
            order.O_RestPayAmount = orderTjModel.needpay_amount - orderTjModel.pay_amount;
            order.O_PayScore = 0;
            order.O_PayTime = DateTime.Now;
            order.O_PayWay = newVOrder.payway;
            order.O_PostFee = orderTjModel.post_fee;
            order.O_Remark = newVOrder.remark;
            order.O_ShippingTime = newVOrder.shipping_time;
            order.O_ShippingWay = newVOrder.shipping_way;
            order.O_Status = Convert.ToInt32(DataConfig.OrderStatusEnum.待付款);
            order.O_TotalAmount = orderTjModel.total_amount;
            order.O_Type = 0;
            order.UserAddressID = newVOrder.addressid;
            order.UserID = LoginedUserModel.ID;
            order.UserShopID = CurrentShopModel.ID;
            order.O_ShippingStatus = Convert.ToInt32(DataConfig.OrderShippingStatusEnum.待发货);
            order.O_TotalShopPrice = orderTjModel.total_shop_price;
            order.O_TotalCostPrice = orderTjModel.total_cost_price;
            order.O_Platform = platform;
            order.O_IsPurchase = ispurchase;

            //创建订单
            work.OrderRepository.Insert(order);
            work.Save();

            ////更新订单号-作废，直接添加之前根据userid生成
            //order.O_OrderNo = OrderService.GetOrderNo(shopId, order.ID);
            //work.OrderRepository.Update(order);
            //work.Save();

            #endregion

            #region 循环添加订单详细

            //选中购物车产品信息
            var rst = work.Context.Carts.Join(work.Context.Goods, c => c.GoodsID, g => g.ID, (c, g) => new
            {
                c,
                g
            }).Where(m => m.c.UserID == LoginedUserModel.ID && cartids.Contains(m.c.ID))
            .OrderByDescending(m => m.c.ID);

            //循环添加订单详细
            foreach (var item in rst)
            {
                OrderDetail orderDetail = new OrderDetail();

                int warehouseID = 0;
                int supplierID = 0;

                if (item.g.G_IsDesign != 1)//非设计产品，分配仓库或供应商
                {
                    if (item.g.G_IsExist == 1)
                    {
                        //分配仓库
                        //warehouseID = WarehouseService.GetWarehouseID(order.UserAddressID, item.g.ID);
                        warehouseID = WarehouseService.GetWarehouseID(CurrentShopModel, item.g.ID);
                    }
                    else
                    {
                        //分配供应商
                        supplierID = SupplierService.GetSupplierID(order.UserAddressID, item.g);
                        //supplierID = SupplierService.GetSupplierID(CurrentShopModel, item.g);
                    }
                }
                orderDetail.GoodsSKUID = item.c.SKUID;
                orderDetail.OD_Count = item.c.Count;
                orderDetail.OD_DiscountAmount = 0;
                orderDetail.OD_GiveScore = GoodsService.GetGiveScoreByPrice(item.c.CartTotalPrice);
                orderDetail.OD_GoodsName = item.g.G_Name;
                orderDetail.OD_IsDelete = 0;
                orderDetail.OD_IsExist = item.g.G_IsExist;
                orderDetail.OD_PayAmount = item.c.CartTotalPrice;
                orderDetail.OD_PayScore = 0;
                orderDetail.OD_PayStatus = Convert.ToInt32(DataConfig.OrderStatusEnum.待付款);
                orderDetail.OD_PostFee = 0;
                orderDetail.OD_PropertiesName = item.c.PropertiesName;
                orderDetail.OD_ShippingStatus = Convert.ToInt32(DataConfig.OrderShippingStatusEnum.待发货);
                orderDetail.OD_TotalAmount = item.c.CartTotalPrice;
                orderDetail.OD_Type = 0;
                orderDetail.OD_UnitPrice = item.g.G_Price;
                orderDetail.OrderID = order.ID;
                orderDetail.SupplierID = supplierID;
                orderDetail.UserID = userId;
                orderDetail.UserShopID = shopId;
                orderDetail.WarehouseID = warehouseID;
                orderDetail.GoodsID = item.g.ID;
                orderDetail.OD_IsHasDesignFile = item.c.IsHasDesignFile;
                orderDetail.OD_DesignFee = item.c.DesignFee;
                orderDetail.OD_DesignFile = "";
                orderDetail.OD_HiddenPostFee = item.c.HiddenShippingFee;
                orderDetail.OD_TotalShopPrice = item.c.TotalShopPrice;
                orderDetail.OD_TotalCostPrice = item.c.TotalCostPrice;
                orderDetail.OD_GiveScore = SiteService.GetScoreByAmount(item.c.CartTotalPrice);
                orderDetail.OD_IsDesign = item.g.G_IsDesign;

                work.OrderDetailRepository.Insert(orderDetail);
            }
            work.Save();

            #endregion

            #region 订单创建完成，清空购物车已下单产品

            if (!string.IsNullOrEmpty(cart))
            {
                //移出购物车
                CartService.DeleteBatch(cart);
                CookieHelper.Delete("orderCartIds");
            }

            #endregion

            #region 附属费用清单(产品及运费之外的费用)

            if (discount_amount_coupon != 0)
            {
                OrderFeeListService.Add(userId, order.ID, "优惠券(" + newVOrder.coupon + ")", discount_amount_coupon, 0);
                UserCouponService.UpdateStatusUsed(userId, newVOrder.coupon);
            }
            if (discount_amount_hongbao != 0)
            {
                OrderFeeListService.Add(userId, order.ID, "红包(" + newVOrder.hongbao + ")", discount_amount_hongbao, 0);
                UserHongBaoService.UpdateStatus(newVOrder.hongbao, 1);
            }
            #endregion

            return order;
        }

        #endregion

        #region 订单结算

        /// <summary>
        /// 订单结算_所有符合条件
        /// </summary>
        /// <returns></returns>
        public static int OrderSettlement()
        {
            return OrderSettlement(0);
        }
        /// <summary>
        /// 订单结算_所有符合条件
        /// </summary>
        /// <param name="orderID">结算指定订单ID</param>
        /// <returns>1:结算已完成，0：没有可结算订单</returns>
        public static int OrderSettlement(int orderID = 0)
        {
            //查询所有待结算订单
            //1.订单未结算，2.订单已付款+交易完成，3.已收货，4.时间大于（确认收货时间+退货期）
            int orderStatusPay = Convert.ToInt16(DataConfig.OrderStatusEnum.已付款);
            int orderStatusDone = Convert.ToInt16(DataConfig.OrderStatusEnum.交易成功);
            int orderShippingReceipt = Convert.ToInt16(DataConfig.OrderShippingStatusEnum.已收货);
            ProfitPercentConfig sysConfig = SystemInfoService.GetPercentConfig();

            DateTime deliveryTimeMin = DateTime.Now.AddDays(-sysConfig.Order_TuiHuo_Days);

            var rstTemp = work.Context.Orders
                .Join(work.Context.OrderShippings, o => o.ID, os => os.OrderID, (o, os) => new { o, os })
                .Join(work.Context.UserShops, oos => oos.o.UserShopID, us => us.ID, (oos, us) => new { oos.o, oos.os, us })
                .Join(work.Context.Users, m => m.us.UserID, u => u.ID, (m, u) => new { m.o, m.os, u })
                //.SelectMany(xy => xy.os.DefaultIfEmpty(),(x, y) => new { o = x.o, os = y })
                .Where(m => m.o.O_IsSettlement_Shop == 0 && m.o.O_ShippingStatus == orderShippingReceipt && m.os.OS_DeliveryTime != null && m.os.OS_DeliveryTime < deliveryTimeMin)
                .Where(m => m.o.O_Status == orderStatusPay || m.o.O_Status == orderShippingReceipt);
            if (orderID != 0)
            {
                rstTemp = rstTemp.Where(m => m.o.ID == orderID);
            }
            var rst = rstTemp.ToList();
            if (rst == null || rst.Count < 1)
            {
                return 0;
            }

            //所有供应商订单详细ID
            //List<OrderDetail> orderDetailOfSupplier = new List<OrderDetail>();
            foreach (var item in rst)
            {
                var rstDetail = work.Context.OrderDetails.Where(m => m.OrderID == item.o.ID);
                var orderDetailOfSupplier = rstDetail.Where(m => m.SupplierID != 0)
                    .Join(work.Context.Suppliers, od => od.SupplierID, s => s.ID, (od, s) => new { od, s })
                    .Join(work.Context.Users, m => m.s.UserID, u => u.ID, (m, u) => new { m.od, m.s, u }).ToList();

                //加盟店结算
                List<OrderDetail> orderDetail = rstDetail.ToList();
                decimal totalShopProfit = 0M;//当前订单-总店铺利润
                totalShopProfit = item.o.O_TotalAmount - item.o.O_AdditionAmount - item.o.O_PostFee - item.o.O_TotalShopPrice;

                User shopUserModel = item.u;
                shopUserModel.U_Amount = shopUserModel.U_Amount + totalShopProfit;

                //添加结算记录
                UserAmountHistoryService.Insert(shopUserModel.ID, totalShopProfit, shopUserModel.U_Amount, 0, shopUserModel.U_LockAmount, 1, "结算", "订单" + item.o.O_OrderNo, shopUserModel.ID, "系统", "加盟商");
                //修改用户余额
                work.UserRepository.Update(shopUserModel);
                //work.Save();
                //结算状态
                Order orderModel = item.o;
                orderModel.O_IsSettlement_Shop = 1;
                work.OrderRepository.Update(orderModel);
                work.Save();

                foreach (var detailItem in orderDetailOfSupplier)
                {
                    decimal totalSupplierCostPrice = 0M;//当前产品的成本-供应商价格
                    User supplierUserModel = detailItem.u;
                    supplierUserModel.U_Amount = supplierUserModel.U_Amount + totalSupplierCostPrice;

                    //添加结算记录
                    UserAmountHistoryService.Insert(supplierUserModel.ID, totalSupplierCostPrice, supplierUserModel.U_Amount, 0, supplierUserModel.U_LockAmount, 1, "结算", "订单" + item.o.O_OrderNo + "|" + detailItem.od.ID, supplierUserModel.ID, "系统", "供应商");
                    //修改用户余额
                    work.UserRepository.Update(supplierUserModel);
                    //work.Save();

                    //结算状态
                    OrderDetail orderDetailModel = detailItem.od;
                    orderDetailModel.OD_IsSettlement_Supplier = 1;
                    work.OrderDetailRepository.Update(orderDetailModel);
                    work.Save();
                }


            }
            return 1;
        }

        #endregion
    }

    /// <summary>
    /// 每次请求临时保存，运费地区数据，减少重复请求数据库
    /// </summary>
    public class AreaModelList
    {
        public ShippingArea shippingArea { get; set; }
        /// <summary>
        /// 运费模板ID
        /// </summary>
        public Int32 shippingTemplateID { get; set; }
        /// <summary>
        /// 收货地址ID
        /// </summary>
        public Int32 addressID { get; set; }
    }
}
