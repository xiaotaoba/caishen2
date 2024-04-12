using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.DAL;
using Pannet.DAL.Repository;
using Pannet.Models;
using PagedList;
using Pannet.Utility;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class CourseController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();
        JsonResult json = new JsonResult
        {
            Data = new { }
        };

        #region 视频类

        public ActionResult Index(int cat_id = 0, int page = 1)
        {
            ViewBag.cat_id = cat_id;
            ViewBag.page = page;

            int goodsStatusOn = Convert.ToInt16(DataConfig.GoodsStatusEnum.上架);
            int g_type = Convert.ToInt16(DataConfig.GoodsTypeEnum.商品类);

            ViewBag.Catagorys = work.GoodsCategoryRepository.Get(m => m.GC_Type == g_type).ToList();

            var rst = work.Context.Goods.Where(m => m.G_Status == goodsStatusOn && m.GoodsTypeID == g_type);
            if (cat_id != 0)
            {
                rst = rst.Where(m => m.GoodsCategoryID == cat_id);
            }
            rst = rst.OrderByDescending(m => m.G_IsRecommend).ThenByDescending(m => m.G_Sort).ThenByDescending(m => m.ID);
            int pageSize = 40;
            return View(rst.ToPagedList(page, pageSize));
        }

        public ActionResult Detail(int ID = 0, int ga_id = 0)
        {
            if (LoginedUserModel == null)
            {
                //return RedirectToAction("Index", "Login",);
                Response.Redirect("/mobile/login?returnurl=" + Server.UrlEncode(Request.Url.ToString()));
                Response.End();
            }

            if (ID != 0)
            {
                CookieHelper.SetValue("recordBeginTime", DateTime.Now.ToString());
                var model = work.Context.Goods.Where(m => m.ID == ID).FirstOrDefault<Goods>();
                GoodsArticle prevArticleModel = new GoodsArticle();
                GoodsArticle articleModel = null;
                GoodsCategory GoodsCategoryModel = null;
                //当前用户测试记录
                ViewBag.UserTests = work.Context.Tests.Where(m => m.UserID == LoginedUserModel.ID && m.GoodsID == ID).ToList();

                bool prevIsDone = true;
                if (model != null)
                {
                    var articles = work.Context.GoodsArticles.AsNoTracking().Where(m => m.GoodsID == model.ID && m.GA_IsEnable == 1).OrderByDescending(m => m.GA_Sort).ThenBy(m => m.ID).ToList();
                    if (ga_id != 0)
                    {
                        articleModel = work.Context.GoodsArticles.AsNoTracking().Where(m=>m.ID==ga_id).FirstOrDefault();
                        ViewBag.ArticleModel = articleModel;

                        articleModel.GA_ShowTimes++;
                        work.GoodsArticleRepository.Update(articleModel);
                        work.Save();
                        //浏览记录
                        BrowseRecordService.Add(LoginedUserModel, CurrentShopModel, Request.Url.ToString(), articleModel.ID, Convert.ToInt16(DataConfig.BrowseRecordTypeEnum.课程文章));
                    }
                    else if (articles != null && articles.Count > 0)
                    {
                        articleModel = articles.First();
                        ViewBag.ArticleModel = articleModel;

                        articleModel.GA_ShowTimes++;
                        work.GoodsArticleRepository.Update(articleModel);
                        work.Save();
                        //浏览记录
                        BrowseRecordService.Add(LoginedUserModel, CurrentShopModel, Request.Url.ToString(), articleModel.ID, Convert.ToInt16(DataConfig.BrowseRecordTypeEnum.课程文章));
                    }

                    ViewBag.Articles = articles;
                    if (articles != null && articles.Count > 1)
                    {
                        //上一条视频是否测试合格
                        if (articleModel != null)
                        {
                            int index = articles.FindIndex(m => m.ID == articleModel.ID);
                            if (index > 0)
                            {
                                prevArticleModel = articles.GetRange(index - 1, 1).First();
                                if (prevArticleModel != null)
                                {
                                    int existTestCount = work.TestRepository.Get(m => m.T_State == 1 && prevArticleModel.ID == m.T_GoodsArticleID && m.UserID == LoginedUserModel.ID).Count();
                                    if (existTestCount < 1)
                                    {
                                        prevIsDone = false;
                                    }
                                }
                            }
                        }
                    }

                    ViewBag.prevIsDone = prevIsDone;

                    model.G_ShowTimes = model.G_ShowTimes + 1;
                    work.GoodsRepository.Update(model);
                    work.Save();

                    GoodsCategoryModel = work.Context.GoodsCategorys.AsNoTracking().Where(m=>m.ID==model.GoodsCategoryID).FirstOrDefault();
                    ViewBag.GoodsCategoryModel = GoodsCategoryModel;
                    //浏览记录
                    BrowseRecordService.Add(LoginedUserModel, CurrentShopModel, Request.Url.ToString(), model.ID, Convert.ToInt16(DataConfig.BrowseRecordTypeEnum.课程));
                    return View(model);
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Test(int goods_id = 0, int article_id = 0)
        {
            if (LoginedUserModel == null)
            {
                //return RedirectToAction("Index", "Login",);
                Response.Redirect("/mobile/login?returnurl=" + Server.UrlEncode(Request.Url.ToString()));
                Response.End();
            }
            if (goods_id == 0)
            {
                return RedirectToAction("Index");
            }

            Goods goodsModel = work.GoodsRepository.GetByID(goods_id);
            if (goodsModel == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Goods = goodsModel;
            ViewBag.article_id = article_id;
            if (article_id != 0)
            {
                GoodsArticle articleModel = work.GoodsArticleRepository.GetByID(article_id);
                ViewBag.ArticleModel = articleModel;
            }
            //是否测试,并且通过测试，已测试直接显示显示测试结果，不重复测试
            if (LoginedUserModel != null)
            {
                var existTest = work.Context.Tests.Where(m => m.UserID == LoginedUserModel.ID && m.GoodsID == goods_id && m.T_GoodsArticleID == article_id && m.T_State == 1).OrderByDescending(m => m.ID).ToList();
                if (existTest != null && existTest.Count() > 0)
                {
                    ViewBag.Test = existTest.First();
                    return View();
                }
            }
            int q_group = Convert.ToInt16(DataConfig.QuestionGroupEnum.课程测试);
            var rst = work.Context.Questions.Where(m => m.Q_GroupItemID == goods_id && m.Q_Group == q_group && m.Q_GroupItemSubID == article_id)
                .GroupJoin(work.Context.Answer, q => q.ID, a => a.QuestionID, (q, a) => new QuestionAnswerVModel
                {
                    Question = q,
                    AnswerList = a
                });

            rst = rst.OrderByDescending(m => m.Question.Q_Sort).ThenBy(m => m.Question.ID);
            int pageSize = 40;
            return View(rst.ToPagedList(1, pageSize));
        }
        /// <summary>
        /// 保存测试结果
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="goods_id"></param>
        /// <param name="article_id"></param>
        /// <returns></returns>
        public ActionResult TestSave(string jsonData, int goods_id = 0, int article_id = 0)
        {
            if (goods_id == 0)
            {
                return RedirectToAction("Index");
            }

            if (LoginedUserModel == null)
            {
                json.Data = new { status = "error", msg = "请先登录" };
                return Json(json.Data, JsonRequestBehavior.AllowGet);
            }

            //提交试题
            List<RequestTestQuestionVModel> requestList = JsonHelper.DeserializeJsonToList<RequestTestQuestionVModel>(jsonData);

            //所有试题
            int q_group = Convert.ToInt16(DataConfig.QuestionGroupEnum.课程测试);
            List<QuestionAnswerVModel> rst = work.Context.Questions.Where(m => m.Q_GroupItemID == goods_id && m.Q_Group == q_group && m.Q_GroupItemSubID == article_id)
                .GroupJoin(work.Context.Answer, q => q.ID, a => a.QuestionID, (q, a) => new QuestionAnswerVModel
                {
                    Question = q,
                    AnswerList = a
                }).ToList();

            //返回结果
            List<ResponseTestQuestionVModel> responseList = new List<ResponseTestQuestionVModel>();

            int rightCount = 0;
            int wrongCount = 0;

            if (rst != null && rst.Count > 0)
            {
                foreach (var item in rst)
                {
                    ResponseTestQuestionVModel responseModel = new ResponseTestQuestionVModel();
                    responseModel.ID = item.Question.ID;
                    if (item.Question.Q_Type == Convert.ToInt16(DataConfig.QuestionTypeEnum.判断题))
                    {
                        if (item.Question.Q_IsTrue.ToString() == requestList.Find(m => m.ID == item.Question.ID).Answer.Trim(','))
                        {
                            responseModel.IsTrue = 1;
                            rightCount++;
                        }
                        else
                        {
                            responseModel.IsTrue = 0;
                            wrongCount++;
                        }
                    }
                    else if (item.Question.Q_Type == Convert.ToInt16(DataConfig.QuestionTypeEnum.问答题))
                    {
                        if (item.Question.Q_Answer.ToString() == requestList.Find(m => m.ID == item.Question.ID).Answer.Trim(','))
                        {
                            responseModel.IsTrue = 1;
                            rightCount++;
                        }
                        else
                        {
                            responseModel.IsTrue = 0;
                            wrongCount++;
                        }
                    }
                    else if (item.Question.Q_Type == Convert.ToInt16(DataConfig.QuestionTypeEnum.单选题) || item.Question.Q_Type == Convert.ToInt16(DataConfig.QuestionTypeEnum.多选题))
                    {
                        RequestTestQuestionVModel requestOneModel = requestList.Find(m => m.ID == item.Question.ID);
                        List<string> requestAnswer = new List<string>();
                        if (requestOneModel != null)
                        {
                            requestAnswer = requestOneModel.Answer.Trim(',').Split(',').ToList();
                        }
                        List<ResponseAnswerVModel> responseAnswerList = new List<ResponseAnswerVModel>();
                        foreach (Answer answer in item.AnswerList)
                        {
                            ResponseAnswerVModel responseAnswerModel = new ResponseAnswerVModel();
                            responseAnswerModel.ID = answer.ID;
                            if (answer.A_IsTrue == 1 && requestAnswer.Contains(answer.ID.ToString()))
                            {
                                responseAnswerModel.IsTrue = 1;
                            }
                            else if (answer.A_IsTrue == 0 && !requestAnswer.Contains(answer.ID.ToString()))
                            {
                                responseAnswerModel.IsTrue = 1;
                            }
                            else
                            {
                                responseAnswerModel.IsTrue = 0;
                            }
                            if (requestAnswer.Contains(answer.ID.ToString()))
                            {
                                responseAnswerModel.IsSelect = 1;
                            }
                            responseAnswerList.Add(responseAnswerModel);
                            responseModel.AnswerRst = responseAnswerList;
                        }
                        if (responseAnswerList.Where(m => m.IsTrue == 0).Count() > 0)
                        {
                            responseModel.IsTrue = 0;
                            wrongCount++;
                        }
                        else
                        {
                            responseModel.IsTrue = 1;
                            rightCount++;
                        }
                    }
                    responseList.Add(responseModel);
                }
            }

            //测试记录
            Test testModel = new Models.Test();
            testModel.GoodsID = goods_id;
            testModel.UserID = LoginedUserModel.ID;
            testModel.T_RightCount = rightCount;
            testModel.T_WrongCount = wrongCount;
            testModel.T_Score = 1;
            testModel.T_GoodsArticleID = article_id;
            if (wrongCount == 0)
            {
                testModel.T_State = 1;
            }
            else
            {
                testModel.T_State = 0;
            }
            work.TestRepository.Insert(testModel);
            work.Save();

            //保存测试提交答案记录
            TestRecord recordAnswerModel = null;
            foreach (var item in requestList)
            {
                recordAnswerModel = new TestRecord();
                recordAnswerModel.GoodsID = goods_id;
                recordAnswerModel.UserID = LoginedUserModel.ID;
                recordAnswerModel.QuestionID = item.ID;
                recordAnswerModel.TestID = testModel.ID;
                recordAnswerModel.T_AnswerIds = item.Answer.Trim(',');
                recordAnswerModel.T_IsTrue = responseList.Find(m => m.ID == item.ID).IsTrue;

                work.TestRecordRepository.Insert(recordAnswerModel);
            }
            work.Save();

            //合格
            if (testModel.T_State == 1)
            {
                //测试送积分
                User user = work.UserRepository.GetByID(LoginedUserModel.ID);
                int score = Convert.ToInt16(ConfigHelper.GetConfigString("TestScore"));
                user.U_Score = user.U_Score + score;

                work.UserRepository.Update(user);
                work.Save();

                UserService.SetCacheUser(user.U_UserName, user);

                UserScoreHistoryService.Insert(user.ID, score, user.U_Score, 0, user.U_LockScore, 1, "收入", "测试送积分", testModel.ID, user.U_UserName);

                AddGoodsArticleRecord(testModel.GoodsID, testModel.T_GoodsArticleID, LoginedUserModel.ID);

            }

            json.Data = new { status = "success", msg = "", rightCount, wrongCount, json = responseList };
            return Json(json.Data, JsonRequestBehavior.AllowGet);

            //int q_group = Convert.ToInt16(DataConfig.QuestionGroupEnum.课程测试);
            //var rst = work.Context.Questions.Where(m => m.Q_GroupItemID == goods_id && m.Q_Group == q_group);
            //rst = rst.OrderByDescending(m => m.Q_Sort).ThenByDescending(m => m.ID);
            //int pageSize = 40;
            //return View(rst.ToPagedList(1, pageSize));
        }

        #endregion

        #region PPT类

        public ActionResult PPT(int cat_id = 0, int page = 1)
        {

            ViewBag.cat_id = cat_id;
            ViewBag.page = page;

            int goodsStatusOn = Convert.ToInt16(DataConfig.GoodsStatusEnum.上架);
            int g_type = Convert.ToInt16(DataConfig.GoodsTypeEnum.康复类);

            ViewBag.Catagorys = work.GoodsCategoryRepository.Get(m => m.GC_Type == g_type).ToList();

            var rst = work.Context.Goods.Where(m => m.G_Status == goodsStatusOn && m.GoodsTypeID == g_type);
            if (cat_id != 0)
            {
                rst = rst.Where(m => m.GoodsCategoryID == cat_id);
            }

            rst = rst.OrderByDescending(m => m.G_IsRecommend).ThenByDescending(m => m.G_Sort).ThenByDescending(m => m.ID);
            int pageSize = 40;
            return View(rst.ToPagedList(page, pageSize));
        }

        public ActionResult PPTDetail(int ID = 0, int ga_id = 0)
        {
            if (LoginedUserModel == null)
            {
                //return RedirectToAction("Index", "Login",);
                Response.Redirect("/mobile/login?returnurl=" + Server.UrlEncode(Request.Url.ToString()));
                Response.End();
            }
            if (ID != 0)
            {
                var model = work.Context.Goods.Where(m => m.ID == ID).FirstOrDefault<Goods>();
                if (model != null)
                {
                    //var articles = work.Context.GoodsArticles.Where(m => m.GoodsID == model.ID && m.GA_IsEnable == 1).OrderByDescending(m => m.GA_Sort).ThenBy(m => m.ID).ToList();
                    //if (ga_id != 0)
                    //{
                    //    GoodsArticle articleModel = work.GoodsArticleRepository.GetByID(ga_id);
                    //    ViewBag.ArticleModel = articleModel;

                    //    articleModel.GA_ShowTimes++;
                    //    work.GoodsArticleRepository.Update(articleModel);
                    //    work.Save();
                    //}
                    //else if (articles != null && articles.Count > 0)
                    //{
                    //    ViewBag.ArticleModel = articles.First();
                    //}
                    //ViewBag.Articles = articles;

                    model.G_ShowTimes = model.G_ShowTimes + 1;
                    work.GoodsRepository.Update(model);
                    work.Save();

                    ViewBag.GoodsCategoryModel = work.Context.GoodsCategorys.AsNoTracking().Where(m => m.ID == model.GoodsCategoryID).FirstOrDefault();

                    AddGoodsArticleRecord(model.ID, 0, LoginedUserModel.ID);

                    //浏览记录
                    BrowseRecordService.Add(LoginedUserModel, CurrentShopModel, Request.Url.ToString(), model.ID, Convert.ToInt16(DataConfig.BrowseRecordTypeEnum.PPT));
                    return View(model);
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region 讲议类

        public ActionResult Jiangyi(int cat_id = 0, int page = 1)
        {

            ViewBag.cat_id = cat_id;
            ViewBag.page = page;

            int goodsStatusOn = Convert.ToInt16(DataConfig.GoodsStatusEnum.上架);
            int g_type = Convert.ToInt16(DataConfig.GoodsTypeEnum.康复类);

            ViewBag.Catagorys = work.GoodsCategoryRepository.Get(m => m.GC_Type == g_type).ToList();

            var rst = work.Context.Goods.Where(m => m.G_Status == goodsStatusOn && m.GoodsTypeID == g_type);
            if (cat_id != 0)
            {
                rst = rst.Where(m => m.GoodsCategoryID == cat_id);
            }

            rst = rst.OrderByDescending(m => m.G_IsRecommend).ThenByDescending(m => m.G_Sort).ThenByDescending(m => m.ID);
            int pageSize = 40;
            return View(rst.ToPagedList(page, pageSize));
        }

        public ActionResult JiangyiDetail(int ID = 0, int ga_id = 0)
        {
            if (LoginedUserModel == null)
            {
                //return RedirectToAction("Index", "Login",);
                Response.Redirect("/mobile/login?returnurl=" + Server.UrlEncode(Request.Url.ToString()));
                Response.End();
            }
            if (ID != 0)
            {
                var model = work.Context.Goods.Where(m => m.ID == ID).FirstOrDefault<Goods>();
                if (model != null)
                {
                    //var articles = work.Context.GoodsArticles.Where(m => m.GoodsID == model.ID && m.GA_IsEnable == 1).OrderByDescending(m => m.GA_Sort).ThenBy(m => m.ID).ToList();
                    //if (ga_id != 0)
                    //{
                    //    GoodsArticle articleModel = work.GoodsArticleRepository.GetByID(ga_id);
                    //    ViewBag.ArticleModel = articleModel;

                    //    articleModel.GA_ShowTimes++;
                    //    work.GoodsArticleRepository.Update(articleModel);
                    //    work.Save();
                    //}
                    //else if (articles != null && articles.Count > 0)
                    //{
                    //    ViewBag.ArticleModel = articles.First();
                    //}
                    //ViewBag.Articles = articles;

                    //model.G_ShowTimes = model.G_ShowTimes + 1;
                    //work.GoodsRepository.Update(model);
                    //work.Save();

                    AddGoodsArticleRecord(model.ID, 0, LoginedUserModel.ID);

                    //浏览记录
                    BrowseRecordService.Add(LoginedUserModel, CurrentShopModel, Request.Url.ToString(), model.ID, Convert.ToInt16(DataConfig.BrowseRecordTypeEnum.课程));
                    return View(model);
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region 获取评价

        /// <summary>
        /// 获取评价
        /// </summary>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public ActionResult GetComments(int goodsID = 0, int page = 1, int pagesize = 10)
        {
            var rst = work.Context.OrderComment
               .Join(work.Context.Users, oc => oc.UserID, u => u.ID, (oc, u) => new { oc, u })
               .Join(work.Context.OrderDetails, ocu => ocu.oc.OrderDetailID, od => od.ID, (ocu, od) => new { ocu.oc, ocu.u, od })
               .Where(m => m.oc.OC_IsDelete == 0 && m.od.GoodsID == goodsID)
               .Select(m => new
               {
                   m.oc,
                   m.u
               }).OrderByDescending(m => m.oc.ID).Skip((page - 1) * pagesize).Take(pagesize).ToList();

            List<OrderCommentClient> list = rst.Select(m => new OrderCommentClient
            {
                Content = m.oc.OC_Content,
                NickName = UserService.GetNickName(m.u.U_NickName, m.oc.OC_IsHiddenName),
                Photos = GetPhotoList(m.oc.OC_Images),
                ReplyContent = m.oc.OC_ReplyContent,
                ScoreGoods = m.oc.OC_ScoreGoods,
                Time = m.oc.OC_CreateTime.ToString("yyyy年MM月dd日"),
                UserImg = UserService.GetThumbnail(m.u.U_Thumbnail)
            }).ToList();
            json.Data = new { status = "success", msg = "", list = list };

            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        private List<string> GetPhotoList(string photos)
        {
            if (string.IsNullOrEmpty(photos))
            {
                return null;
            }
            else
            {
                string[] list = photos.Split('|');
                List<string> listPhoto = new List<string>();
                foreach (var item in list)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        listPhoto.Add(SiteService.GetImgUrl(item));
                    }
                }
                return listPhoto;
            }
        }

        #endregion

        #region 新增学习记录

        /// <summary>
        /// 新增学习记录
        /// </summary>
        /// <param name="goodsid"></param>
        /// <param name="goodsArticleID"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool AddGoodsArticleRecord(int goodsid, int goodsArticleID, int userid)
        {
            //新增学习记录
            GoodsArticleRecord articleRecord = new GoodsArticleRecord();
            string beginTime = CookieHelper.GetValue("recordBeginTime");
            if (!string.IsNullOrEmpty(beginTime))
            {
                articleRecord.GAR_Time = Convert.ToDateTime(beginTime);
            }
            else
            {
                articleRecord.GAR_Time = DateTime.Now.AddMinutes(-5);
            }
            articleRecord.GAR_EndTime = DateTime.Now;
            articleRecord.GAR_State = 1;
            articleRecord.GoodsID = goodsid;
            articleRecord.GoodsArticleID = goodsArticleID;
            articleRecord.UserID = userid;

            work.GoodsArticleRecordRepository.Insert(articleRecord);
            work.Save();

            return true;
        }
        #endregion
    }
}