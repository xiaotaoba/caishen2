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
    public class HongBaoController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 红包

        //默认红包列表
        [CheckPermission]
        public ActionResult Index(int page = 1)
        {
            var rst = work.Context.HongBaos.AsQueryable();
            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
            //return View(work.HongBaoRepository.Get());
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

            if (ID != 0)
            {
                var model = work.HongBaoRepository.Get(m => m.ID == ID).FirstOrDefault<HongBao>();
                return View(model);
            }
            HongBao newModel = new HongBao();
            newModel.HB_BeginTime = DateTime.Now;
            newModel.HB_EndTime = DateTime.Now;
            newModel.HB_CreateUser = LoginedAdminModel.ID;
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
        public ActionResult Add(HongBao newModel, int ID = 0)
        {
            ViewBag.UserShops = work.UserShopRepository.Get();

            if (ModelState.IsValid)
            {
                if (newModel.HB_Type == 0)
                {
                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("warning", "请选择红包类型");
                    //ModelState.AddModelError("HB_Type", "请选择红包类型");
                    return View(newModel);
                }
                if (newModel.HB_ValidDate == 0)
                {
                    //ModelState.AddModelError("HB_ValidDate", "请选择有效期");
                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("warning", "请选择有效期");
                    return View(newModel);
                }
                if (ID == 0)//新增
                {
                    var role = work.HongBaoRepository.Get(m => m.HB_Name == newModel.HB_Name);
                    if (role.Count() > 0)
                    {
                        ModelState.AddModelError("HB_Name", "红包名称已存在");
                    }
                    else
                    {
                        work.HongBaoRepository.Insert(newModel);
                        work.Save();
                        work.Dispose();

                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    }
                }
                else
                {
                    var existModel = work.Context.HongBaos.AsNoTracking().Where(m => m.HB_Name == newModel.HB_Name & m.ID != ID);
                    if (existModel.Count() > 0)
                    {
                        ModelState.AddModelError("HB_Name", "红包名称已存在");
                    }
                    else
                    {

                        work.HongBaoRepository.Update(newModel);
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
        /// 删除红包
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Delete(int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.HongBaoRepository.Get(m => m.ID == ID).FirstOrDefault<HongBao>();
                model.HB_IsDelete = 1;
                work.HongBaoRepository.Update(model);
                //work.HongBaoRepository.Delete(model);
                work.Save();
                work.Dispose();
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region 用户领取红包记录

        /// <summary>
        /// 用户领取红包记录
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult UserHongBao(int page = 1)
        {
            var rst = work.Context.UserHongBaos
                .Join(work.Context.HongBaos, uc => uc.HongBaoID, c => c.ID, (uc, c) => new { uc, c })
                .Join(work.Context.Users, uc => uc.uc.UserID, u => u.ID, (uc, u) => new { uc.uc, uc.c, u })
                .Select(m => new UserHongBaoVmodel
                {
                    HongBao = m.c,
                    UserHongBao = m.uc,
                    U_UserName = m.u.U_UserName
                });
            rst = rst.OrderByDescending(m => m.UserHongBao.ID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
            //return View(work.ScoreProductRepository.Get());
        }

        /// <summary>
        /// 删除用户红包记录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult UserHongBaoDelete(int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.UserHongBaoRepository.Get(m => m.ID == ID).FirstOrDefault<UserHongBao>();
                model.UHB_IsDelete = 1;
                work.UserHongBaoRepository.Update(model);
                //work.HongBaoRepository.Delete(model);
                work.Save();
                work.Dispose();
            }

            return RedirectToAction("UserHongBao");
        }

        #endregion
    }
}