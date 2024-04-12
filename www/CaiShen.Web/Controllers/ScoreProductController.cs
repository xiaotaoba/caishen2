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
using Pannet.DAL.Repository;

namespace Pannet.Web.Controllers
{
    public class ScoreProductController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 积分礼品

        //默认积分礼品列表
        [CheckPermission]
        public ActionResult Index(int page = 1)
        {
            var rst = work.Context.ScoreProducts.AsQueryable();
            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
            //return View(work.ScoreProductRepository.Get());
        }

        /// <summary>
        /// 添加/编辑页面
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [ValidateInput(false)]
        public ActionResult Add(int ID = 0)
        {
            ViewBag.UserShops = work.UserShopRepository.Get();

            if (ID != 0)
            {
                var model = work.ScoreProductRepository.Get(m => m.ID == ID).FirstOrDefault<ScoreProduct>();
                return View(model);
            }
            return View(new ScoreProduct());
        }

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(ScoreProduct newModel, int ID = 0)
        {
            ViewBag.UserShops = work.UserShopRepository.Get();

            if (ModelState.IsValid)
            {
                if (newModel.SP_IsFreeShipping == 1)//包邮
                {
                    newModel.SP_PostFee = 0;
                }

                if (ID == 0)//新增
                {
                    var role = work.ScoreProductRepository.Get(m => m.SP_Name == newModel.SP_Name);
                    if (role.Count() > 0)
                    {
                        ModelState.AddModelError("SP_Name", "积分礼品名称已存在");
                    }
                    else
                    {
                        work.ScoreProductRepository.Insert(newModel);
                        work.Save();
                        work.Dispose();

                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    }
                }
                else
                {
                    //var oldModel = work.ScoreProductRepository.Get(m => m.ID == ID).FirstOrDefault<ScoreProduct>();
                    var existModel = work.Context.ScoreProducts.AsNoTracking().Where(m => m.SP_Name == newModel.SP_Name & m.ID != ID);
                    if (existModel.Count() > 0)
                    {
                        ModelState.AddModelError("GC_Name", "积分礼品名称已存在");
                    }
                    else
                    {
                        work.ScoreProductRepository.Update(newModel);
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
        /// 删除积分礼品
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Delete(int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.ScoreProductRepository.Get(m => m.ID == ID).FirstOrDefault<ScoreProduct>();
                work.ScoreProductRepository.Delete(model);
                work.Save();
                work.Dispose();
                LogService.Add(ManagerService.GetLoginModel(), "删除积分礼品", ID.ToString());
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region 用户积分兑换礼品记录

        /// <summary>
        /// 用户积分兑换礼品记录
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult UserScoreProduct(int page = 1)
        {
            var rst = work.Context.UserScoreProducts
                .Join(work.Context.ScoreProducts, usp => usp.ScoreProductID, sp => sp.ID, (usp, sp) => new { usp, sp })
                .Join(work.Context.Users, usp => usp.usp.UserID, u => u.ID, (usp, u) => new { usp.usp, usp.sp, u })
                .Select(m => new UserScoreProductVmodel
                {
                    UserScoreProduct = m.usp,
                    ScoreProduct = m.sp,
                    U_UserName = m.u.U_UserName
                });
            rst = rst.OrderByDescending(m => m.UserScoreProduct.ID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
            //return View(work.ScoreProductRepository.Get());
        }

        /// <summary>
        /// 删除用户积分兑换礼品记录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult UserScoreProductDelete(int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.UserScoreProductRepository.Get(m => m.ID == ID).FirstOrDefault<UserScoreProduct>();
                model.USP_IsDelete = 1;
                work.UserScoreProductRepository.Update(model);
                //work.UserScoreProductRepository.Delete(model);
                work.Save();
                work.Dispose();

                LogService.Add(ManagerService.GetLoginModel(), "删除用户积分兑换记录", ID.ToString());

            }

            return RedirectToAction("UserScoreProduct");
        }

        /// <summary>
        /// 礼品物流信息填写
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult UserScoreProductShipping(int ID = 0)
        {
            ViewBag.UserShops = work.UserShopRepository.Get();
            if (ID != 0)
            {
                var model = work.UserScoreProductRepository.Get(m => m.ID == ID).FirstOrDefault<UserScoreProduct>();
                return View(model);
            }

            return RedirectToAction("UserScoreProduct");
        }
        /// <summary>
        /// 礼品物流信息填写-保存
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult UserScoreProductShipping(UserScoreProduct newModel, int ID = 0)
        {
            ViewBag.UserShops = work.UserShopRepository.Get();
            if (ModelState.IsValid)
            {
                if (ID != 0)
                {
                    bool isSave = true;
                    if (string.IsNullOrEmpty(newModel.USP_ShippingCompany))
                    {
                        ModelState.AddModelError("USP_ShippingCompany", "请填写快递公司");
                        isSave = false;
                    }
                    if (string.IsNullOrEmpty(newModel.USP_ShippingNo))
                    {
                        ModelState.AddModelError("USP_ShippingNo", "请填写快递单号");
                        isSave = false;
                    }

                    var existModel = work.UserScoreProductRepository.GetByID(ID);
                    if (isSave)
                    {
                        LogService.Add(ManagerService.GetLoginModel(), "积分兑换礼品发货", ID.ToString());


                        existModel.USP_ShippingCompany = newModel.USP_ShippingCompany;
                        existModel.USP_ShippingNo = newModel.USP_ShippingNo;
                        existModel.USP_Status = newModel.USP_Status;

                        work.UserScoreProductRepository.Update(existModel);
                        work.Save();

                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");

                    }
                    return View(existModel);
                }

            }
            return RedirectToAction("UserScoreProduct");
        }


        #endregion
    }
}