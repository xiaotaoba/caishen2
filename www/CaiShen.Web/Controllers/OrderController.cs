using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Web.Attribute;
using Pannet.Models;
using Pannet.DAL;
using Pannet.DAL.Repository;
using Pannet.Utility;
using PagedList;

namespace Pannet.Web.Controllers
{
    public class OrderController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 订单

        //默认订单列表
        [CheckPermission]
        public ActionResult Index(string keyword = "", int ShopID = 0, string status = "", string shippingstatus = "", int page = 1, string action = "", string time_start = "", string time_end = "", string ispurchase = "")
        {
            ViewBag.keyword = keyword;
            ViewBag.ShopID = ShopID;
            ViewBag.status = status;
            ViewBag.shippingstatus = shippingstatus;
            ViewBag.UserShops = work.UserShopRepository.Get();
            ViewBag.action = action;
            ViewBag.time_start = time_start;
            ViewBag.time_end = time_end;
            ViewBag.ispurchase = ispurchase;

            var rst = work.Context.Orders
                .Join(work.Context.Users, o => o.UserID, u => u.ID, (o, u) => new { o, u })
                .Join(work.Context.UserShops, ou => ou.o.UserShopID, us => us.ID, (ou, us) => new
                {
                    o = ou.o,
                    u = ou.u,
                    us
                });

            if (ShopID != 0)
            {
                rst = rst.Where(m => m.o.UserShopID == ShopID);
            }
            if (status != "")
            {
                rst = rst.Where(m => m.o.O_Status.ToString() == status);
            }
            if (shippingstatus != "")
            {
                rst = rst.Where(m => m.o.O_ShippingStatus.ToString() == shippingstatus);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                //搜索为产品名称
                var rst_goods_orderids = work.Context.Goods
                    .Join(work.Context.OrderDetails, g => g.ID, od => od.GoodsID, (g, od) => new { g, od })
                    .Where(m => m.g.G_Name.Contains(keyword)).Select(m => m.od.OrderID);

                rst = rst.Where(m => m.o.O_OrderNo.Contains(keyword) || m.u.U_UserName.Contains(keyword) || rst_goods_orderids.Contains(m.o.ID));
            }
            if (!string.IsNullOrEmpty(time_start))
            {
                DateTime timeStart = Convert.ToDateTime(time_start);
                rst = rst.Where(m => m.o.O_CreateTime > timeStart);
            }
            if (!string.IsNullOrEmpty(time_end))
            {
                DateTime timeEnd = Convert.ToDateTime(time_end).AddDays(1);
                rst = rst.Where(m => m.o.O_CreateTime < timeEnd);
            }
            if (!string.IsNullOrEmpty(ispurchase))
            {
                int _ispurchase = Convert.ToInt16(ispurchase);
                rst = rst.Where(m => m.o.O_IsPurchase == _ispurchase);
            }

            var rst2 = rst.Select(m => new OrderVModel
            {
                Order = m.o,
                User = m.u,
                UserShop = m.us

            }).OrderByDescending(m => m.Order.ID);

            if (action == "export")//导出
            {
                string fileName = "订单" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                CreateExcel(rst2.ToList(), fileName);
                //try
                //{

                //}
                //catch (Exception ex)
                //{
                //    Response.End();
                //}
            }

            int pageSize = 20;
            //return View(rst.ToPagedList(pageNumber, pageSize));
            return View(rst2.ToPagedList(page, pageSize));
        }

        /// <summary>
        /// 详细页
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Detail(int ID = 0)
        {
            ViewBag.ID = ID;
            ViewBag.orderID = ID;
            if (ID != 0)
            {
                var model = work.OrderRepository.Get(m => m.ID == ID).FirstOrDefault<Order>();

                //购买产品记录
                List<OrderDetailVModel> listOrderDetailV2 = work.Context.OrderDetails
                   .Join(work.Context.Goods, od => od.GoodsID, g => g.ID, (od, g) => new { od, g })
                   .Where(m => m.od.OrderID == ID).Select(m => new OrderDetailVModel
                   {
                       OrderDetail = m.od,
                       PhotoUrl = m.g.G_Image
                   }).Distinct().ToList();
                ViewBag.OrderDetails = listOrderDetailV2;

                return View(model);
            }
            return View(new Order());
        }

