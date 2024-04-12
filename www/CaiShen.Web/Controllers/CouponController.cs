using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Web.Attribute;
using Pannet.Models;
using Pannet.DAL;
using Pannet.Utility;
using PagedList;

namespace Pannet.Web.Controllers
{
    public class CouponController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 优惠券

        //默认优惠券列表
        [CheckPermission]
        public ActionResult Index(int page = 1)
        {
            var rst = work.Context.CouponInfos.AsQueryable();
            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
            //return View(work.CouponInfoRepository.Get());
        }

        /// <summary>
        /// 添加/编辑页面
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Add(int ID = 0)
        {
            ViewBag.UserShops = work.UserShopRepository.Get();
            ViewBag.GoodsCategorys = work.GoodsCategoryRepository.Get();

            if (ID != 0)
            {
                var model = work.CouponInfoRepository.Get(m => m.ID == ID).FirstOrDefault<CouponInfo>();
                return View(model);
            }
            CouponInfo newModel = new CouponInfo();
            newModel.CP_BeginTime = DateTime.Now;
            newModel.CP_EndTime = DateTime.Now;
            newModel.CP_CreateUser = LoginedAdminModel.ID;
            return View(newModel);
        }

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult Add(CouponInfo newModel, int ID = 0)
        {
            ViewBag.UserShops = work.UserShopRepository.Get();
            ViewBag.GoodsCategorys = work.GoodsCategoryRepository.Get();

            if (ModelState.IsValid)
            {
                string usingRangeIds = "";
                if (newModel.CP_UsingRange == Convert.ToInt16(DataConfig.CouponUsingRangeEnum.平台通用类))
                {
                    usingRangeIds = "";
                }
                else if (newModel.CP_UsingRange == Convert.ToInt16(DataConfig.CouponUsingRangeEnum.店铺通用类))
                {
                    usingRangeIds = Request["usingRangeShop"];
                }
                //else if (newModel.CP_UsingRange == Convert.ToInt16(DataConfig.CouponUsingRangeEnum.品类通用类))
                //{
                //    usingRangeIds = Request["usingRangeCategory"];
                //}
                //else if (newModel.CP_UsingRange == Convert.ToInt16(DataConfig.CouponUsingRangeEnum.特定商品使用))
                //{
                //    usingRangeIds = Request["usingRangeGoods"];
                //}
                newModel.CP_UsingRangeIds = usingRangeIds;
                newModel.CP_MaxAmount = newModel.CP_Amount;//最大优惠为面额

                if (ID == 0)//新增
                {
                    var role = work.CouponInfoRepository.Get(m => m.CP_Name == newModel.CP_Name);
                    if (role.Count() > 0)
                    {
                        ModelState.AddModelError("GC_Name", "优惠券名称已存在");
                    }
                    else
                    {
                        work.CouponInfoRepository.Insert(newModel);
                        work.Save();
                        work.Dispose();

                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    }
                }
                else
                {
                    var existModel = work.Context.CouponInfos.AsNoTracking().Where(m => m.CP_Name == newModel.CP_Name & m.ID != ID);
                    if (existModel.Count() > 0)
                    {
                        ModelState.AddModelError("GC_Name", "优惠券名称已存在");
                    }
                    else
                    {

                        work.CouponInfoRepository.Update(newModel);
                        work.Save();
                        work.Dispose();

                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(newModel);
        }

        /// <summary>
        /// 删除优惠券
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Delete(int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.CouponInfoRepository.Get(m => m.ID == ID).FirstOrDefault<CouponInfo>();
                model.CP_IsDelete = 1;
                work.CouponInfoRepository.Update(model);
                //work.CouponInfoRepository.Delete(model);
                work.Save();
                work.Dispose();
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region 用户领取优惠券记录

        /// <summary>
        /// 用户领取优惠券记录
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult UserCoupon(int page = 1)
        {
            var rst = work.Context.UserCoupons
                .Join(work.Context.CouponInfos, uc => uc.CouponInfoID, c => c.ID, (uc, c) => new { uc, c })
                .Join(work.Context.Users, uc => uc.uc.UserID, u => u.ID, (uc, u) => new { uc.uc, uc.c, u })
                .Select(m => new UserCouponVmodel
                {
                    CouponInfo = m.c,
                    UserCoupon = m.uc,
                    U_UserName = m.u.U_UserName
                });
            rst = rst.OrderByDescending(m => m.UserCoupon.ID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
            //return View(work.ScoreProductRepository.Get());
        }

        /// <summary>
        /// 删除用户优惠券记录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult UserCouponDelete(int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.UserCouponRepository.Get(m => m.ID == ID).FirstOrDefault<UserCoupon>();
                model.UCP_IsDelete = 1;
                work.UserCouponRepository.Update(model);
                //work.CouponInfoRepository.Delete(model);
                work.Save();
                work.Dispose();
            }

            return RedirectToAction("UserCoupon");
        }

        #endregion
    }
}