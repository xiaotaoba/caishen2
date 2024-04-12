using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class CartService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获取购物车列表

        /// <summary>
        /// 获取购物车
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="ShopID"></param>
        /// <param name="isPurchase">默认0购物车，1进货</param>
        /// <returns></returns>
        public static List<Cart> GetCarts(int UserID = 0, int ShopID = 0, int isPurchase = 0)
        {
            var rst = work.Context.Carts.Where(m => m.UserID == UserID);
            if (ShopID != 0)
            {
                rst = rst.Where(m => m.ShopID == ShopID);
            }
            if (isPurchase == 1)
            {
                rst = rst.Where(m => m.IsPurchase == isPurchase);
            }
            else
            {
                rst = rst.Where(m => m.IsPurchase == 0);
            }

            return rst.ToList();
        }

        /// <summary>
        /// 获取用户购物车+包含产品图片
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="shopID"></param>
        /// <param name="isPurchase">默认0购物车，1进货</param>
        /// <returns></returns>
        public static List<CartVModel> GetCartVList(int userID, int shopID, int isPurchase = 0)
        {
            var rst = work.Context.Carts.Join(work.Context.Goods, c => c.GoodsID, g => g.ID, (c, g) => new
            {
                c,
                g.G_Name,
                g.G_Image
            }).Where(m => m.c.UserID == userID);
            if (shopID != 0)
            {
                rst = rst.Where(m => m.c.ShopID == shopID);
            }
            if (isPurchase == 1)
            {
                rst = rst.Where(m => m.c.IsPurchase == isPurchase);
            }
            else
            {
                rst = rst.Where(m => m.c.IsPurchase == 0);
            }
            List<CartVModel> listV = rst.Select(m => new CartVModel
            {
                Cart = m.c,
                Title = m.G_Name,
                PhotoUrl = m.G_Image
            }).OrderByDescending(m => m.Cart.ID).ToList();

            //主图处理
            listV = CartService.GetCartVModelWidthPhoto(listV);

            return listV;
        }

        #endregion

        #region 获取购物车SKU数量
        /// <summary>
        /// 获取购物车SKU数量
        /// </summary>
        /// <returns></returns>
        public static int GetSKUCount(int UserID = 0, int ShopID = 0)
        {
            if (ShopID == 0)
            {
                return work.CartRepository.Get(m => m.UserID == UserID, null).Count();
            }
            else
            {
                return work.CartRepository.Get(m => m.UserID == UserID & m.ShopID == ShopID, null).Count();
            }
        }

        #endregion

        #region 获取购物车产品数量

        /// <summary>
        /// 获取购物车产品数量
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="ShopID"></param>
        /// <param name="isPurchase">默认0购物车，1进货</param>
        /// <returns></returns>
        public static int GetCount(int UserID, int ShopID, int isPurchase = 0)
        {
            if (UserID == 0)
            {
                return 0;
            }
            var rst = work.Context.Carts.Join(work.Context.Goods, c => c.GoodsID, g => g.ID, (c, g) => new { c }).Select(m => m.c);
            if (isPurchase == 1)
            {
                rst = rst.Where(m => m.IsPurchase == isPurchase);
            }
            else
            {
                rst = rst.Where(m => m.IsPurchase == 0);
            }
            rst = rst.Where(m => m.UserID == UserID);
            //return work.CartRepository.Get(m => m.UserID == UserID & m.ShopID == ShopID, null).Select(m => m.Count).Sum();
            if (rst == null || rst.Count() < 1)
            {
                return 0;
            }
            else
            {
                return rst.Where(m => m.Count != null).Select(m => m.Count).Sum();
            }
        }

        #endregion

        #region 移出购物车

        /// <summary>
        /// 移出购物车
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool Delete(int ID)
        {
            if (ID != 0)
            {
                var model = work.CartRepository.Get(m => m.ID == ID).FirstOrDefault<Cart>();
                work.CartRepository.Delete(model);
                work.Save();
            }
            return true;

        }
        /// <summary>
        /// 批量移出购物车
        /// </summary>
        /// <param name="cartids"></param>
        /// <returns></returns>
        public static bool DeleteBatch(string cartids)
        {
            if (cartids != "")
            {
                cartids = cartids.Trim(',');
                List<string> ids = cartids.Split(',').ToList();
                foreach (var item in ids)
                {
                    int id = Convert.ToInt32(item);
                    var model = work.CartRepository.Get(m => m.ID == id).FirstOrDefault<Cart>();
                    work.CartRepository.Delete(model);
                }
                work.Save();
            }
            return true;
        }

        #endregion

        #region 转换成 整型 cartid数组

        /// <summary>
        /// 逗号隔开的cartidz字符串 转换成 整型 cartid数组
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public static Int32[] GetCartIds(string cartidsStr)
        {
            try
            {
                //if (string.IsNullOrEmpty(cartidsStr))
                //{
                //    return new Int32[] { 0 };
                //}
                cartidsStr = cartidsStr.Trim(',');
                string[] splitStr = { "," };
                string[] cartidsArr = cartidsStr.Split(splitStr, StringSplitOptions.RemoveEmptyEntries);
                Int32[] cartids = new Int32[cartidsArr.Length];
                for (int i = 0; i < cartidsArr.Length; i++)
                {
                    cartids[i] = Convert.ToInt32(cartidsArr[i]);
                }
                return cartids;
            }
            catch
            {
                throw new Exception("参数错误！");
            }
        }

        #endregion

        #region 主图处理

        /// <summary>
        /// 主图处理
        /// </summary>
        /// <param name="listV"></param>
        /// <returns></returns>
        public static List<CartVModel> GetCartVModelWidthPhoto(List<CartVModel> listV)
        {
            List<int> goodsIds = listV.Select(m => m.Cart.GoodsID).ToList();
            //返回产品的主图
            List<GoodsPhoto> GoodsPhotoList = work.Context.GoodsPhotos.Where(m => goodsIds.Contains(m.GoodsID)).ToList();
            foreach (var item in listV)
            {
                GoodsPhoto photoModel = GoodsPhotoList.Where(m => m.GoodsID == item.Cart.GoodsID).OrderByDescending(m => m.GP_IsFirst).FirstOrDefault();
                if (photoModel != null)
                {
                    item.PhotoUrl = photoModel.GP_Image;
                }
            }
            return listV;
        }

        #endregion

        #region 加入购物车

        /// <summary>
        /// 加入购物车
        /// </summary>
        /// <param name="LoginedUserModel">当前登录用户</param>
        /// <param name="cart">单个购物车实体</param>
        /// <param name="isPurchase">是否为加盟商进货，0购物车，1进货</param>
        /// <returns></returns>
        public static object Add(User LoginedUserModel, Cart cart, int isPurchase = 0)
        {
            if (LoginedUserModel == null)
            {
                return new { status = "-1", msg = "请先登录!" };
            }
            var rst = work.Context.Carts
                                .Join(work.Context.Goods, c => c.GoodsID, g => g.ID, (c, g) => new { c, g })
                                .Where(m => m.c.UserID == LoginedUserModel.ID && m.c.ShopID == cart.ShopID && m.c.SKUID == cart.SKUID && m.c.IsHasDesignFile == cart.IsHasDesignFile && m.c.PropertiesName == cart.PropertiesName && m.c.IsPurchase == isPurchase).FirstOrDefault();
            if (rst == null || rst.c == null)//不存在，新增
            {
                cart.UserID = LoginedUserModel.ID;
                cart.IsPurchase = isPurchase;

                //cart.HiddenShippingFee = 0;
                work.CartRepository.Insert(cart);
                work.Save();
            }
            else//存在更新
            {
                Cart existModel = rst.c;
                if (existModel != null && existModel.ID != 0)
                {
                    existModel.Count = existModel.Count + cart.Count;
                    existModel.GoodsCount = existModel.GoodsCount + cart.GoodsCount;

                    //新总费用= 原总费用 + 新增产品总价 
                    existModel.CartTotalPrice = existModel.CartTotalPrice + cart.CartTotalPrice;
                    existModel.TotalShopPrice = existModel.TotalShopPrice + cart.TotalShopPrice;//累加加盟商价格
                    existModel.TotalCostPrice = existModel.TotalCostPrice + cart.TotalCostPrice;//累加成本价格
                    existModel.HiddenShippingFee = 0;

                    cart.ID = existModel.ID;

                    work.CartRepository.Update(existModel);
                    work.Save();
                }
            }
            //return View();
            return new { status = "success", msg = "添加成功!", cartid = cart.ID };
        }

        #endregion

        #region 立即购买，如果购物车有就不再加入

        /// <summary>
        /// 立即购买，如果购物车有就不再加入
        /// </summary>
        /// <param name="LoginedUserModel">当前登录用户</param>
        /// <param name="cart">单个购物车实体</param>
        /// <param name="isPurchase">是否为加盟商进货，0购物车，1进货</param>
        /// <returns></returns>
        public static object BuyNowAdd(User LoginedUserModel, Cart cart, int isPurchase = 0)
        {
            if (LoginedUserModel == null)
            {
                return new { status = "-1", msg = "请先登录!" };
            }
            else if (LoginedUserModel.UserRoleID != Convert.ToInt16(DataConfig.RoleEnum.加盟店) && isPurchase == 1)//非加盟商转账进货清单
            {
                return new { status = "-1", msg = "权限不足!" };
            }
            Cart existModel = work.Context.Carts.Where(m => m.GoodsID == cart.GoodsID && m.UserID == LoginedUserModel.ID && m.ShopID == cart.ShopID && m.SKUID == cart.SKUID && m.IsHasDesignFile == cart.IsHasDesignFile && m.PropertiesName == cart.PropertiesName && m.IsPurchase == isPurchase).FirstOrDefault();
            if (existModel != null && existModel.ID != 0)//存在替换更新
            {
                cart.ID = existModel.ID;
                existModel.Count = cart.Count;
                existModel.CartTotalPrice = cart.CartTotalPrice;
                existModel.DesignFee = cart.DesignFee;
                existModel.GoodsCount = cart.GoodsCount;
                existModel.TotalShopPrice = cart.TotalShopPrice;
                existModel.TotalCostPrice = cart.TotalCostPrice;
                existModel.HiddenShippingFee = cart.HiddenShippingFee;

                work.CartRepository.Update(existModel);
                work.Save();

                return new { status = "success", msg = "立即购买!", cartid = cart.ID };
            }
            else//新增
            {
                cart.UserID = LoginedUserModel.ID;
                cart.IsPurchase = isPurchase;
                //cart.HiddenShippingFee = 0;

                work.CartRepository.Insert(cart);
                work.Save();

                return new { status = "success", msg = "添加成功!", cartid = cart.ID };
            }

        }

        #endregion

        #region 购物车修改数量

        /// <summary>
        /// 购物车修改数量
        /// </summary>
        /// <param name="LoginedUserModel">当前登录用户</param>
        /// <param name="id">购物车ID</param>
        /// <param name="count">购买SKU数量</param>
        /// <returns></returns>
        public static object UpdateCount(User LoginedUserModel, int id, int count)
        {
            if (LoginedUserModel == null)
            {
                return new { status = "-1", msg = "请先登录!" };
            }
            var rst = work.Context.Carts
                      .Join(work.Context.Goods, c => c.GoodsID, g => g.ID, (c, g) => new { c, g })
                      .Where(m => m.c.ID == id).FirstOrDefault();
            if (rst == null || rst.c == null)//不存在
            {
                return new { status = "err", msg = "请求失败!", cartid = id };
            }
            else//存在更新
            {
                Cart existModel = rst.c;
                if (existModel != null && existModel.ID != 0)//存在更新
                {
                    int unitCount = existModel.GoodsCount / existModel.Count;//单个sku产品数量

                    decimal unitPrice = existModel.CartTotalPrice / existModel.Count;//单个sku产品价格 = （总价-设计费-隐藏运费）/SKU数量

                    existModel.HiddenShippingFee = 0;
                    existModel.CartTotalPrice = unitPrice * count;

                    //加盟商价格
                    decimal unitShopPrice = existModel.TotalShopPrice / existModel.Count;
                    existModel.TotalShopPrice = unitShopPrice * count;
                    //成本价格
                    decimal unitCostPrice = existModel.TotalCostPrice / existModel.Count;
                    existModel.TotalCostPrice = unitCostPrice * count;

                    existModel.Count = count;
                    existModel.GoodsCount = unitCount * count;

                    work.CartRepository.Update(existModel);
                    work.Save();
                }
                return new { status = "success", msg = "修改成功!", cartid = id, unitprice = existModel.CartTotalPrice / count, totalprice = existModel.CartTotalPrice };
            }
        }

        #endregion

        #region 获取购物车统计数据

        /// <summary>
        /// 获取购物车统计数据
        /// </summary>
        /// <param name="LoginedUserModel">当前登录用户</param>
        /// <param name="cart">单个购物车实体</param>
        /// <param name="isPurchase">是否为加盟商进货，0购物车，1进货</param>
        /// <returns></returns>
        public static object GetCartTJ(User LoginedUserModel, Cart cart, int isPurchase = 0)
        {
            if (LoginedUserModel == null)
            {
                return new { status = "-1", msg = "请先登录!" };
            }
            //else if (LoginedUserModel.UserRoleID != Convert.ToInt16(DataConfig.RoleEnum.加盟店) && isPurchase == 1)//非加盟商转账进货清单
            //{
            //    return new { status = "-1", msg = "权限不足!" };
            //}

            var rst = work.Context.Carts.Join(work.Context.Goods, c => c.GoodsID, g => g.ID, (c, g) => new { c }).Select(m => m.c).Where(m => m.UserID == LoginedUserModel.ID);
            //if (cart.ShopID != 0)
            //{
            //    rst = rst.Where(m => m.ShopID == cart.ShopID);
            //}
            //if (isPurchase == 1)
            //{
            //    rst = rst.Where(m => m.IsPurchase == isPurchase);
            //}
            //else
            //{
            rst = rst.Where(m => m.IsPurchase == 0);
            //}
            return new { status = "success", msg = "", totalPrice = rst.Sum(m => m.CartTotalPrice), totalCount = rst.Sum(m => m.Count) };
        }

        #endregion

        #region 指定购物车ID信息列表

        /// <summary>
        /// 指定购物车ID信息列表
        /// </summary>
        /// <returns></returns>
        public static List<CartVModel> GetCartVList(int[] cartids)
        {
            var rst = work.Context.Carts.Join(work.Context.Goods, c => c.GoodsID, g => g.ID, (c, g) => new
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
                      }).OrderByDescending(m => m.Cart.ID)
                      .ToList();
            return CartService.GetCartVModelWidthPhoto(rst);
        }

        /// <summary>
        /// 指定购物车ID商品信息列表
        /// </summary>
        /// <returns></returns>
        public static List<Goods> GetGoodsList(int[] cartids)
        {
            var rst = work.Context.Carts.Join(work.Context.Goods, c => c.GoodsID, g => g.ID, (c, g) => new
            {
                c,
                g
            }).Where(m => cartids.Contains(m.c.ID))
                      .Select(m => m.g)
                      .ToList();
            return rst;
        }

        #endregion

        #region 获取购物车指定产品总价格，不包含运费

        /// <summary>
        /// 获取购物车指定产品总价格，不包含运费
        /// </summary>
        /// <returns></returns>
        public static decimal GetTotalGoodsAmount(List<CartVModel> cartList)
        {
            decimal total_amount = 0;
            foreach (var item in cartList)
            {
                total_amount = total_amount + item.Cart.CartTotalPrice;
            }
            return total_amount;
        }

        #endregion
    }
}
