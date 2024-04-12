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
    /// <summary>
    /// 咨询产品-留言
    /// </summary>
    public class ConsultMessageController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 咨询留言

        //默认咨询留言列表
        [CheckPermission]
        public ActionResult Index(string keyword = "", int page = 1, string action = "", string time_start = "", string time_end = "")
        {
            ViewBag.keyword = keyword;
            ViewBag.action = action;

            var rst = work.Context.ConsultMessages.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.Title.Contains(keyword));
            }
            rst = rst.OrderByDescending(m => m.ID);

            if (action == "export")//导出
            {
                string fileName = "培训需求" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                ExportMessage(rst.ToList(), fileName);
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
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 添加/编辑页面
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult Add(int ID = 0)
        {
            ViewBag.ID = ID;

            if (ID != 0)
            {
                var model = work.ConsultMessageRepository.Get(m => m.ID == ID).FirstOrDefault<ConsultMessage>();
                return View(model);
            }
            return View(new ArticleType());
        }

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="userRoleModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        [HttpPost]
        public ActionResult Add(ConsultMessage newModel, int ID = 0)
        {
            ViewBag.ID = ID;
            if (ModelState.IsValid)
            {
                if (ID == 0)//新增
                {
                    work.ConsultMessageRepository.Insert(newModel);
                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                }
                else
                {
                    ConsultMessage existModel = work.ConsultMessageRepository.GetByID(ID);
                    if (existModel != null)
                    {
                        existModel.IsContact = newModel.IsContact;
                        existModel.Remark = newModel.Remark;
                        work.ConsultMessageRepository.Update(existModel);
                        work.Save();
                        work.Dispose();
                    }

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    return View(existModel);
                }
            }
            return View(newModel);
        }

        /// <summary>
        /// 删除咨询留言
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult Delete(int ID = 0)
        {
            if (ID != 0)
            {
                if (ID == 1)
                {
                    return RedirectToAction("Index");
                }
                var role = work.ConsultMessageRepository.Get(m => m.ID == ID).FirstOrDefault<ConsultMessage>();
                work.ConsultMessageRepository.Delete(role);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("Index");
        }

        #endregion

        #region 导出

        public void ExportMessage(List<ConsultMessage> list, string fileName)
        {
            HttpResponseBase resp;
            resp = HttpContext.Response;
            resp.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            resp.ContentType = "application/ms-excel";
            string colHeaders = "", ls_item = "";

            colHeaders += "标题" + "\t";
            colHeaders += "联系人" + "\t";
            colHeaders += "联系电话" + "\t";
            colHeaders += "所在地" + "\t";
            colHeaders += "留言时间" + "\t";
            colHeaders += "是否联系" + "\n";


            resp.Write(colHeaders);
            //向HTTP输出流中写入取得的数据信息 

            //逐行处理数据   
            foreach (var item in list)
            {
                ls_item += item.Title + "\t";
                ls_item += item.UserName + "\t";
                ls_item += item.Tel + "\t";
                ls_item += item.Address + "\t";
                ls_item += item.CreateTime + "\t";
                ls_item += (item.IsContact == 1 ? "已联系" : "未联系") + "\n";

                resp.Write(ls_item);
                ls_item = "";

            }
            resp.End();
        }

        #endregion

    }
}