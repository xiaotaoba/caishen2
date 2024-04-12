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
    public class GoodsTypeController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 产品类型

        //默认产品类型列表
        [CheckPermission]
        public ActionResult Index(string keyword = "", int page = 1)
        {
            ViewBag.Keyword = keyword;

            var rst = work.Context.GoodsTypes.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.GT_Name.Contains(keyword));
            }
            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 添加/编辑页面
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult GoodsTypeAdd(int ID = 0)
        {
            if (ID != 0)
            {
                ViewBag.GoodsTypeParents = work.GoodsTypeRepository.Get(m => m.ID != ID & m.GT_ParentID == 0).OrderByDescending(m => m.GT_Sort).ToList();
                var model = work.GoodsTypeRepository.Get(m => m.ID == ID).FirstOrDefault<GoodsType>();
                return View(model);
            }
            else
            {
                ViewBag.GoodsTypeParents = work.GoodsTypeRepository.Get(m => m.GT_ParentID == 0).OrderByDescending(m => m.GT_Sort).ToList();
            }
            return View();
        }

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="userRoleModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult GoodsTypeAdd(GoodsType newModel, int ID = 0)
        {
            if (ModelState.IsValid)
            {
                if (ID == 0)//新增
                {
                    ViewBag.GoodsTypeParents = work.GoodsTypeRepository.Get(m => m.GT_ParentID == 0).OrderByDescending(m => m.GT_Sort).ToList();
                    var role = work.GoodsTypeRepository.Get(m => m.GT_Name == newModel.GT_Name);
                    if (role.Count() > 0)
                    {
                        ModelState.AddModelError("GT_Name", "类型名称已存在");
                    }
                    else
                    {
                        work.GoodsTypeRepository.Insert(newModel);
                        work.Save();
                        work.Dispose();

                        LogService.Add(ManagerService.GetLoginModel(), "新增产品类型:" + newModel.GT_Name, newModel.ID.ToString());


                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    }
                }
                else
                {
                    ViewBag.GoodsTypeParents = work.GoodsTypeRepository.Get(m => m.ID != ID & m.GT_ParentID == 0).OrderByDescending(m => m.GT_Sort).ToList();
                    var oldModel = work.GoodsTypeRepository.Get(m => m.ID == ID).FirstOrDefault<GoodsType>();
                    var existModel = work.GoodsTypeRepository.Get(m => m.GT_Name == newModel.GT_Name & m.ID != ID);
                    if (existModel.Count() > 0)
                    {
                        //return json("角色名称已存在", jsonrequestbehavior.allowget);
                        ModelState.AddModelError("GT_Name", "类型名称已存在");
                    }
                    else
                    {
                        oldModel.GT_IsEnable = newModel.GT_IsEnable;
                        oldModel.GT_IsRecommend = newModel.GT_IsRecommend;
                        oldModel.GT_Name = newModel.GT_Name;
                        oldModel.GT_ParentID = newModel.GT_ParentID;
                        oldModel.GT_Sort = newModel.GT_Sort;

                        work.GoodsTypeRepository.Update(oldModel);
                        work.Save();
                        work.Dispose();

                        LogService.Add(ManagerService.GetLoginModel(), "编辑产品类型:" + newModel.GT_Name, newModel.ID.ToString());


                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(newModel);
        }

        /// <summary>
        /// 删除产品类型
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult GoodsTypeDelete(int ID = 0)
        {
            if (ID != 0)
            {
                if (ID == 1 || ID == 2 || ID == 3)
                {
                    return RedirectToAction("Index");
                }
                var role = work.GoodsTypeRepository.Get(m => m.ID == ID).FirstOrDefault<GoodsType>();
                work.GoodsTypeRepository.Delete(role);
                work.Save();
                work.Dispose();

                LogService.Add(ManagerService.GetLoginModel(), "删除产品类型", ID.ToString());

            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}