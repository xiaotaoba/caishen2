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
    public class ArticleController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 文章

        //默认文章列表
        [CheckPermission]
        public ActionResult Index(string keyword = "", int ArticleTypeID = 0, int ArticleTypeIDSub = 0, int page = 1)
        {
            ViewBag.keyword = keyword;
            ViewBag.ArticleTypeID = ArticleTypeID;
            ViewBag.ArticleTypeIDSub = ArticleTypeIDSub;
            ViewBag.ArticleType = work.ArticleTypeRepository.Get(m => m.AT_ParentID == 0 && m.ID != 15 );
            if (ArticleTypeID != 0)
            {
                ViewBag.ArticleTypeSub = work.ArticleTypeRepository.Get(m => m.AT_ParentID == ArticleTypeID);
            }

            var rst = work.Context.Articles
                .Join(work.Context.ArticleTypes, art => art.ArticleTypeID, at => at.ID, (art, at) => new { art, at })
                .Where(m => m.at.ID != 15 && m.at.AT_ParentID != 15 );

            if (ArticleTypeIDSub != 0)
            {
                rst = rst.Where(m => m.art.ArticleTypeID == ArticleTypeIDSub);
            }
            else if (ArticleTypeID != 0)
            {
                rst = rst.Where(m => m.at.AT_ParentID == ArticleTypeID || m.at.ID == ArticleTypeID);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.art.Art_Title.Contains(keyword));
            }
            var list = rst.Select(m => new ArticleVModel
             {
                 Article = m.art,
                 ArticleType = m.at

             }).OrderByDescending(m => m.Article.ID);

            int pageSize = 20;
            int pageNumber = page;
            //return View(rst.ToPagedList(pageNumber, pageSize));
            return View(list.ToPagedList(pageNumber, pageSize));
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
            ViewBag.ID = ID;
            ViewBag.ArticleType = work.ArticleTypeRepository.Get(m => m.AT_ParentID == 0 && m.ID != 15 );
            ViewBag.ArticleTypeID = 0;
            ViewBag.ArticleTypeIDSub = 0;


            if (ID != 0)
            {
                var model = work.ArticleRepository.Get(m => m.ID == ID).FirstOrDefault<Article>();
                if (model != null)
                {
                    #region 文章所属类型

                    //当前所属类型
                    ArticleType categoryModel = work.ArticleTypeRepository.Get(m => m.ID == model.ArticleTypeID).FirstOrDefault<ArticleType>();
                    if (categoryModel.AT_ParentID == 0)//所属分类为一级
                    {
                        ViewBag.ArticleTypeSub = work.ArticleTypeRepository.Get(m => m.AT_ParentID == model.ArticleTypeID);
                        ViewBag.ArticleTypeID = model.ArticleTypeID;
                    }
                    else
                    {
                        ViewBag.ArticleTypeSub = work.ArticleTypeRepository.Get(m => m.AT_ParentID == categoryModel.AT_ParentID);
                        ViewBag.ArticleTypeID = categoryModel.AT_ParentID;
                        ViewBag.ArticleTypeIDSub = model.ArticleTypeID;
                    }
                    #endregion
                }
                return View(model);
            }
            else
            {
            }
            return View(new Article());
        }

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="userRoleModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Article newModel, int ID = 0, int ArticleTypeID = 0, int ArticleTypeIDSub = 0, string Keyword = "")
        {
            ViewBag.ID = ID;
            ViewBag.ArticleType = work.ArticleTypeRepository.Get(m => m.AT_ParentID == 0 && m.ID != 15 );
            ViewBag.ArticleTypeIDSub = ArticleTypeIDSub;

            if (ModelState.IsValid)
            {
                //选择二级类型时，保存二级类型
                if (ArticleTypeIDSub != 0)
                {
                    newModel.ArticleTypeID = ArticleTypeIDSub;
                }



                if (ID == 0)//新增
                {
                    work.ArticleRepository.Insert(newModel);
                    work.Save();

                    #region 处理完成后，继续选中已选值

                    //当前所属类型
                    ArticleType categoryModel = work.ArticleTypeRepository.Get(m => m.ID == newModel.ArticleTypeID).FirstOrDefault<ArticleType>();
                    if (categoryModel.AT_ParentID == 0)//所属分类为一级
                    {
                        ViewBag.ArticleTypeSub = work.ArticleTypeRepository.Get(m => m.AT_ParentID == newModel.ArticleTypeID);
                        ViewBag.ArticleTypeID = newModel.ArticleTypeID;
                    }
                    else
                    {
                        ViewBag.ArticleTypeSub = work.ArticleTypeRepository.Get(m => m.AT_ParentID == categoryModel.AT_ParentID);
                        ViewBag.ArticleTypeID = categoryModel.AT_ParentID;
                        ViewBag.ArticleTypeIDSub = newModel.ArticleTypeID;
                    }

                    #endregion

                    work.Dispose();


                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                }
                else
                {
                    work.ArticleRepository.Update(newModel);
                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    return RedirectToAction("Index", new { ArticleTypeID, ArticleTypeIDSub, Keyword });
                }
            }
            return View(newModel);
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Delete(int ID = 0, int ArticleTypeID = 0, int ArticleTypeIDSub = 0, string Keyword = "")
        {

            if (ID != 0)
            {
                if (ID == 1)
                {
                    return RedirectToAction("Index", new { ArticleTypeID, Keyword, ArticleTypeIDSub });
                }
                var role = work.ArticleRepository.Get(m => m.ID == ID).FirstOrDefault<Article>();
                work.ArticleRepository.Delete(role);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("Index", new { ArticleTypeID, Keyword, ArticleTypeIDSub });
        }

        #endregion

        #region 作业本

        //作业本
        [CheckPermission]
        public ActionResult Zuoyeben(string keyword = "", int ArticleTypeID = 12, int ArticleTypeIDSub = 0, int page = 1)
        {
            ViewBag.keyword = keyword;
            ViewBag.ArticleTypeID = ArticleTypeID;
            ViewBag.ArticleTypeIDSub = ArticleTypeIDSub;
            if (ArticleTypeID != 0)
            {
                ViewBag.ArticleTypeSub = work.ArticleTypeRepository.Get(m => m.AT_ParentID == ArticleTypeID);
            }

            var rst = work.Context.Articles
                .Join(work.Context.ArticleTypes, art => art.ArticleTypeID, at => at.ID, (art, at) => new { art, at })
                .Where(m => m.at.ID == 12 || m.at.AT_ParentID == 12);

            if (ArticleTypeIDSub != 0)
            {
                rst = rst.Where(m => m.art.ArticleTypeID == ArticleTypeIDSub);
            }
            else if (ArticleTypeID != 0)
            {
                rst = rst.Where(m => m.at.AT_ParentID == ArticleTypeID || m.at.ID == ArticleTypeID);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.art.Art_Title.Contains(keyword));
            }
            var list = rst.Select(m => new ArticleVModel
            {
                Article = m.art,
                ArticleType = m.at

            }).OrderByDescending(m => m.Article.ID);

            int pageSize = 20;
            int pageNumber = page;
            //return View(rst.ToPagedList(pageNumber, pageSize));
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 添加/编辑页面
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [ValidateInput(false)]
        public ActionResult AddZuoyeben(int ID = 0)
        {
            ViewBag.ID = ID;
            ViewBag.ArticleType = work.ArticleTypeRepository.Get(m => m.AT_ParentID == 0 && m.ID == 12);
            ViewBag.ArticleTypeID = 0;
            ViewBag.ArticleTypeIDSub = 0;


            if (ID != 0)
            {
                var model = work.ArticleRepository.Get(m => m.ID == ID).FirstOrDefault<Article>();
                if (model != null)
                {
                    #region 文章所属类型

                    //当前所属类型
                    ArticleType categoryModel = work.ArticleTypeRepository.Get(m => m.ID == model.ArticleTypeID).FirstOrDefault<ArticleType>();
                    if (categoryModel.AT_ParentID == 0)//所属分类为一级
                    {
                        ViewBag.ArticleTypeSub = work.ArticleTypeRepository.Get(m => m.AT_ParentID == model.ArticleTypeID);
                        ViewBag.ArticleTypeID = model.ArticleTypeID;
                    }
                    else
                    {
                        ViewBag.ArticleTypeSub = work.ArticleTypeRepository.Get(m => m.AT_ParentID == categoryModel.AT_ParentID);
                        ViewBag.ArticleTypeID = categoryModel.AT_ParentID;
                        ViewBag.ArticleTypeIDSub = model.ArticleTypeID;
                    }
                    #endregion
                }
                return View(model);
            }
            else
            {
            }
            return View(new Article());
        }

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="userRoleModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddZuoyeben(Article newModel, int ID = 0, int ArticleTypeID = 0, int ArticleTypeIDSub = 0, string Keyword = "")
        {
            ViewBag.ID = ID;
            ViewBag.ArticleType = work.ArticleTypeRepository.Get(m => m.AT_ParentID == 0 && m.ID == 12);
            ViewBag.ArticleTypeIDSub = ArticleTypeIDSub;

            if (ModelState.IsValid)
            {
                //选择二级类型时，保存二级类型
                if (ArticleTypeIDSub != 0)
                {
                    newModel.ArticleTypeID = ArticleTypeIDSub;
                }



                if (ID == 0)//新增
                {
                    work.ArticleRepository.Insert(newModel);
                    work.Save();

                    #region 处理完成后，继续选中已选值

                    //当前所属类型
                    ArticleType categoryModel = work.ArticleTypeRepository.Get(m => m.ID == newModel.ArticleTypeID).FirstOrDefault<ArticleType>();
                    if (categoryModel.AT_ParentID == 0)//所属分类为一级
                    {
                        ViewBag.ArticleTypeSub = work.ArticleTypeRepository.Get(m => m.AT_ParentID == newModel.ArticleTypeID);
                        ViewBag.ArticleTypeID = newModel.ArticleTypeID;
                    }
                    else
                    {
                        ViewBag.ArticleTypeSub = work.ArticleTypeRepository.Get(m => m.AT_ParentID == categoryModel.AT_ParentID);
                        ViewBag.ArticleTypeID = categoryModel.AT_ParentID;
                        ViewBag.ArticleTypeIDSub = newModel.ArticleTypeID;
                    }

                    #endregion

                    work.Dispose();


                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                }
                else
                {
                    work.ArticleRepository.Update(newModel);
                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    return RedirectToAction("Zuoyeben", new { ArticleTypeID, ArticleTypeIDSub, Keyword });
                }
            }
            return View(newModel);
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult DeleteZuoyeben(int ID = 0, int ArticleTypeID = 0, int ArticleTypeIDSub = 0, string Keyword = "")
        {

            if (ID != 0)
            {
                if (ID == 1)
                {
                    return RedirectToAction("Zuoyeben", new { ArticleTypeID, Keyword, ArticleTypeIDSub });
                }
                var role = work.ArticleRepository.Get(m => m.ID == ID).FirstOrDefault<Article>();
                work.ArticleRepository.Delete(role);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("Zuoyeben", new { ArticleTypeID, Keyword, ArticleTypeIDSub });
        }

        #endregion

        #region 微信模板消息

        //微信模板消息
        [CheckPermission]
        public ActionResult WXNotice(string keyword = "", int ArticleTypeID = 15, int ArticleTypeIDSub = 0, int page = 1)
        {
            ViewBag.keyword = keyword;
            ViewBag.ArticleTypeID = ArticleTypeID;
            ViewBag.ArticleTypeIDSub = ArticleTypeIDSub;
            ViewBag.ArticleType = work.ArticleTypeRepository.Get(m => m.ID == 15);
            if (ArticleTypeID != 0)
            {
                ViewBag.ArticleTypeSub = work.ArticleTypeRepository.Get(m => m.AT_ParentID == ArticleTypeID);
            }

            var rst = work.Context.Articles.Join(work.Context.ArticleTypes, art => art.ArticleTypeID, at => at.ID, (art, at) => new { art, at })
                .Where(m => m.at.ID == 15 || m.at.AT_ParentID == 15);

            if (ArticleTypeIDSub != 0)
            {
                rst = rst.Where(m => m.art.ArticleTypeID == ArticleTypeIDSub);
            }
            else if (ArticleTypeID != 0)
            {
                rst = rst.Where(m => m.at.AT_ParentID == ArticleTypeID || m.at.ID == ArticleTypeID);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.art.Art_Title.Contains(keyword));
            }
            var list = rst.Select(m => new ArticleVModel
            {
                Article = m.art,
                ArticleType = m.at

            }).OrderByDescending(m => m.Article.ID);

            int pageSize = 20;
            int pageNumber = page;
            //return View(rst.ToPagedList(pageNumber, pageSize));
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 添加/编辑页面
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [ValidateInput(false)]
        public ActionResult AddWXNotice(int ID = 0)
        {
            ViewBag.ID = ID;
            ViewBag.ArticleType = work.ArticleTypeRepository.Get(m => m.ID == 15);
            ViewBag.ArticleTypeID = 0;
            ViewBag.ArticleTypeIDSub = 0;
            ViewBag.Departments = work.DepartmentRepository.Get();

            if (ID != 0)
            {
                var model = work.ArticleRepository.Get(m => m.ID == ID).FirstOrDefault<Article>();
                if (model != null)
                {
                    #region 文章所属类型

                    //当前所属类型
                    ArticleType categoryModel = work.ArticleTypeRepository.Get(m => m.ID == model.ArticleTypeID).FirstOrDefault<ArticleType>();
                    if (categoryModel.AT_ParentID == 0)//所属分类为一级
                    {
                        ViewBag.ArticleTypeSub = work.ArticleTypeRepository.Get(m => m.AT_ParentID == model.ArticleTypeID);
                        ViewBag.ArticleTypeID = model.ArticleTypeID;
                    }
                    else
                    {
                        ViewBag.ArticleTypeSub = work.ArticleTypeRepository.Get(m => m.AT_ParentID == categoryModel.AT_ParentID);
                        ViewBag.ArticleTypeID = categoryModel.AT_ParentID;
                        ViewBag.ArticleTypeIDSub = model.ArticleTypeID;
                    }
                    #endregion
                }
                return View(model);
            }
            else
            {
                ViewBag.ArticleTypeSub = work.ArticleTypeRepository.Get(m => m.AT_ParentID == 15);
            }
            return View(new Article());
        }

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <param name="ArticleTypeID"></param>
        /// <param name="ArticleTypeIDSub"></param>
        /// <param name="type">1:按部门，2按openid</param>
        /// <param name="DepartmentID"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddWXNotice(Article newModel, int ID = 0, int ArticleTypeID = 0, int ArticleTypeIDSub = 0, int type = 1, int DepartmentID = 0, string openid = "")
        {
            ViewBag.ID = ID;
            ViewBag.ArticleType = work.ArticleTypeRepository.Get(m => m.ID == 15);
            ViewBag.ArticleTypeIDSub = ArticleTypeIDSub;
            ViewBag.Departments = work.DepartmentRepository.Get();
            string url = "";
            //if (ModelState.IsValid)
            //{
            //选择二级类型时，保存二级类型
            if (ArticleTypeIDSub != 0)
            {
                newModel.ArticleTypeID = ArticleTypeIDSub;
            }

            if (ID == 0)//新增
            {
                newModel.Art_WX_Type = type;
                newModel.Art_WX_DepartmentID = DepartmentID;
                newModel.Art_WX_Openids = openid;

                work.ArticleRepository.Insert(newModel);
                work.Save();

                #region 处理完成后，继续选中已选值

                //当前所属类型
                ArticleType categoryModel = work.ArticleTypeRepository.Get(m => m.ID == newModel.ArticleTypeID).FirstOrDefault<ArticleType>();
                if (categoryModel.AT_ParentID == 0)//所属分类为一级
                {
                    ViewBag.ArticleTypeSub = work.ArticleTypeRepository.Get(m => m.AT_ParentID == newModel.ArticleTypeID);
                    ViewBag.ArticleTypeID = newModel.ArticleTypeID;
                }
                else
                {
                    ViewBag.ArticleTypeSub = work.ArticleTypeRepository.Get(m => m.AT_ParentID == categoryModel.AT_ParentID);
                    ViewBag.ArticleTypeID = categoryModel.AT_ParentID;
                    ViewBag.ArticleTypeIDSub = newModel.ArticleTypeID;
                }

                #endregion


                #region 发送微信模板处理

                if (type == 1)
                {
                    if (DepartmentID == 0)
                    {
                        var openidList = work.Context.Users
                            .Where(m => m.U_IsDelete == 0 && m.U_Is_Enable == 1 && m.U_OpenId != "")
                            .Select(m => m.U_OpenId).Distinct().ToList();
                        if (openidList != null && openidList.Count > 0)
                        {
                            openid = string.Join(",", openidList);
                        }
                    }
                    else
                    {
                        var openidList = work.Context.Users
                            .Join(work.Context.Departments, u => u.U_DepartmentID, d => d.ID, (u, d) => new { u, d })
                            .Where(m => m.u.U_IsDelete == 0 && m.u.U_Is_Enable == 1 && m.u.U_OpenId != "")
                            .Where(m => m.d.ID == DepartmentID || m.d.Dep_FollowID == DepartmentID)
                            .Select(m => m.u.U_OpenId).Distinct().ToList();
                        if (openidList != null && openidList.Count > 0)
                        {
                            openid = string.Join(",", openidList);
                        }
                    }
                }
                else
                {

                }

                if (newModel.Art_IsUrl == 1)
                {
                    url = newModel.Art_Url;
                }
                else
                {
                    url = string.Format("{0}/Mobile/article/Detail/{1}", ConfigHelper.GetConfigString("WebSiteDomain"), newModel.ID);
                }

                List<string> openids = openid.Split(',').ToList();
                int index = 0;
                foreach (var item in openids)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        WxTemplateMessage.SendNewCourseMessage(item, newModel.Art_ShortTitle, newModel.Art_Title, newModel.Art_Description, newModel.Art_Author, newModel.Art_From, newModel.Art_Keywords, url);
                        index++;
                    }
                }

                #endregion

                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", string.Format("保存成功，共发送{0}条模板消息！", index));

                work.Dispose();
            }
            //else
            //{
            //    work.ArticleRepository.Update(newModel);
            //    work.Save();
            //    work.Dispose();

            //    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            //    return RedirectToAction("WXNotice", new { ArticleTypeID, ArticleTypeIDSub });
            //}
            //}
            return View(newModel);
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult DeleteWXNotice(int ID = 0, int ArticleTypeID = 0, int ArticleTypeIDSub = 0, string Keyword = "")
        {

            if (ID != 0)
            {
                if (ID == 1)
                {
                    return RedirectToAction("WXNotice", new { ArticleTypeID, Keyword, ArticleTypeIDSub });
                }
                var role = work.ArticleRepository.Get(m => m.ID == ID).FirstOrDefault<Article>();
                work.ArticleRepository.Delete(role);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("WXNotice", new { ArticleTypeID, Keyword, ArticleTypeIDSub });
        }

        #endregion
    }
}