using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Models;
using Pannet.DAL;
using Pannet.Utility;
using PagedList;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Migrations;
using Pannet.Web.Attribute;
using Pannet.DAL.Repository;
//using System.ComponentModel.DataAnnotations;

namespace Pannet.Web.Controllers
{
    public class ShopController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();
        private int ShopRoleID = Convert.ToInt16(DataConfig.RoleEnum.加盟店);

        #region 门店信息列表

        [CheckPermission]
        public ActionResult Index(string field = "shopname", string keyword = "", int page = 1)
        {
            ViewBag.Field = field;
            ViewBag.Keyword = keyword;

            var rst = work.Context.UserShops.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                switch (field)
                {
                    case "shopname": rst = rst.Where(m => m.Shop_Name.Contains(keyword)); break;
                    case "shopnumber": rst = rst.Where(m => m.Shop_Number.Contains(keyword)); break;
                    case "username": rst = rst.Where(m => m.Shop_UserName.Contains(keyword)); break;
                    case "tel": rst = rst.Where(m => m.Shop_Tel.Contains(keyword)); break;
                    default: break;
                };
            }
            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        #endregion

        #region 新增门店信息

        /// <summary>
        /// 新增门店信息
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Add()
        {
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.ShopUser = work.UserRepository.Get(m => m.UserRoleID == ShopRoleID);

            return View(new UserShop());
        }

        /// <summary>
        ///  新增门店信息-post
        /// </summary>
        /// <param name="newModel"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission]
        public ActionResult Add(UserShop newModel)
        {
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.ShopUser = work.UserRepository.Get(m => m.UserRoleID == ShopRoleID);

            if (ModelState.IsValid)
            {
                var existModel = work.UserShopRepository.Get(m => m.Shop_Name == newModel.Shop_Name);
                if (existModel.Count() > 0)
                {
                    ModelState.AddModelError("Shop_Name", string.Format("店铺名称{0}已存在！", newModel.Shop_Name));
                }
                else
                {
                    var existModel2 = work.UserShopRepository.Get(m => m.Shop_Number == newModel.Shop_Number);
                    if (existModel2.Count() > 0)
                    {
                        ModelState.AddModelError("Shop_Number", string.Format("店铺编号{0}已存在！", newModel.Shop_Number));
                    }
                    else
                    {
                        //newModel.Shop_CreateTime = DateTime.Now;

                        work.UserShopRepository.Insert(newModel);

                        work.Save();
                        work.Dispose();
                        LogService.Add(ManagerService.GetLoginModel(), "新增门店：" + newModel.Shop_Name, newModel.ID.ToString());
                    }
                }
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            }

            return View(newModel);
        }

        #endregion

        #region 编辑门店信息

        /// <summary>
        /// 资料编辑
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Edit(int ID = 0)
        {
            ViewBag.ID = ID;
            ViewBag.ShopUser = work.UserRepository.Get(m => m.UserRoleID == ShopRoleID);

            if (ID != 0)
            {
                ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
                UserShop oldModel = work.UserShopRepository.Get(m => m.ID == ID).FirstOrDefault<UserShop>();
                if (oldModel != null)
                {
                    ViewBag.Citys = work.AreaRepository.Get(m => m.Area_ParentID == oldModel.Shop_Province);
                    ViewBag.Regions = work.AreaRepository.Get(m => m.Area_ParentID == oldModel.Shop_City);
                }

                return View(oldModel);
            }
            else
            {
                return RedirectToAction("Add");
            }
        }

