using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Web.Attribute;
using Pannet.Models;
using Pannet.DAL;using Pannet.Utility;

namespace Pannet.Web.Controllers
{
    public class BrandController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 品牌

        //默认品牌列表
        [CheckPermission]
        public ActionResult Index()
        {
            return View(work.BrandRepository.Get());
        }

        /// <summary>
        /// 添加/编辑页面
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Add(int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.BrandRepository.Get(m => m.ID == ID).FirstOrDefault<Brand>();
                return View(model);
            }
            return View(new Brand());
        }

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult Add(Brand newModel, int ID = 0)
        {
            if (ModelState.IsValid)
            {
                if (ID == 0)//新增
                {
                    var role = work.BrandRepository.Get(m => m.B_Name == newModel.B_Name);
                    if (role.Count() > 0)
                    {
                        ModelState.AddModelError("GC_Name", "品牌名称已存在");
                    }
                    else
                    {
                        work.BrandRepository.Insert(newModel);
                        work.Save();
                        work.Dispose();

                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    }
                }
                else
                {
                    var oldModel = work.BrandRepository.Get(m => m.ID == ID).FirstOrDefault<Brand>();
                    var existModel = work.BrandRepository.Get(m => m.B_Name == newModel.B_Name & m.ID != ID);
                    if (existModel.Count() > 0)
                    {
                        ModelState.AddModelError("GC_Name", "品牌名称已存在");
                    }
                    else
                    {
                        oldModel.B_Desc = newModel.B_Desc;
                        oldModel.B_Image = newModel.B_Image;
                        oldModel.B_IsEnable = newModel.B_IsEnable;
                        oldModel.B_IsRecommend = newModel.B_IsRecommend;
                        oldModel.B_Name = newModel.B_Name;
                        oldModel.B_NameEn = newModel.B_NameEn;
                        oldModel.B_Sort = newModel.B_Sort;
                        oldModel.B_Url = newModel.B_Url;

                        work.BrandRepository.Update(oldModel);
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
        /// 删除品牌
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Delete(int ID = 0)
        {
            if (ID != 0)
            {
                //if (ID == 1 || ID == 2 || ID == 3)
                //{
                //    return RedirectToAction("Index");
                //}
                var role = work.BrandRepository.Get(m => m.ID == ID).FirstOrDefault<Brand>();
                work.BrandRepository.Delete(role);
                work.Save();
                work.Dispose();
            }

            return RedirectToAction("Index");
        }

        #endregion
    }
}