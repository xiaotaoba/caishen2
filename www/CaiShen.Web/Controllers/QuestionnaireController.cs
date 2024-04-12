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
using System.Text;

//using System.ComponentModel.DataAnnotations;

namespace Pannet.Web.Controllers
{
    public class QuestionnaireController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();
        JsonResult json = new JsonResult
        {
            Data = new { }
        };
        #region 问卷调查列表

        [CheckPermission]
        public ActionResult Index(string keyword = "", int page = 1, string action = "", int status = -1)
        {
            ViewBag.Keyword = keyword;
            ViewBag.status = status;
            ViewBag.action = action;


            var rst = work.Context.Questionnaires.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.Quest_Title.Contains(keyword));
            }
            //启用状态
            if (status != -1)
            {
                rst = rst.Where(m => m.Quest_Status == status);
            }
            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 20;
            return View(rst.ToPagedList(page, pageSize));
        }

        #endregion

        #region 添加/编辑问卷调查

        /// <summary>
        /// 添加/编辑问卷调查
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        [ValidateInput(false)]
        public ActionResult Add(int ID = 0)
        {
            ViewBag.Quest_ID = ID;
            if (ID != 0)
            {
                var existModel = work.QuestionnaireRepository.GetByID(ID);
                return View(existModel);

            }
            return View(new Questionnaire());
        }

        /// <summary>
        /// 添加/编辑问卷调查-post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission]
        [ValidateInput(false)]
        public ActionResult Add(Questionnaire newModel, int ID = 0)
        {
            ViewBag.Quest_ID = ID;
            if (ModelState.IsValid)
            {
                if (ID == 0)
                {
                    work.QuestionnaireRepository.Insert(newModel);
                    work.Save();

                    LogService.Add(ManagerService.GetLoginModel(), "新增问卷调查信息:" + newModel.Quest_Title, newModel.ID.ToString());
                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    work.Dispose();
                }
                else
                {
                    work.QuestionnaireRepository.Update(newModel);
                    work.Save();

                    LogService.Add(LoginedAdminModel, "编辑问卷调查信息:" + newModel.Quest_Title, newModel.ID.ToString());
                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    work.Dispose();
                }
            }
            return View(newModel);
        }

        #endregion

        #region 删除
        /// <summary>
        /// 删除问卷调查
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
                var role = work.QuestionnaireRepository.Get(m => m.ID == ID).FirstOrDefault<Questionnaire>();
                work.QuestionnaireRepository.Delete(role);
                work.Save();
                work.Dispose();
                LogService.Add(ManagerService.GetLoginModel(), "删除问卷调查信息", ID.ToString());

            }
            return RedirectToAction("Index");
        }

        #endregion

        #region 问卷问题

        /// <summary>
        /// 问卷问题
        /// </summary>
        /// <param name="Quest_ID"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult Question(int Quest_ID, int page = 1)
        {
            ViewBag.Quest_ID = Quest_ID;
            int q_group = Convert.ToInt16(DataConfig.QuestionGroupEnum.问卷调查);
            var rst = work.Context.Questions.Where(m => m.Q_GroupItemID == Quest_ID && m.Q_Group == q_group)
                .GroupJoin(work.Context.Answer, q => q.ID, a => a.QuestionID, (q, a) => new { q, a })
                .Select(m => new QuestionAnswerVModel
                {
                    Question = m.q,
                    AnswerList = m.a
                });
            //.SelectMany(m=>m.q,  (q,a)=>new{ q,a});
            rst = rst.OrderByDescending(m => m.Question.ID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
            //return View(work.GoodsArticleRepository.Get());
        }

        /// <summary>
        /// 添加试题
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult QuestionAdd(int Quest_ID)
        {
            ViewBag.Quest_ID = Quest_ID;
            return View(new Question());
        }

        /// <summary>
        /// 添加处理
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        [HttpPost]
        public ActionResult QuestionAdd(Question newModel, int Quest_ID, int ID = 0, List<AnswerVModel> answer = null)
        {
            ViewBag.Quest_ID = Quest_ID;
            int q_group = Convert.ToInt16(DataConfig.QuestionGroupEnum.问卷调查);

            //if (ModelState.IsValid)
            //{
            newModel.Q_Group = q_group;
            newModel.Q_GroupItemID = Quest_ID;
            work.QuestionRepository.Insert(newModel);
            work.Save();

            if (answer != null)
            {
                foreach (var item in answer)
                {
                    if (!string.IsNullOrEmpty(item.answer))
                    {
                        Answer newAnswer = new Answer();
                        newAnswer.A_Answer = item.answer;
                        newAnswer.A_IsTrue = item.istrue;
                        newAnswer.A_Sort = item.sort;
                        newAnswer.QuestionID = newModel.ID;

                        work.AnswerRepository.Insert(newAnswer);
                        work.Save();
                    }
                }
            }
            work.Dispose();

            ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            //}
            return View(newModel);
        }
        /// <summary>
        /// 编辑试题
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult QuestionEdit(int Quest_ID, int ID = 0)
        {
            ViewBag.Quest_ID = Quest_ID;

            if (ID != 0)
            {
                var model = work.QuestionRepository.Get(m => m.ID == ID).FirstOrDefault<Question>();
                if (model != null)
                {
                    ViewBag.AnswerList = work.AnswerRepository.Get(m => m.QuestionID == model.ID);
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("QuestionAdd", Quest_ID);
            }
            return View(new Question());
        }

        /// <summary>
        /// 编辑处理
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        [HttpPost]
        public ActionResult QuestionEdit(Question newModel, int Quest_ID, int ID = 0, List<AnswerVModel> answer = null)
        {
            ViewBag.Quest_ID = Quest_ID;
            int q_group = Convert.ToInt16(DataConfig.QuestionGroupEnum.问卷调查);

            ViewBag.AnswerList = work.AnswerRepository.Get(m => m.QuestionID == ID);

            if (ModelState.IsValid)
            {
                if (ID == 0)//新增
                {
                }
                else
                {
                    newModel.Q_Group = q_group;
                    newModel.Q_GroupItemID = Quest_ID;
                    work.QuestionRepository.Update(newModel);

                    if (answer != null)
                    {
                        foreach (var item in answer)
                        {
                            if (!string.IsNullOrEmpty(item.answer))
                            {
                                Answer newAnswer = new Answer();
                                newAnswer.A_Answer = item.answer;
                                newAnswer.A_IsTrue = item.istrue;
                                newAnswer.A_Sort = item.sort;
                                newAnswer.QuestionID = newModel.ID;
                                if (item.id == 0)
                                {
                                    work.AnswerRepository.Insert(newAnswer);
                                }
                                else
                                {
                                    newAnswer.ID = item.id;
                                    work.AnswerRepository.Update(newAnswer);
                                    work.Save();
                                }
                            }
                        }
                    }

                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    return RedirectToAction("Question", new { Quest_ID });
                }
            }
            return View(newModel);
        }

        /// <summary>
        /// 删除试题
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult QuestionDelete(int Quest_ID, int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.QuestionRepository.Get(m => m.ID == ID).FirstOrDefault<Question>();
                work.QuestionRepository.Delete(model);
                work.Save();
                work.Dispose();
            }

            return RedirectToAction("Question", new { Quest_ID });
        }
        /// <summary>
        /// 删除试题选项
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult AnswerDelete(int Quest_ID, int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.AnswerRepository.Get(m => m.ID == ID).FirstOrDefault<Answer>();
                work.AnswerRepository.Delete(model);
                work.Save();
                work.Dispose();
            }

            json.Data = new { status = "success", msg = "" };
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 问卷调查记录

        //问卷调查记录
        //[CheckPermission]
        public ActionResult Record(string keyword = "", int Quest_ID = 0, int page = 1, string action = "", string time_start = "", string time_end = "", int province = 0, int city = 0, int region = 0)
        {
            ViewBag.keyword = keyword;
            ViewBag.Quest_ID = Quest_ID;
            ViewBag.action = action;
            ViewBag.province = province;
            ViewBag.city = city;
            ViewBag.region = region;
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);

            var rst = work.Context.QuestionnaireRecords
                .Join(work.Context.Questionnaires, qr => qr.Quest_ID, q => q.ID, (qr, q) => new { qr, q })
                .Join(work.Context.Users, m => m.qr.UserID, u => u.ID, (m, u) => new QuestionnaireRecordVModel
                {
                    QuestionnaireRecord = m.qr,
                    Quest_Title = m.q.Quest_Title,
                    UserName = u.U_UserName,
                    RealName = u.U_RealName,
                    Department = u.U_DepartmentID,
                    City = u.U_City,
                    Province = u.U_Province,
                    ShopName = u.U_ShopName
                });

            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.Quest_Title.Contains(keyword) || m.UserName.Contains(keyword) || m.RealName.Contains(keyword) || m.ShopName.Contains(keyword));
            }
            if (province != 0)
            {
                rst = rst.Where(m => m.Province == province);
            }
            if (city != 0)
            {
                rst = rst.Where(m => m.City == city);
            }
            rst = rst.OrderByDescending(m => m.QuestionnaireRecord.ID);


            if (action == "export")//导出
            {
                string fileName = "问卷调查记录" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                ExportQuestionanaire(rst.ToList(), fileName);
                //try
                //{

                //}
                //catch (Exception ex)
                //{
                //    Response.End();
                //}
            }

            int pageSize = 20;
            int pageNumber = page;
            //return View(rst.ToPagedList(pageNumber, pageSize));
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 删除问卷调查记录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult RecordDelete(int ID = 0, int Quest_ID = 0)
        {
            ViewBag.Quest_ID = Quest_ID;

            if (ID != 0)
            {
                var model = work.QuestionnaireRecordRepository.Get(m => m.ID == ID).FirstOrDefault<QuestionnaireRecord>();
                work.QuestionnaireRecordRepository.Delete(model);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("Record", new { Quest_ID });
        }

        /// <summary>
        /// 删除问卷调查详细（各选项答案）
        /// </summary>
        /// <param name="Record_ID"></param>
        /// <param name="Quest_ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult RecordDetail(int Record_ID = 0, int Quest_ID = 0)
        {
            ViewBag.Quest_ID = Quest_ID;

            if (Record_ID != 0)
            {
                if (Quest_ID != 0)
                {
                    var model = work.Context.Questionnaires.Where(m => m.ID == Quest_ID).FirstOrDefault<Questionnaire>();
                    if (model != null)
                    {
                        ViewBag.Questionnaire = model;
                    }
                }

                //已选选项
                ViewBag.RecordAnswerList = work.QuestionnaireRecordAnswerRepository.Get(m => m.Quest_ID == Quest_ID && m.QuestionnaireRecordID == Record_ID).ToList();

                //问卷试题
                int q_group = Convert.ToInt16(DataConfig.QuestionGroupEnum.问卷调查);
                var rst = work.Context.Questions.Where(m => m.Q_GroupItemID == Quest_ID && m.Q_Group == q_group)
                    .GroupJoin(work.Context.Answer, q => q.ID, a => a.QuestionID, (q, a) => new QuestionAnswerVModel
                    {
                        Question = q,
                        AnswerList = a
                    });

                rst = rst.OrderByDescending(m => m.Question.Q_Sort).ThenBy(m => m.Question.ID);
                int pageSize = 40;

                return View(rst.ToPagedList(1, pageSize));



            }
            return RedirectToAction("Record", new { Quest_ID });
        }

        #endregion

        #region 导出

        public void ExportQuestionanaire(List<QuestionnaireRecordVModel> list, string fileName)
        {
            HttpResponseBase resp;
            resp = HttpContext.Response;
            resp.Charset = "utf-8";
            resp.ContentEncoding = System.Text.Encoding.GetEncoding("GBK");
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            resp.ContentType = "application/ms-excel";
            string colHeaders = "", ls_item = "";

            colHeaders += "问卷名称" + "\t";
            colHeaders += "用户" + "\t";
            colHeaders += "姓名" + "\t";
            colHeaders += "身份" + "\t";
            colHeaders += "省份" + "\t";
            colHeaders += "城市" + "\t";
            colHeaders += "门店" + "\t";
            colHeaders += "调查时间" + "\t";
            colHeaders += "回答情况" + "\n";


            resp.Write(colHeaders);
            //向HTTP输出流中写入取得的数据信息 

            //逐行处理数据   
            foreach (var itemv in list)
            {
                QuestionnaireRecord item = itemv.QuestionnaireRecord;

                //已选选项
                List<QuestionnaireRecordAnswer> recordAnswerList = work.QuestionnaireRecordAnswerRepository.Get(m => m.Quest_ID == item.Quest_ID && m.QuestionnaireRecordID == item.ID).ToList();
                StringBuilder answer_content = new StringBuilder();
                int num = 1;
                foreach (var recordAnswerItem in recordAnswerList)
                {
                    string answerIdsStr = recordAnswerItem.AnswerIds;
                    List<string> answerIds = answerIdsStr.Trim(',').Split(',').ToList();
                    List<Answer> answerList = work.AnswerRepository.Get(m => answerIds.Contains(m.ID.ToString())).ToList();
                    string answerTxt = string.Join(",", answerList.Select(m => m.A_Answer).ToList());
                    answer_content.AppendFormat("【问题{0}】回答（{1}）;", num, answerTxt);
                    num++;
                }


                ls_item += itemv.Quest_Title + "\t";
                ls_item += itemv.UserName + "\t";
                ls_item += itemv.RealName + "\t";

                Department department = work.Context.Departments.Where(m => m.ID == itemv.Department).FirstOrDefault();
                if (department != null)
                {
                    ls_item += department.Dep_Name + "\t";
                }
                else
                {
                    ls_item += " " + "\t";
                }
                Area province = work.Context.Areas.Where(m => m.ID == itemv.Province).FirstOrDefault();
                if (province != null)
                {
                    ls_item += province.Area_Name + "\t";

                }
                else
                {
                    ls_item += " " + "\t";
                }
                Area city = work.Context.Areas.Where(m => m.ID == itemv.City).FirstOrDefault();
                if (city != null)
                {
                    ls_item += city.Area_Name + "\t";

                }
                else
                {
                    ls_item += " " + "\t";
                }
                ls_item += itemv.ShopName + "\t";
                ls_item += item.CreateTime + "\t";
                ls_item += answer_content.ToString() + "\n";

                resp.Write(ls_item);
                ls_item = "";

            }
            resp.End();
        }

        #endregion

        #region 复制问卷

        /// <summary>
        /// 复制问卷
        /// </summary>
        /// <param name="ID">问卷ID</param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult Copy(int ID)
        {
            int q_group = Convert.ToInt16(DataConfig.QuestionGroupEnum.问卷调查);
            var existModel = work.QuestionnaireRepository.GetByID(ID);

            if (existModel != null)
            {
                existModel.Quest_Title = existModel.Quest_Title + " 副本";
                existModel.Quest_CreateTime = DateTime.Now;
                work.QuestionnaireRepository.Insert(existModel);
                work.Save();

                var rst = work.Context.Questions.Where(m => m.Q_GroupItemID == ID && m.Q_Group == q_group)
                 .GroupJoin(work.Context.Answer, q => q.ID, a => a.QuestionID, (q, a) => new { q, a })
                 .Select(m => new QuestionAnswerVModel
                 {
                     Question = m.q,
                     AnswerList = m.a
                 }).ToList();

                if (rst != null && rst.Count() > 0)
                {
                    foreach (var itemQuestion in rst)
                    {
                        Question newModel = itemQuestion.Question;
                        newModel.Q_Group = q_group;
                        newModel.Q_GroupItemID = existModel.ID;
                        newModel.Q_CreateTime = DateTime.Now;

                        work.QuestionRepository.Insert(newModel);
                        work.Save();

                        if (itemQuestion.AnswerList != null)
                        {
                            foreach (var item in itemQuestion.AnswerList)
                            {
                                Answer newAnswer = new Answer();
                                newAnswer.A_Answer = item.A_Answer;
                                newAnswer.A_IsTrue = item.A_IsTrue;
                                newAnswer.A_Sort = item.A_Sort;
                                newAnswer.QuestionID = newModel.ID;

                                work.AnswerRepository.Insert(newAnswer);
                                work.Save();
                            }
                        }
                    }
                }
                work.Save();
                work.Dispose();
            }

            return RedirectToAction("Index");
        }

        #endregion
    }
}