        /// <summary>
        /// 资料编辑-post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission]
        public ActionResult Edit(UserShop newModel)
        {
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.ShopUser = work.UserRepository.Get(m => m.UserRoleID == ShopRoleID);
            ViewBag.Citys = work.AreaRepository.Get(m => m.Area_ParentID == newModel.Shop_Province);
            ViewBag.Regions = work.AreaRepository.Get(m => m.Area_ParentID == newModel.Shop_City);

            if (ModelState.IsValid)
            {
                //UserShop oldModel = work.UserShopRepository.Get(m => m.ID == newModel.ID).FirstOrDefault<UserShop>();
                UserShop oldModel = work.Context.UserShops.AsNoTracking().Where(m => m.ID == newModel.ID).FirstOrDefault<UserShop>();
                if (oldModel != null)
                {

                    if (oldModel.Shop_Name != newModel.Shop_Name)//修改店铺名称
                    {
                        var existModel = work.UserShopRepository.Get(m => m.Shop_Name == newModel.Shop_Name);
                        if (existModel.Count() > 0)
                        {
                            ModelState.AddModelError("Shop_Name", string.Format("店铺名称{0}已存在！", newModel.Shop_Name));
                            return View(newModel);
                        }
                        //else
                        //{
                        //    oldModel.Shop_Name = newModel.Shop_Name;
                        //}
                    }
                    if (oldModel.Shop_Number != newModel.Shop_Number)//修改店铺编号
                    {
                        var existUser = work.UserShopRepository.Get(m => m.Shop_Number == newModel.Shop_Number);
                        if (existUser.Count() > 0)
                        {
                            ModelState.AddModelError("Shop_Number", string.Format("店铺编号{0}已存在！", newModel.Shop_Number));
                            return View(newModel);
                        }
                        //else
                        //{
                        //    oldModel.Shop_Number = newModel.Shop_Number;
                        //}
                    }
                }

                //oldModel.Shop_Address = newModel.Shop_Address;
                //oldModel.Shop_City = newModel.Shop_City;
                //oldModel.Shop_CloseReason = newModel.Shop_CloseReason;
                ////oldModel.Shop_CreateTime = newModel.Shop_CreateTime;
                //oldModel.Shop_Desc = newModel.Shop_Desc;
                //oldModel.Shop_Is_Enable = newModel.Shop_Is_Enable;
                //oldModel.Shop_Province = newModel.Shop_Province;
                //oldModel.Shop_Region = newModel.Shop_Region;
                //oldModel.Shop_Tel = newModel.Shop_Tel;
                //oldModel.Shop_URL = newModel.Shop_URL;
                //oldModel.Shop_UserName = newModel.Shop_UserName;
                //oldModel.UserID = newModel.UserID;
                //oldModel.Shop_SaleGoodsWay = newModel.Shop_SaleGoodsWay;
                //oldModel.Shop_QQ = newModel.Shop_QQ;
                //oldModel.Shop_Weixin = newModel.Shop_Weixin;
                //oldModel.Shop_WeixinQrCode = newModel.Shop_WeixinQrCode;
                //oldModel.Shop_IsZiti = newModel.Shop_IsZiti;
                //oldModel.Shop_IsYufu = newModel.Shop_IsYufu;
                //oldModel.Shop_Yufu_Percent = newModel.Shop_IsYufu;
                //oldModel.TotalGuaranteeMoney = newModel.TotalGuaranteeMoney;
                //oldModel.RestGuaranteeMoney = newModel.RestGuaranteeMoney;


                work.UserShopRepository.Update(newModel);

                work.Save();
                work.Dispose();

                LogService.Add(ManagerService.GetLoginModel(), "编辑门店：" + newModel.Shop_Name, newModel.ID.ToString());
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            }
            //return JavaScript("alert('修改成功!');");

            return View(newModel);
        }

        #endregion

