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
    public class NavigationController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 导航链接

        //默认导航链接列表
        [CheckPermission]
        public ActionResult Index(string keyword = "", int page = 1, int followID = 0, string action = "", int recommend = -1, int enable = -1)
        {
            ViewBag.keyword = keyword;
            ViewBag.recommend = recommend;
            ViewBag.enable = enable;
            ViewBag.followID = followID;
            ViewBag.action = action;

            #region 批量操作

            if (!string.IsNullOrEmpty(action))
            {
                string ids = Request.Form["ids"];
                if (!string.IsNullOrEmpty(ids))
                {
                    string[] arrIds = ids.Trim(',').Split(',');
                    if (action == "delete")//批量删除
                    {

                    }
                    else if (action == "update")//批量更新
                    {
                        Navigation model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.NavigationRepository.GetByID(Convert.ToInt32(a_id));

                                model.Nav_Order = Convert.ToInt32(Request.Form["order_" + a_id]);
                                model.Nav_Name = Request.Form["name_" + a_id];
                                model.Nav_ShortName = Request.Form["shortname_" + a_id];
                                model.Nav_Url = Request.Form["url_" + a_id];

                                work.NavigationRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                    else if (action == "tj")//批量推荐
                    {
                        Navigation model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.NavigationRepository.GetByID(Convert.ToInt32(a_id));

                                model.Nav_IsRecommend = 1;

                                work.NavigationRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                    else if (action == "tj_cancel")//批量取消推荐
                    {
                        Navigation model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.NavigationRepository.GetByID(Convert.ToInt32(a_id));

                                model.Nav_IsRecommend = 0;

                                work.NavigationRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                    else if (action == "enable")//上架
                    {
                        Navigation model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.NavigationRepository.GetByID(Convert.ToInt32(a_id));

                                model.Nav_IsEnable = 1;

                                work.NavigationRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                    else if (action == "enable_cancel")//不启动
                    {
                        Navigation model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.NavigationRepository.GetByID(Convert.ToInt32(a_id));

                                model.Nav_IsEnable = 0;

                                work.NavigationRepository.Update(model);
                                work.Save();
                            }
                        }
                    }

                }

            }


            #endregion

            ViewBag.Parents = work.NavigationRepository.Get(m => m.Nav_FollowID == 0).OrderByDescending(m => m.Nav_Order).ToList();

            var rst = work.Context.Navigations.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.Nav_Name.Contains(keyword));
            }
            if (followID != 0)
            {
                List<int> childIds = rst.Where(m => m.Nav_FollowID == followID).Select(m=>m.ID).ToList();//二级ID
                rst = rst.Where(m => m.ID == followID || m.Nav_FollowID == followID || childIds.Contains(m.Nav_FollowID));
            }
            rst = rst.OrderByDescending(m => m.Nav_FollowID).ThenByDescending(m => m.Nav_Order);

            int pageSize = 100;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 添加/编辑页面
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Add(int ID = 0)
        {
            ViewBag.ID = ID;
            var rst = work.Context.Navigations.AsQueryable();
            List<int> firstIds = rst.Where(m => m.Nav_FollowID == 0).Select(m => m.ID).ToList();
            ViewBag.Parents = rst.Where(m => m.ID != ID).Where(m => m.Nav_FollowID == 0 || firstIds.Contains(m.Nav_FollowID)).ToList();

            if (ID != 0)
            {
                //ViewBag.Parents = work.NavigationRepository.Get(m => m.ID != ID & m.Nav_FollowID == 0).OrderByDescending(m => m.Nav_Order).ToList();
                var model = work.NavigationRepository.Get(m => m.ID == ID).FirstOrDefault<Navigation>();
                return View(model);
            }
            else
            {
                //ViewBag.Parents = work.NavigationRepository.Get(m => m.Nav_FollowID == 0).OrderByDescending(m => m.Nav_Order).ToList();
            }
            return View(new Navigation());
        }

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="userRoleModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult Add(Navigation newModel, int ID = 0)
        {
            ViewBag.ID = ID;
            if (ModelState.IsValid)
            {

                var rst = work.Context.Navigations.AsQueryable();
                List<int> firstIds = rst.Where(m => m.Nav_FollowID == 0).Select(m => m.ID).ToList();
                ViewBag.Parents = rst.Where(m => m.ID != ID).Where(m => m.Nav_FollowID == 0 || firstIds.Contains(m.Nav_FollowID)).ToList();

                if (ID == 0)//新增
                {
                    //ViewBag.Parents = work.NavigationRepository.Get(m => m.Nav_FollowID == 0).OrderByDescending(m => m.Nav_Order).ToList();
                    //var existModel = work.NavigationRepository.Get(m => m.Nav_Name == newModel.Nav_Name);
                    //if (existModel.Count() > 0)
                    //{
                    //    ModelState.AddModelError("Nav_Name", "导航链接名称已存在");
                    //}
                    //else
                    //{
                    work.NavigationRepository.Insert(newModel);
                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    //}
                }
                else
                {
                    //ViewBag.Parents = work.NavigationRepository.Get(m => m.ID != ID & m.Nav_FollowID == 0).OrderByDescending(m => m.Nav_Order).ToList();
                    //var existModel = work.NavigationRepository.Get(m => m.Nav_Name == newModel.Nav_Name & m.ID != ID);
                    //if (existModel.Count() > 0)
                    //{
                    //    ModelState.AddModelError("Nav_Name", "导航链接名称已存在");
                    //}
                    //else
                    //{
                    work.NavigationRepository.Update(newModel);
                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    return RedirectToAction("Index");
                    //}
                }
            }
            return View(newModel);
        }

        /// <summary>
        /// 删除导航链接
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
                var role = work.NavigationRepository.Get(m => m.ID == ID).FirstOrDefault<Navigation>();
                work.NavigationRepository.Delete(role);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("Index");
        }

        #endregion

        #region 获取导航JSON

        /// <summary>
        /// 获取导航 Json
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult GetJson(int parentid)
        {
            List<Navigation> listArea = work.NavigationRepository.Get(m => m.Nav_FollowID == parentid).ToList();

            return Json(listArea, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}