        /// <summary>
        /// 详细页-保存
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult Detail(int ID = 0, string O_Remark = "", int status = -1, int payway = 0, int shippingway = 0)
        {
            ViewBag.ID = ID;
            ViewBag.orderID = ID;

            if (ID != 0)
            {
                var model = work.OrderRepository.Get(m => m.ID == ID).FirstOrDefault<Order>();

                if (!string.IsNullOrEmpty(O_Remark))
                {
                    model.O_Remark = O_Remark;
                }
                if (status != -1)
                {
                    model.O_Status = status;
                }
                model.O_ShippingWay = shippingway;
                model.O_PayWay = payway;

                work.OrderRepository.Update(model);
                work.Save();
                return View(model);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Delete(int ID = 0)
        {
            if (ID != 0)
            {
                if (ID == 1)
                {
                    return RedirectToAction("Index");
                }
                var model = work.OrderRepository.Get(m => m.ID == ID).FirstOrDefault<Order>();
                if (model != null)
                {
                    model.O_IsDelete = 1;

                    work.OrderRepository.Update(model);
                    work.Save();
                    work.Dispose();
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region 导出订单

        public void CreateExcel(List<OrderVModel> list, string fileName)
        {
            HttpResponseBase resp;
            resp = HttpContext.Response;
            resp.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            resp.ContentType = "application/ms-excel";
            string colHeaders = "", ls_item = "";

            colHeaders += "购买产品" + "\t";
            colHeaders += "下单时间" + "\t";
            colHeaders += "订单号" + "\t";
            colHeaders += "门店" + "\t";
            colHeaders += "购买用户" + "\t";
            colHeaders += "总金额" + "\t";
            colHeaders += "优惠金额" + "\t";
            colHeaders += "应付款" + "\t";
            colHeaders += "实付款" + "\t";
            colHeaders += "运费" + "\t";
            colHeaders += "配送方式" + "\t";
            colHeaders += "付款状态" + "\t";
            colHeaders += "发货状态" + "\t";
            colHeaders += "支付方式" + "\t";
            colHeaders += "付款时间" + "\t";
            colHeaders += "收件人信息" + "\t";
            colHeaders += "是否开发票" + "\t";
            colHeaders += "发票抬头" + "\t";
            colHeaders += "企业税号" + "\t";
            colHeaders += "订单备注" + "\n";

            resp.Write(colHeaders);
            //向HTTP输出流中写入取得的数据信息 

            //逐行处理数据   
            foreach (OrderVModel itemv in list)
            {
                Order item = itemv.Order;
                User user = itemv.User;
                UserShop shop = itemv.UserShop;

                //购买产品记录
                List<OrderDetailVModel> listOrderDetailV2 = work.Context.OrderDetails
                   .Join(work.Context.Goods, od => od.GoodsID, g => g.ID, (od, g) => new { od, g })
                   .Where(m => m.od.OrderID == item.ID).Select(m => new OrderDetailVModel
                   {
                       OrderDetail = m.od,
                       PhotoUrl = m.g.G_Image
                   }).Distinct().ToList();

                string productInfo = "";
                //当前行数据写入HTTP输出流，并且置空ls_item以便下行数据    
                if (listOrderDetailV2 != null)
                {
                    foreach (var orderDetail in listOrderDetailV2)
                    {
                        productInfo += orderDetail.OrderDetail.OD_GoodsName + "【" + orderDetail.OrderDetail.OD_PropertiesName + "】，";
                    }
                }
                else
                {
                    productInfo = "未查询到";
                }
                ls_item += productInfo + "\t";
                ls_item += item.O_CreateTime + "\t";
                //ls_item += "=TEXT(" + item.O_OrderNo + ",\"0\")\t";
                ls_item += "|" + item.O_OrderNo + "\t";
                ls_item += shop.Shop_Name + "\t";
                ls_item += user.U_UserName + "\t";
                ls_item += item.O_TotalAmount + "\t";
                ls_item += item.O_DiscountAmount + "\t";
                ls_item += item.O_NeedPayAmount + "\t";
                ls_item += item.O_PayAmount + "\t";
                ls_item += item.O_PostFee + "\t";
                ls_item += DataConfig.OrderShippingWay.Find(m => m.Value == item.O_ShippingWay.ToString()).Name + "\t";
                ls_item += DataConfig.OrderStatus.Find(m => m.Value == item.O_Status.ToString()).Name + "\t";
                ls_item += DataConfig.OrderShippingStatus.Find(m => m.Value == item.O_ShippingStatus.ToString()).Name + "\t";
                ls_item += DataConfig.OrderPayWay.Find(m => m.Value == item.O_PayWay.ToString()).Name + "\t";
                ls_item += item.O_PayTime + "\t";
                ls_item += item.O_Address + "\t";
                ls_item += DataConfig.OrderInvoice.Find(m => m.Value == item.O_IsInvoice.ToString()).Name + "\t";
                ls_item += item.O_InvoiceTitle + "\t";
                ls_item += item.O_BusinessTax + "\t";
                ls_item += item.O_Remark + "\n";

                resp.Write(ls_item);
                ls_item = "";

            }
            resp.End();
        }

        #endregion

        #region 订单-发货记录

        /// <summary>
        /// 订单-发货记录
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult OrderShippingList(int orderID = 0, int page = 1)
        {
            ViewBag.orderID = orderID;
            if (orderID != 0)
            {
                ////购买产品记录
                //List<OrderDetailVModel> listOrderDetailV2 = work.Context.OrderDetails
                //   .Join(work.Context.Goods, od => od.GoodsID, g => g.ID, (od, g) => new { od, g })
                //   .Where(m => m.od.OrderID == OrderID).Select(m => new OrderDetailVModel
                //   {
                //       OrderDetail = m.od,
                //       PhotoUrl = m.g.G_Image
                //   }).Distinct().ToList();
                //ViewBag.OrderDetails = listOrderDetailV2;

                //订单详情 + 店铺
                Order order = work.OrderRepository.Get(m => m.ID == orderID).FirstOrDefault();// work.OrderRepository.GetByID(ID);
                ViewBag.Order = order;

                //配送记录
                var rst = work.Context.OrderShippings
                    .Join(work.Context.Orders, os => os.OrderID, o => o.ID, (os, o) => new { os, o })
                    .Where(m => m.o.ID == orderID)
                    .Select(m => m.os).OrderByDescending(m => m.OS_CreateTime);

                List<UserOrderShippingVModel> listOrderShipping = new List<UserOrderShippingVModel>();
                listOrderShipping = rst.ToList().Select(m => new UserOrderShippingVModel
                {
                    OrderShipping = m,
                    //UserShop = m.o,
                    OrderDetailVList = null
                }).OrderByDescending(m => m.OrderShipping.OS_CreateTime).Distinct().ToList();

                foreach (var item in listOrderShipping)
                {
                    //配送物品，订单详细id
                    string orderDetailIds = item.OrderShipping.OrderDetailIds;
                    string[] od_ids_arr = orderDetailIds.Split(',');

                    var rst2 = work.Context.OrderDetails
                   .Join(work.Context.Goods, od => od.GoodsID, g => g.ID, (od, g) => new { od, g })
                   .Where(m => m.od.OrderID == orderID && od_ids_arr.Contains(m.od.ID.ToString()));
                    List<OrderDetailVModel> listOrderDetailV = null;
                    listOrderDetailV = rst2.Select(m => new OrderDetailVModel
                    {
                        OrderDetail = m.od,
                        PhotoUrl = m.g.G_Image
                    }).Distinct().ToList();
                    item.OrderDetailVList = listOrderDetailV;
                }

                int pageSize = 12;
                int pageNumber = page;

                ////未配送
                //List<OrderDetailVModel> listOrderDetailV2 = work.Context.OrderDetails
                //   .Join(work.Context.Goods, od => od.GoodsID, g => g.ID, (od, g) => new { od, g })
                //   .Where(m => m.od.OrderID == OrderID && m.od.OD_ShippingStatus == 0).Select(m => new OrderDetailVModel
                //   {
                //       OrderDetail = m.od,
                //       PhotoUrl = m.g.G_Image
                //   }).Distinct().ToList();
                //ViewBag.OrderDetailNoShipping = listOrderDetailV2;

                return View(listOrderShipping.ToPagedList(pageNumber, pageSize));

            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region 订单设计稿上传

        /// <summary>
        /// 订单设计稿上传
        /// </summary>
        /// <returns></returns>
        public ActionResult DesignFile(int orderDetailID = 0)
        {
            if (orderDetailID == 0)
            {
                return RedirectToAction("Order");
            }
            ViewBag.orderDetailID = orderDetailID;
            OrderDetail model = work.OrderDetailRepository.GetByID(orderDetailID);

            return View(model);
        }
        /// <summary>
        /// 订单设计稿上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DesignFile(int orderDetailID, string OD_DesignFile)
        {
            if (orderDetailID == 0)
            {
                return RedirectToAction("Order");
            }
            ViewBag.orderDetailID = orderDetailID;
            OrderDetail model = work.OrderDetailRepository.GetByID(orderDetailID);
            model.OD_DesignFile = OD_DesignFile;

            work.OrderDetailRepository.Update(model);
            work.Save();
            work.Dispose();

            ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功！");

            return View(model);
        }

        #endregion

        #region 修改订单金额【应付金额】

        /// <summary>
        /// 修改订单金额
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderChangeAmount(int orderID = 0, decimal payAmount = 0)
        {
            if (orderID == 0)
            {
                return RedirectToAction("Order");
            }
            ViewBag.orderID = orderID;
            ViewBag.payAmount = payAmount;

            ViewBag.List = work.Context.OrderChangeLogs.AsNoTracking().Where(m => m.OrderID == orderID).OrderByDescending(m => m.ID).ToList();

            return View(new OrderChangeAmountVModel());
        }
        /// <summary>
        /// 修改订单金额-post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OrderChangeAmount(OrderChangeAmountVModel newModel, int orderID)
        {
            if (orderID == 0)
            {
                return RedirectToAction("Order");
            }
            ViewBag.payAmount = newModel.New_PayAmount;
            ViewBag.orderID = orderID;

            string content = newModel.Remark + string.Format("（原金额：{0}元 -> 新金额：{1}元）", newModel.Old_PayAmount, newModel.New_PayAmount);
            OrderChangeLogService.Add(ManagerService.GetLoginModel(), content, orderID);
            Order orderModel = work.Context.Orders.Where(m => m.ID == orderID).First();
            orderModel.O_PayAmount = newModel.New_PayAmount;
            work.OrderRepository.Update(orderModel);
            work.Save();

            ViewBag.List = work.Context.OrderChangeLogs.AsNoTracking().Where(m => m.OrderID == orderID).OrderByDescending(m => m.ID).ToList();
            ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功！");

            work.Dispose();

            return View(newModel);
        }

        #endregion

        #region 订单价格调整记录

        /// <summary>
        /// 订单价格调整记录
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderChangeAmountList(int orderID)
        {
            if (orderID == 0)
            {
                return RedirectToAction("Order");
            }
            ViewBag.orderID = orderID;
            ViewBag.List = work.Context.OrderChangeLogs.AsNoTracking().Where(m => m.OrderID == orderID).OrderByDescending(m => m.ID).ToList();
            return View();
        }

        #endregion

        #region 发货

        /// <summary>
        /// 订单发货
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public ActionResult OrderShipping(int orderID = 0)
        {
            ViewBag.orderID = orderID;
            if (orderID != 0)
            {
                //订单详情 + 店铺
                Order order = work.OrderRepository.Get(m => m.ID == orderID).FirstOrDefault();
                ViewBag.Order = order;
                UserShop userShopModel = new UserShop();
                if (order != null)
                {
                    userShopModel = work.UserShopRepository.Get(m => m.ID == order.UserShopID).FirstOrDefault();
                    ViewBag.UserShop = userShopModel;
                }

                #region 待发货物品列表

                //未配送
                List<OrderDetailVModel> listOrderDetailV = work.Context.OrderDetails
                   .Join(work.Context.Goods, od => od.GoodsID, g => g.ID, (od, g) => new { od, g })
                   .Where(m => m.od.OrderID == orderID && m.od.OD_ShippingStatus == 0).Select(m => new OrderDetailVModel
                   {
                       OrderDetail = m.od,
                       PhotoUrl = m.g.G_Image
                   }).Distinct().ToList();

                #endregion

                ViewBag.OrderDetails = listOrderDetailV;

                return View(new OrderShipping());
            }
            return RedirectToAction("Order");
        }
        /// <summary>
        /// 订单发货-提交
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OrderShippingSave(OrderShipping newModel, int orderID = 0)
        {
            ViewBag.orderID = orderID;
            if (orderID != 0)
            {
                if (ModelState.IsValid)
                {
                    //保存发货记录
                    work.OrderShippingRepository.Insert(newModel);
                    work.Save();

                    //更新订单及订单详细发货状态
                    Int32[] idsArr = UtilityClass.ConvertIntArr(newModel.OrderDetailIds);
                    OrderDetailService.UpdateShippingStatus(idsArr, Convert.ToInt32(DataConfig.OrderShippingStatusEnum.已发货));
                    if (OrderDetailService.ExistShippingStatus(newModel.OrderID, Convert.ToInt32(DataConfig.OrderShippingStatusEnum.待发货)))
                    {
                        OrderService.UpdateShippingStatus(newModel.OrderID, Convert.ToInt32(DataConfig.OrderShippingStatusEnum.部分发货));
                    }
                    else
                    {
                        OrderService.UpdateShippingStatus(newModel.OrderID, Convert.ToInt32(DataConfig.OrderShippingStatusEnum.已发货));
                    }
                    LogService.Add(ManagerService.GetLoginModel(), "订单发货:" + orderID, newModel.ID.ToString());

                    work.Dispose();
                }
            }
            return RedirectToAction("OrderShippingList", new { orderID });
        }

        #endregion

        #region 确认收货

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="orderShippingId">订单运送ID</param>
        /// <param name="ID">订单ID</param>
        /// <returns></returns>
        public ActionResult OrderDetailReceipt(int orderShippingId = 0, int orderID = 0)
        {
            if (orderShippingId != 0)
            {
                OrderShipping model = work.OrderShippingRepository.Get(m => m.OrderID == orderID && m.ID == orderShippingId).FirstOrDefault();

                if (model != null)
                {
                    //修改 订单包裹运送状态及时间
                    model.OS_DeliveryTime = DateTime.Now;
                    model.OS_Status = Convert.ToInt32(DataConfig.OrderShippingDeliveryStatusEnum.已签收);
                    work.OrderShippingRepository.Update(model);
                    work.Save();

                    //修改物品配送状态，订单详细id
                    OrderDetailService.UpdateShippingStatus(UtilityClass.ConvertIntArr(model.OrderDetailIds), Convert.ToInt32(DataConfig.OrderShippingStatusEnum.已收货));
                    if (OrderDetailService.ExistShippingStatus(orderID, Convert.ToInt32(DataConfig.OrderShippingStatusEnum.待发货)) || OrderDetailService.ExistShippingStatus(orderID, Convert.ToInt32(DataConfig.OrderShippingStatusEnum.已发货)))
                    {
                        //OrderService.UpdateShippingStatus(ID, Convert.ToInt32(DataConfig.OrderShippingStatusEnum.部分发货));
                    }
                    else
                    {
                        OrderService.UpdateShippingStatus(orderID, Convert.ToInt32(DataConfig.OrderShippingStatusEnum.已收货));
                    }

                    LogService.Add(ManagerService.GetLoginModel(), "确认收货:" + orderID, orderShippingId.ToString());

                    work.Dispose();
                }
            }
            return RedirectToAction("OrderShippingList", new { orderID = orderID });
        }

        #endregion

        #region 订单详细——修改成本价格

        /// <summary>
        /// 修改成本价格
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="orderDetailID"></param>
        /// <param name="price">默认原价格</param>
        /// <returns></returns>
        public ActionResult OrderDetailChangeCostPrice(int orderID = 0, int orderDetailID = 0, decimal price = 0)
        {
            if (orderDetailID == 0)
            {
                return RedirectToAction("Order");
            }
            ViewBag.orderID = orderID;
            ViewBag.orderDetailID = orderDetailID;
            ViewBag.Price = price;

            ViewBag.List = work.Context.OrderDetailLogs.AsNoTracking().Where(m => m.OrderID == orderDetailID).OrderByDescending(m => m.ID).ToList();

            return View(new OrderChangeAmountVModel());
        }
        /// <summary>
        /// 修改订单金额-post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OrderDetailChangeCostPrice(OrderChangePriceVModel newModel, int orderID, int orderDetailID)
        {
            if (orderID == 0)
            {
                return RedirectToAction("Order");
            }
            ViewBag.Price = newModel.New_Price;
            ViewBag.orderID = orderID;

            string content = newModel.Remark + string.Format("（原成本价：{0}元 -> 新成本价：{1}元）", newModel.Old_Price, newModel.New_Price);
            OrderDetailLogService.Add(ManagerService.GetLoginModel(), content, orderID, orderDetailID, 0);
            OrderDetail orderDetailModel = work.Context.OrderDetails.Where(m => m.ID == orderID).First();
            orderDetailModel.OD_TotalCostPrice = newModel.New_Price;
            work.OrderDetailRepository.Update(orderDetailModel);
            work.Save();

            ViewBag.List = work.Context.OrderChangeLogs.AsNoTracking().Where(m => m.OrderID == orderID).OrderByDescending(m => m.ID).ToList();
            ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功！");

            work.Dispose();

            return View(newModel);
        }

        #endregion

        #region 订单结算操作页面

        [CheckPermission]
        public ActionResult OrderSettlement(int orderID = 0)
        {
            ViewBag.orderID = orderID;
            ViewBag.Order = work.OrderRepository.GetByID(orderID);
            return View();
        }
        [CheckPermission]
        public ActionResult OrderSettlementDo(int orderID = 0)
        {
            ViewBag.orderID = orderID;
            ViewBag.Order = work.OrderRepository.GetByID(orderID);

            int rstStatus = OrderService.OrderSettlement(orderID);
            if (rstStatus == 1)
            {
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "结算已完成");
            }
            else if (rstStatus == 0)
            {
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("warning", "没有可结算订单");
            }
            return View("OrderSettlement");
        }

        #endregion

        #region 订单结算记录

        [CheckPermission]
        public ActionResult SettlementLog(string keyword = "", int ShopUserID = 0, int SupplierUserID = 0, int page = 1, string action = "", string time_start = "", string time_end = "")
        {
            ViewBag.ShopUserID = ShopUserID;
            ViewBag.SupplierUserID = SupplierUserID;
            ViewBag.action = action;
            ViewBag.keyword = keyword;
            ViewBag.time_start = time_start;
            ViewBag.time_end = time_end;
            ViewBag.UserShops = work.UserShopRepository.Get();
            ViewBag.Suppliers = work.SupplierRepository.Get();
            //调整记录
            int pageSize = 12;
            var rst = work.Context.UserAmountHistorys.Where(m => m.Category == "结算")
                .Join(work.Context.Users, uah => uah.UserID, u => u.ID, (uah, u) => new { uah, u })
                .Select(m => new UserAmountSettlementVModel
                {
                    User = m.u,
                    UserAmountHistory = m.uah
                });

            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.UserAmountHistory.Thing.Contains(keyword) || m.User.U_UserName.Contains(keyword));
            }
            if (ShopUserID != 0)
            {
                rst = rst.Where(m => m.UserAmountHistory.UserID == ShopUserID);
            }
            if (SupplierUserID != 0)
            {
                rst = rst.Where(m => m.UserAmountHistory.UserID == SupplierUserID);
            }
            if (!string.IsNullOrEmpty(time_start))
            {
                DateTime timeStart = Convert.ToDateTime(time_start);
                rst = rst.Where(m => m.UserAmountHistory.Time > timeStart);
            }
            if (!string.IsNullOrEmpty(time_end))
            {
                DateTime timeEnd = Convert.ToDateTime(time_end).AddDays(1);
                rst = rst.Where(m => m.UserAmountHistory.Time < timeEnd);
            }
            rst = rst.OrderByDescending(m => m.UserAmountHistory.ID);

            return View(rst.ToPagedList(page, pageSize));
        }

        #endregion

        #region 修改【运费】

        /// <summary>
        /// 修改【运费】
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderChangePostfee(int orderID = 0, decimal postfee = 0, int onlyShow = 0)
        {
            if (orderID == 0)
            {
                return RedirectToAction("Order");
            }
            ViewBag.orderID = orderID;
            ViewBag.Old_Price = postfee;
            ViewBag.onlyShow = onlyShow;

            ViewBag.List = work.Context.OrderChangeLogs.AsNoTracking().Where(m => m.OrderID == orderID).OrderByDescending(m => m.ID).ToList();

            return View(new OrderChangePriceVModel());
        }
        /// <summary>
        /// 修改【运费】-post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OrderChangePostfee(OrderChangePriceVModel newModel, int orderID, int onlyShow = 0)
        {
            if (orderID == 0)
            {
                return RedirectToAction("Order");
            }
            ViewBag.Old_Price = newModel.New_Price;
            ViewBag.orderID = orderID;
            ViewBag.onlyShow = onlyShow;

            string content = newModel.Remark + string.Format("（原运费：{0}元 -> 新运费：{1}元）", newModel.Old_Price, newModel.New_Price);
            OrderChangeLogService.Add(ManagerService.GetLoginModel(), content, orderID);
            Order orderModel = work.Context.Orders.Where(m => m.ID == orderID).First();
            decimal old_postfee = orderModel.O_PostFee;
            decimal new_postfee = newModel.New_Price;

            orderModel.O_TotalAmount = orderModel.O_TotalAmount - old_postfee + new_postfee;
            orderModel.O_NeedPayAmount = orderModel.O_NeedPayAmount - old_postfee + new_postfee;
            orderModel.O_PayAmount = orderModel.O_PayAmount - old_postfee + new_postfee;
            orderModel.O_PostFee = new_postfee;

            work.OrderRepository.Update(orderModel);
            work.Save();

            ViewBag.List = work.Context.OrderChangeLogs.AsNoTracking().Where(m => m.OrderID == orderID).OrderByDescending(m => m.ID).ToList();
            ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功！");

            work.Dispose();

            return View(newModel);
        }

        #endregion
    }
}