        #region 删除
        /// <summary>
        /// 删除门店
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
                var model = work.UserShopRepository.Get(m => m.ID == ID).FirstOrDefault<UserShop>();
                work.UserShopRepository.Delete(model);
                work.Save();
                work.Dispose();
                LogService.Add(ManagerService.GetLoginModel(), "删除门店", ID.ToString());

            }
            return RedirectToAction("Index");
        }

        #endregion

        #region 门店商品管理


        /// <summary>
        /// 商品管理，出售中商品
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult ShopGoods(int UserShopID = 0, string field = "name", string keyword = "", int GoodsCategoryID = 0, int page = 1)
        {
            //当前用户店铺
            UserShop userShopModel = work.UserShopRepository.Get(m => m.ID == UserShopID).FirstOrDefault<UserShop>();
            ViewBag.UserShop = userShopModel;

            if (userShopModel != null)
            {
                ViewBag.Field = field;
                ViewBag.Keyword = keyword;
                ViewBag.GoodsCategorys = work.GoodsCategoryRepository.Get();
                ViewBag.GoodsCategoryID = GoodsCategoryID;

                var rst = work.Context.Goods
                    //.Join(work.Context.UserShopGoodsDowns.DefaultIfEmpty(), g => g.ID, gd => gd.GoodsID, (g, gd) => new { gd, g }).DefaultIfEmpty();
                        .Where(m => m.UserShopID == 0);
                //.Where(m => (Int32?)m.gd.ShopID == );
                if (GoodsCategoryID != 0)
                {
                    rst = rst.Where(m => m.GoodsCategoryID == GoodsCategoryID);
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    switch (field)
                    {
                        case "name": rst = rst.Where(m => m.G_Name.Contains(keyword)); break;
                        case "number": rst = rst.Where(m => m.G_Number.Contains(keyword)); break;
                        default: break;
                    };
                }

                //下架商品ID
                List<int> rst_down_goodsids = work.Context.UserShopGoodsDowns.Where(m => m.ShopID == userShopModel.ID).Select(m => m.GoodsID).ToList();

                List<Goods> goodsList = rst.Where(m => !rst_down_goodsids.Contains(m.ID)).OrderByDescending(m => m.ID).ToList();

                ViewBag.TotalCount = goodsList.Count();

                int pageSize = 20;
                int pageNumber = page;

                return View(goodsList.ToPagedList(pageNumber, pageSize));
            }

            return View();
        }
        //门店出售商品首页
        //public ActionResult ShopGoods(string field = "shopname", string keyword = "", int page = 1)
        //{
        //    ViewBag.Field = field;
        //    ViewBag.Keyword = keyword;

        //    var rst = work.Context.UserShops.AsQueryable();

        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        switch (field)
        //        {
        //            case "shopname": rst = rst.Where(m => m.Shop_Name.Contains(keyword)); break;
        //            case "shopnumber": rst = rst.Where(m => m.Shop_Number.Contains(keyword)); break;
        //            case "username": rst = rst.Where(m => m.Shop_UserName.Contains(keyword)); break;
        //            case "tel": rst = rst.Where(m => m.Shop_Tel.Contains(keyword)); break;
        //            default: break;
        //        };
        //    }
        //    rst = rst.OrderByDescending(m => m.ID);

        //    int pageSize = 20;
        //    int pageNumber = page;
        //    return View(rst.ToPagedList(pageNumber, pageSize));
        //}

        //门店出售商品分类列表
        [CheckPermission]
        public ActionResult ShopGoodsCategory(int ShopID = 0, int page = 1)
        {
            ViewBag.ShopID = ShopID;
            ViewBag.ShopModel = work.UserShopRepository.Get(m => m.ID == ShopID).FirstOrDefault();

            var rst = work.Context.UserShopGoodsCategorys
                .Join(work.Context.GoodsCategorys, sgc => sgc.GoodsCategoryID, gc => gc.ID, (sgc, gc) => new { gc, sgc })
                .Where(m => m.sgc.ShopID == ShopID).Select(m => m.gc);

            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        //门店出售自定义商品列表
        [CheckPermission]
        public ActionResult ShopGoodsList(int ShopID = 0, int page = 1)
        {
            ViewBag.ShopID = ShopID;
            ViewBag.ShopModel = work.UserShopRepository.Get(m => m.ID == ShopID).FirstOrDefault();

            var rst = work.Context.UserShopGoodses
                .Join(work.Context.Goods, sg => sg.GoodsID, g => g.ID, (gs, g) => new { gs, g })
                .Where(m => m.gs.ShopID == ShopID).Select(m => m.g);

            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        #endregion

        #region 调整店铺保障金

        /// <summary>
        /// 调整店铺保障金
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult ChangeShopGuaranTee(int UserShopID = 0, int page = 1)
        {
            //调整记录
            int pageSize = 12;
            int pageNumber = page;
            var rst = work.Context.ShopGuaranteeHistorys.Where(m => m.UserShopID == UserShopID && m.Is_Delete == 0);
            rst = rst.OrderByDescending(m => m.ID);

            ViewBag.UserShop = work.Context.UserShops.Where(m => m.ID == UserShopID).FirstOrDefault<UserShop>();
            ViewBag.ID = UserShopID;

            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 调整店铺保障金-post
        /// </summary>
        /// <param name="UserShopID"></param>
        /// <param name="money">变动额度</param>
        /// <param name="type">1增加，0减少</param>
        /// <param name="thing">备注</param>
        /// <param name="page"></param>
        /// <param name="money_type">额度类型：</param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission]
        public ActionResult ChangeShopGuaranTee(int UserShopID, decimal money, int type, string thing, int page = 1, int money_type = 0)
        {
            //展示调整记录
            int pageSize = 12;
            int pageNumber = page;
            var rst = work.Context.ShopGuaranteeHistorys.Where(m => m.UserShopID == UserShopID && m.Is_Delete == 0);
            rst = rst.OrderByDescending(m => m.ID);

            ViewBag.UserShop = work.Context.UserShops.Where(m => m.ID == UserShopID).FirstOrDefault<UserShop>();
            ViewBag.ID = UserShopID;


            if (ModelState.IsValid)
            {

                if (money <= 0)
                {
                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("warning", "请填写调整额度");
                    ModelState.AddModelError("score", string.Format("请填写调整额度"));
                }
                else if (string.IsNullOrEmpty(thing))
                {
                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("warning", "请填写调整说明");
                }
                else
                {
                    UserShop oldModel = ViewBag.UserShop;
                    //调整保证金额度
                    if (money_type == 0)
                    {
                        if (type == 0)//减少
                        {
                            oldModel.TotalGuaranteeMoney = oldModel.TotalGuaranteeMoney - money;

                            if (oldModel.RestGuaranteeMoney > oldModel.TotalGuaranteeMoney)
                            {
                                oldModel.RestGuaranteeMoney = oldModel.TotalGuaranteeMoney;
                            }
                        }
                        else
                        {
                            oldModel.TotalGuaranteeMoney = oldModel.TotalGuaranteeMoney + money;
                            oldModel.RestGuaranteeMoney = oldModel.RestGuaranteeMoney + money;
                        }

                        //添加调整记录
                        ShopGuaranteeHistoryService.Insert(UserShopID, money, oldModel.TotalGuaranteeMoney, oldModel.RestGuaranteeMoney, type, "调整保障金额度", thing, UserShopID, LoginedAdminModel.UserName);

                    }
                    else
                    { //调整剩余保证金额度

                        if (type == 0)//减少
                        {
                            oldModel.RestGuaranteeMoney = oldModel.RestGuaranteeMoney - money;
                        }
                        else
                        {
                            oldModel.RestGuaranteeMoney = oldModel.RestGuaranteeMoney + money;
                        }
                        if (oldModel.RestGuaranteeMoney > oldModel.TotalGuaranteeMoney)
                        {
                            oldModel.TotalGuaranteeMoney = oldModel.RestGuaranteeMoney;
                        }

                        //添加调整记录
                        ShopGuaranteeHistoryService.Insert(UserShopID, money, oldModel.TotalGuaranteeMoney, oldModel.RestGuaranteeMoney, type, "调整剩余额度", thing, UserShopID, LoginedAdminModel.UserName);
                    }

                    //调整店铺保障金额度
                    work.UserShopRepository.Update(oldModel);
                    work.Save();

                }
            }
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        //删除调整记录
        [CheckPermission]
        public ActionResult ShopGuaranTeeHistoryDelete(int ID = 0, int UserShopID = 0)
        {
            if (ID != 0)
            {
                //if (ID == 1)
                //{
                //    return RedirectToAction("ChangeUserScore", new { UserID = UserID });
                //}
                var m = work.ShopGuaranteeHistoryRepository.Get(g => g.ID == ID).FirstOrDefault<ShopGuaranteeHistory>();
                //work.ShopGuaranteeHistoryRepository.Delete(m);
                m.Is_Delete = 1;
                work.ShopGuaranteeHistoryRepository.Update(m);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("ChangeShopGuaranTee", new { UserShopID = UserShopID });
        }


        #endregion
    }
}