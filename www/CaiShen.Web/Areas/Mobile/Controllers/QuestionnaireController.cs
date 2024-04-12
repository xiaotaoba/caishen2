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
    public class QuestionnaireController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();
        JsonResult json = new JsonResult
        {
            Data = new { }
        };
        public ActionResult Index(int ID = 0, int page = 1)
        {
            if (LoginedUserModel == null)
            {
                //return RedirectToAction("Index", "Login",);
                Response.Redirect("/mobile/login?returnurl=" + Server.UrlEncode(Request.Url.ToString()));
                Response.End();
            }
            ViewBag.ID = ID;
            ViewBag.page = page;

            //已参与记录
            if (LoginedUserModel != null)
            {
                ViewBag.ExistRecordQuest_IDS = work.QuestionnaireRecordRepository.Get(m => m.UserID == LoginedUserModel.ID).Select(m => m.Quest_ID).ToList();
            }

            var rst = work.Context.Questionnaires.Where(m => m.Quest_Status == 1);
            rst = rst.OrderByDescending(m => m.Quest_IsRecommend).ThenByDescending(m => m.Quest_Sort).ThenByDescending(m => m.ID);
            int pageSize = 40;
            return View(rst.ToPagedList(page, pageSize));
        }

        public ActionResult Detail(int ID = 0)
        {
            if (LoginedUserModel == null)
            {
                //return RedirectToAction("Index", "Login",);
                Response.Redirect("/mobile/login?returnurl=" + Server.UrlEncode(Request.Url.ToString()));
                Response.End();
            }
            if (ID != 0)
            {
                var model = work.Context.Questionnaires.Where(m => m.ID == ID).FirstOrDefault<Questionnaire>();
                if (model != null)
                {
                    ViewBag.Questionnaire = model;
                    //浏览记录
                    //BrowseRecordService.Add(LoginedUserModel, CurrentShopModel, Request.Url.ToString(), model.ID, Convert.ToInt16(DataConfig.BrowseRecordTypeEnum.设计作品));
                }
            }
            

            int q_group = Convert.ToInt16(DataConfig.QuestionGroupEnum.问卷调查);
            var rst = work.Context.Questions.Where(m => m.Q_GroupItemID == ID && m.Q_Group == q_group)
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
        /// 保存问卷调查数据
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="quest_id">问卷调查ID</param>
        /// <returns></returns>Goods
        public ActionResult Save(string jsonData, int quest_id = 0)
        {
            if (quest_id == 0)
            {
                return RedirectToAction("Index");
            }

            if (LoginedUserModel == null)
            {
                json.Data = new { status = "error", msg = "请先登录" };
                return Json(json.Data, JsonRequestBehavior.AllowGet);
            }

            //提交问卷数据
            List<RequestTestQuestionVModel> requestList = JsonHelper.DeserializeJsonToList<RequestTestQuestionVModel>(jsonData);

            //判断当前用户是否已有调查记录
            var existCount = work.QuestionnaireRecordRepository.Get(m => m.UserID == LoginedUserModel.ID && m.Quest_ID == quest_id).Count();
            if (existCount > 0)
            {
                json.Data = new { status = "error", msg = "您之前已参与过调查！" };
                return Json(json.Data, JsonRequestBehavior.AllowGet);
            }
            //调查记录
            int score = Convert.ToInt16(ConfigHelper.GetConfigString("QuestionnaireScore"));
            QuestionnaireRecord recordModel = new QuestionnaireRecord();
            recordModel.Quest_ID = quest_id;
            recordModel.Score = score;
            recordModel.UserID = LoginedUserModel.ID;

            work.QuestionnaireRecordRepository.Insert(recordModel);
            work.Save();

            //问卷详细选项（答案）
            QuestionnaireRecordAnswer recordAnswerModel = null;
            foreach (var item in requestList)
            {
                recordAnswerModel = new QuestionnaireRecordAnswer();
                recordAnswerModel.QuestionnaireRecordID = recordModel.ID;
                recordAnswerModel.UserID = LoginedUserModel.ID;
                recordAnswerModel.QuestionID = item.ID;
                recordAnswerModel.Quest_ID = quest_id;
                recordAnswerModel.AnswerIds = item.Answer.Trim(',');

                work.QuestionnaireRecordAnswerRepository.Insert(recordAnswerModel);
            }
            work.Save();


            //调查送积分
            User user = work.UserRepository.GetByID(LoginedUserModel.ID);
            user.U_Score = user.U_Score + score;

            work.UserRepository.Update(user);
            work.Save();

            UserService.SetCacheUser(user.U_UserName, user);

            UserScoreHistoryService.Insert(user.ID, score, user.U_Score, 0, user.U_LockScore, 1, "收入", "调查送积分", recordModel.ID, user.U_UserName);

            json.Data = new { status = "success", msg = "" };
            return Json(json.Data, JsonRequestBehavior.AllowGet);

            //int q_group = Convert.ToInt16(DataConfig.QuestionGroupEnum.课程测试);
            //var rst = work.Context.Questions.Where(m => m.Q_GroupItemID == goods_id && m.Q_Group == q_group);
            //rst = rst.OrderByDescending(m => m.Q_Sort).ThenByDescending(m => m.ID);
            //int pageSize = 40;
            //return View(rst.ToPagedList(1, pageSize));
        }

    }
}