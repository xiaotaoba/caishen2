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
    public class DesignWorkController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();
        private int tag_type = Convert.ToInt16(DataConfig.TagTypeEnum.团队成员所属分类);
        private int tag_style = Convert.ToInt16(DataConfig.TagTypeEnum.团队成员临床经验);
        private int tag_tag = Convert.ToInt16(DataConfig.TagTypeEnum.团队成员专长标签);

        #region 团队信息列表

        [CheckPermission]
        public ActionResult Index(string field = "name", string keyword = "", int type = 0, int style = 0, int istuijian = -1, int page = 1)
        {
            ViewBag.type = type;
            ViewBag.style = style;
            ViewBag.Field = field;
            ViewBag.Keyword = keyword;
            ViewBag.istuijian = istuijian;
            ViewBag.DesignTypes = work.TagRepository.Get(m => m.T_Type == tag_type);
            ViewBag.DesignStyles = work.TagRepository.Get(m => m.T_Type == tag_style);

            var rst = work.Context.DesignWorks.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                switch (field)
                {
                    case "name": rst = rst.Where(m => m.DW_Name.Contains(keyword)); break;
                    case "number": rst = rst.Where(m => m.DW_Number.Contains(keyword)); break;
                    case "author": rst = rst.Where(m => m.DW_Author.Contains(keyword)); break;
                    default: break;
                };
            }
            if (style != 0)
            {
                rst = rst.Where(m => m.DW_Style == style);
            }
            if (type != 0)
            {
                rst = rst.Where(m => m.DW_Type == type);
            }
            if (istuijian != -1)
            {
                rst = rst.Where(m => m.DW_IsRecommend == istuijian);
            }
            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        #endregion

        #region 新增团队信息

        /// <summary>
        /// 新增团队信息
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        [ValidateInput(false)]
        public ActionResult Add()
        {
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.DesignTypes = work.TagRepository.Get(m => m.T_Type == tag_type);
            ViewBag.DesignStyles = work.TagRepository.Get(m => m.T_Type == tag_style);
            ViewBag.DesignTags = work.TagRepository.Get(m => m.T_Type == tag_tag);

            return View(new DesignWork());
        }

        /// <summary>
        ///  新增团队信息-post
        /// </summary>
        /// <param name="newModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [CheckPermission]
        public ActionResult Add(DesignWork newModel)
        {
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.DesignTypes = work.TagRepository.Get(m => m.T_Type == tag_type);
            ViewBag.DesignStyles = work.TagRepository.Get(m => m.T_Type == tag_style);
            ViewBag.DesignTags = work.TagRepository.Get(m => m.T_Type == tag_tag);

            if (ModelState.IsValid)
            {
                var existModel = work.DesignWorkRepository.Get(m => m.DW_Number == newModel.DW_Number);
                if (existModel.Count() > 0)
                {
                    ModelState.AddModelError("DW_Number", string.Format("团队编号{0}已存在！", newModel.DW_Number));
                }
                else
                {
                    newModel.DW_TypeTags = Request["DW_TypeTags"];
                    work.DesignWorkRepository.Insert(newModel);

                    work.Save();
                    work.Dispose();
                    LogService.Add(ManagerService.GetLoginModel(), "新增团队：" + newModel.DW_Name, newModel.ID.ToString());
                }
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            }

            return View(newModel);
        }

        #endregion

        #region 编辑团队信息

        /// <summary>
        /// 资料编辑
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        [ValidateInput(false)]
        public ActionResult Edit(int ID = 0)
        {
            ViewBag.ID = ID;
            ViewBag.DesignTypes = work.TagRepository.Get(m => m.T_Type == tag_type);
            ViewBag.DesignStyles = work.TagRepository.Get(m => m.T_Type == tag_style);
            ViewBag.DesignTags = work.TagRepository.Get(m => m.T_Type == tag_tag);

            if (ID != 0)
            {
                ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
                DesignWork oldModel = work.DesignWorkRepository.Get(m => m.ID == ID).FirstOrDefault<DesignWork>();
                if (oldModel != null)
                {
                    ViewBag.Citys = work.AreaRepository.Get(m => m.Area_ParentID == oldModel.DW_Province);
                    ViewBag.Regions = work.AreaRepository.Get(m => m.Area_ParentID == oldModel.DW_City);
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
        [ValidateInput(false)]
        public ActionResult Edit(DesignWork newModel)
        {
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.Citys = work.AreaRepository.Get(m => m.Area_ParentID == newModel.DW_Province);
            ViewBag.Regions = work.AreaRepository.Get(m => m.Area_ParentID == newModel.DW_City);
            ViewBag.DesignTypes = work.TagRepository.Get(m => m.T_Type == tag_type);
            ViewBag.DesignStyles = work.TagRepository.Get(m => m.T_Type == tag_style);
            ViewBag.DesignTags = work.TagRepository.Get(m => m.T_Type == tag_tag);

            if (ModelState.IsValid)
            {
                DesignWork oldModel = work.Context.DesignWorks.AsNoTracking().Where(m => m.ID == newModel.ID).FirstOrDefault<DesignWork>();
                if (oldModel != null)
                {

                    if (oldModel.DW_Name != newModel.DW_Name)//修改团队名称
                    {
                        var existModel = work.DesignWorkRepository.Get(m => m.DW_Number == newModel.DW_Number);
                        if (existModel.Count() > 0)
                        {
                            ModelState.AddModelError("DW_Number", string.Format("团队编号{0}已存在！", newModel.DW_Number));
                            return View(newModel);
                        }
                        //else
                        //{
                        //    oldModel.DW_Name = newModel.DW_Name;
                        //}
                    }
                }

                newModel.DW_TypeTags = Request["DW_TypeTags"];

                work.DesignWorkRepository.Update(newModel);

                work.Save();
                work.Dispose();

                LogService.Add(ManagerService.GetLoginModel(), "编辑团队：" + newModel.DW_Name, newModel.ID.ToString());
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            }
            //return JavaScript("alert('修改成功!');");

            return View(newModel);
        }

        #endregion

        #region 删除
        /// <summary>
        /// 删除团队
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Delete(int ID = 0)
        {
            if (ID != 0)
            {
                //if (ID == 1)
                //{
                //    return RedirectToAction("Index");
                //}
                var model = work.DesignWorkRepository.Get(m => m.ID == ID).FirstOrDefault<DesignWork>();
                work.DesignWorkRepository.Delete(model);
                work.Save();
                work.Dispose();
                LogService.Add(ManagerService.GetLoginModel(), "删除团队", ID.ToString());

            }
            return RedirectToAction("Index");
        }

        #endregion

        #region 编辑团队相册

        /// <summary>
        /// 编辑团队相册
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Photos(int DesignWorkID = 0, int ID = 0, int page = 1)
        {
            int pageSize = 12;
            int pageNumber = page;
            var rst = work.DesignWorkPhotoRepository.Get(m => m.DesignWorkID == DesignWorkID);
            rst = rst.OrderByDescending(m => m.ID);

            ViewBag.DesignWorkID = DesignWorkID;
            ViewBag.DesignWorkPhoto = new DesignWorkPhoto();

            if (ID != 0)
            {
                ViewBag.DesignWorkPhoto = work.DesignWorkPhotoRepository.Get(m => m.ID == ID).FirstOrDefault();
            }

            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 上传新图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission]
        public ActionResult Photos(DesignWorkPhoto newModel, int ID = 0, int page = 1)
        {
            //展示调整记录
            int pageSize = 12;
            int pageNumber = page;


            ViewBag.DesignWorkID = newModel.DesignWorkID;
            ViewBag.DesignWorkPhoto = new DesignWorkPhoto();

            if (ModelState.IsValid)
            {
                if (ID == 0)
                {
                    if (newModel.DWP_IsFirst == 1)//主图,取消其他主图
                    {
                        DesignWorkPhotoService.SetFirst(newModel.DesignWorkID, 0);
                        //GoodsService.UpdateImage(newModel.DesignWorkID, newModel.DWP_Image);
                    }
                    //保存
                    work.DesignWorkPhotoRepository.Insert(newModel);

                    work.Save();
                }
                else
                {
                    ViewBag.DesignWorkPhoto = newModel;// work.DesignWorkPhotoRepository.Get(m => m.ID == ID).FirstOrDefault();

                    if (newModel.DWP_IsFirst == 1)//主图,取消其他主图
                    {
                        DesignWorkPhotoService.SetFirst(newModel.DesignWorkID, ID);
                        //GoodsService.UpdateImage(newModel.DesignWorkID, newModel.DWP_Image);
                    }

                    //编辑
                    //DesignWorkPhoto model = new DesignWorkPhoto();
                    //model.DesignWorkID = newModel.DesignWorkID;
                    //model.DWP_Image = newModel.DWP_Image;
                    //model.DWP_IsFirst = newModel.DWP_IsFirst;
                    //model.DWP_Sort = newModel.DWP_Sort;
                    work.DesignWorkPhotoRepository.Update(newModel);
                    work.Save();
                }

            }
            //var rst = work.DesignWorkPhotoRepository.Get(m => m.DesignWorkID == newModel.DesignWorkID);
            //rst = rst.OrderByDescending(m => m.ID);
            //return View(rst.ToPagedList(pageNumber, pageSize));
            return RedirectToAction("Photos", new { DesignWorkID = newModel.DesignWorkID });
        }

        //删除图片
        [CheckPermission]
        public ActionResult DeletePhoto(int ID = 0, int DesignWorkID = 0)
        {
            if (ID != 0)
            {
                var m = work.DesignWorkPhotoRepository.Get(g => g.ID == ID).FirstOrDefault<DesignWorkPhoto>();
                work.DesignWorkPhotoRepository.Delete(m);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("Photos", new { DesignWorkID = DesignWorkID });
        }


        #endregion

    }
}