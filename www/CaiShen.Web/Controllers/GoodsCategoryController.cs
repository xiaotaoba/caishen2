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
    public class GoodsCategoryController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 商品分类

        //默认商品分类列表
        [CheckPermission]
        public ActionResult Index(int page = 1)
        {
            var rst = work.Context.GoodsCategorys.AsQueryable();

            rst = rst.OrderBy(m => m.GC_Type).ThenByDescending(m => m.GC_ParentID).ThenByDescending(m => m.GC_Sort).ThenByDescending(m => m.ID);

            int pageSize = 15;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 添加/编辑页面
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult GoodsCategoryAdd(int ID = 0)
        {
            ViewBag.GoodsTypes = work.GoodsTypeRepository.Get().OrderBy(m => m.GT_Name).ToList();
            ViewBag.Departments = work.DepartmentRepository.Get();

            if (ID != 0)
            {
                ViewBag.GoodsTypeParents = work.GoodsCategoryRepository.Get(m => m.ID != ID & m.GC_ParentID == 0).OrderByDescending(m => m.GC_Sort).ToList();
                var model = work.GoodsCategoryRepository.Get(m => m.ID == ID).FirstOrDefault<GoodsCategory>();
                return View(model);
            }
            else
            {
                ViewBag.GoodsTypeParents = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == 0).OrderByDescending(m => m.GC_Sort).ToList();
            }
            return View(new GoodsCategory());
        }

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="userRoleModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        [HttpPost]
        public ActionResult GoodsCategoryAdd(GoodsCategory newModel, int ID = 0)
        {
            ViewBag.GoodsTypes = work.GoodsTypeRepository.Get().OrderBy(m => m.GT_Name).ToList();
            ViewBag.Departments = work.DepartmentRepository.Get();

            if (ModelState.IsValid)
            {
                if (ID == 0)//新增
                {
                    ViewBag.GoodsTypeParents = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == 0).OrderByDescending(m => m.GC_Sort).ToList();
                    var role = work.GoodsCategoryRepository.Get(m => m.GC_Name == newModel.GC_Name);
                    if (role.Count() > 0)
                    {
                        ModelState.AddModelError("GC_Name", "商品分类名称已存在");
                    }
                    else
                    {
                        work.GoodsCategoryRepository.Insert(newModel);
                        work.Save();
                        work.Dispose();

                        LogService.Add(ManagerService.GetLoginModel(), "新增商品分类:" + newModel.GC_Name, newModel.ID.ToString());
                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    }
                }
                else
                {
                    ViewBag.GoodsTypeParents = work.GoodsCategoryRepository.Get(m => m.ID != ID & m.GC_ParentID == 0).OrderByDescending(m => m.GC_Sort).ToList();
                    var oldModel = work.GoodsCategoryRepository.Get(m => m.ID == ID).FirstOrDefault<GoodsCategory>();
                    var existModel = work.GoodsCategoryRepository.Get(m => m.GC_Name == newModel.GC_Name & m.ID != ID);
                    if (existModel.Count() > 0)
                    {
                        ModelState.AddModelError("GC_Name", "商品分类名称已存在");
                    }
                    else
                    {
                        oldModel.GC_IsEnable = newModel.GC_IsEnable;
                        oldModel.GC_IsRecommend = newModel.GC_IsRecommend;
                        oldModel.GC_Name = newModel.GC_Name;
                        oldModel.GC_ParentID = newModel.GC_ParentID;
                        oldModel.GC_Sort = newModel.GC_Sort;
                        oldModel.GC_Type = newModel.GC_Type;
                        oldModel.GC_Department = newModel.GC_Department;
                        oldModel.GC_Image = newModel.GC_Image;

                        work.GoodsCategoryRepository.Update(oldModel);
                        work.Save();
                        work.Dispose();

                        LogService.Add(ManagerService.GetLoginModel(), "编辑商品分类:" + newModel.GC_Name, newModel.ID.ToString());


                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(newModel);
        }

        /// <summary>
        /// 删除商品分类
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult GoodsCategoryDelete(int ID = 0)
        {
            if (ID != 0)
            {
                //if (ID == 1 || ID == 2 || ID == 3)
                //{
                //    return RedirectToAction("Index");
                //}
                var role = work.GoodsCategoryRepository.Get(m => m.ID == ID).FirstOrDefault<GoodsCategory>();
                work.GoodsCategoryRepository.Delete(role);
                work.Save();
                work.Dispose();

                LogService.Add(ManagerService.GetLoginModel(), "删除商品分类", ID.ToString());

            }
            return RedirectToAction("Index");
        }

        #endregion

        #region 获取分类JSON

        /// <summary>
        /// 获取分类 Json
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult GetCategoryJson(int parentid)
        {
            List<GoodsCategory> listArea = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == parentid).ToList();

            return Json(listArea, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取分类 Json
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult GetCategoryJsonByType(int typeid,int parentid)
        {
            List<GoodsCategory> listArea = work.GoodsCategoryRepository.Get(m => m.GC_Type == typeid && m.GC_ParentID == parentid).ToList();

            return Json(listArea, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}