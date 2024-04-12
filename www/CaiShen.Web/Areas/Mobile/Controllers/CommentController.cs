using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.DAL;
using Pannet.DAL.Repository;
using Pannet.Models;
using Pannet.Utility;
using System.Text;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class CommentController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();
        JsonResult json = new JsonResult
        {
            Data = new { }
        };

        public ActionResult Feedback()
        {
            return View();
        }

        #region 咨询产品留言

        /// <summary>
        /// 咨询产品留言
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddConsult(string title, string tel)
        {
            //if (LoginedUserModel == null)
            //{
            //    json.Data = new { status = "-1", msg = "请先登录!" };
            //    return Json(json.Data, JsonRequestBehavior.AllowGet);
            //}
            try
            {
                //当前店铺
                UserShop currentUserShop = UserShopService.GetCurrentShop();

                ConsultMessage newModel = new ConsultMessage();
                newModel.Title = title;
                newModel.Tel = tel;
                if (LoginedUserModel != null)
                {
                    newModel.UserID = LoginedUserModel.ID;
                }
                if (currentUserShop != null)
                {
                    newModel.ShopName = currentUserShop.Shop_Name;
                    newModel.UserShopID = currentUserShop.ID;
                }
                work.ConsultMessageRepository.Insert(newModel);
                work.Save();
                //work.Dispose();
                string mobile = ConfigHelper.GetConfigString("SmsOrderMobile");
                if (!string.IsNullOrEmpty(currentUserShop.Shop_Mobile) && PageValidate.IsNumber(currentUserShop.Shop_Mobile.Replace(",", "")))//如果联系电话是手机，且不为空，发送短信提示，否则发送到总商城默认手机上。
                {
                    mobile = currentUserShop.Shop_Mobile;
                }
                //SmsService.SendSms(mobile, currentUserShop.Shop_Name, "新留言", "处理");

                json.Data = new { status = "success", msg = "提交成功!" };

            }
            catch
            {
                json.Data = new { status = "-1", msg = "操作失败!" };
            }
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 培训需求提交

        /// <summary>
        /// 培训需求提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PeiXunApply(string username, string company, string content, string tel, string address)
        {
            if (LoginedUserModel == null)
            {
                json.Data = new { status = "-1", msg = "请先登录!" };
                return Json(json.Data, JsonRequestBehavior.AllowGet);
            }
            try
            {

                ConsultMessage newModel = new ConsultMessage();
                newModel.Title = "培训需求留言";
                newModel.Tel = tel;
                newModel.UserName = username;
                newModel.ShopName = company;
                newModel.Content = content;
                newModel.Address = address;
                if (LoginedUserModel != null)
                {
                    newModel.UserID = LoginedUserModel.ID;
                }
                work.ConsultMessageRepository.Insert(newModel);
                work.Save();
                //work.Dispose();
                //SmsService.SendSms(mobile, currentUserShop.Shop_Name, "新留言", "处理");

                json.Data = new { status = "success", msg = "提交成功!" };

            }
            catch
            {
                json.Data = new { status = "-1", msg = "操作失败!" };
            }
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 获取评论列表

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid">课程ID</param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="sort"></param>
        /// <param name="type">0课程 1新闻 </param>
        /// <returns></returns>
        public ActionResult GetPageComments(int pid = 0, int pagesize = 10, int pageindex = 1, string sort = "time", int type = 1)
        {

            int totalrows = 0;
            int totalpages = 1;
            string pagerHtml = "";
            //string orderby = " order by com_id desc ";
            //if (sort == "hot")
            //{
            //    orderby = " order by Com_ReplyCount+Com_Zan desc ";
            //}
            var rst = work.Context.Comments.Where(m => m.Comm_P_Id == pid && m.Comm_Status == 1 && m.Comm_Type == 0 && m.Comm_ReplyId == 0 && m.Comm_ZanId == 0);

            StringBuilder ContentHtml = new StringBuilder();


            var pageRst = rst
               .Join(work.Context.Users, c => c.Comm_UserId, u => u.ID, (c, u) => new CommentGoodsVModel
               {
                   Comment = c,
                   UserName = u.U_NickName == "" ? u.U_UserName : u.U_NickName,
                   GoodsName = "",
                   UserIMG = u.U_Thumbnail
               }).OrderByDescending(m => m.Comment.ID).Take(pagesize).Skip((pageindex - 1) * pagesize)
                .ToList();
            if (pageRst != null)
            {
                foreach (CommentGoodsVModel commModel in pageRst)
                {
                    int replyCount = work.Context.Comments.Where(m => m.Comm_P_Id == pid && m.Comm_ReplyId == commModel.Comment.ID).Count();
                    ContentHtml.AppendFormat("<dd><a href=\"javascript:void(0);\" class=\"headpic\"><img src=\"{0}\" /></a>", commModel.UserIMG);
                    ContentHtml.AppendFormat("<div class=\"message_info\"><a href=\"javascript:void(0);\" class=\"name\">{0}</a><span class=\"time\">{1}</span><br><p>{2}</p>", commModel.UserName, commModel.Comment.Comm_Time.ToString("yyyy-MM-dd"), commModel.Comment.Comm_Content);
                    //<a href=\"javascript:void(0);\" class=\"zan\" data-writer=\"{2}\" data-cid=\"{3}\">赞(<span>{0}</span>)</a>
                    ContentHtml.AppendFormat("<div class=\"operate\"> <a href=\"javascript:void(0);\" class=\"reply\" data-writer=\"{2}\" data-cid=\"{3}\">回复(<span>{1}</span>)</a></div>", commModel.Comment.Comm_Zan, replyCount, commModel.UserName, commModel.Comment.ID);
                    ContentHtml.AppendFormat("</div></dd>");
                    ContentHtml.AppendFormat("<dd class=\"reply_box_line\"><div class=\"reply_box\" data-writer=\"{0}\" data-cid=\"{1}\" ><textarea name=\"content\" cols=\"10\" rows=\"10\" placeholder=\"请输入回复内容\"></textarea>", commModel.UserName, commModel.Comment.ID);
                    ContentHtml.AppendFormat("<div class=\"sendline\"><input type=\"button\" class=\"btn btnReply\" value=\"回复\" data-writer=\"{0}\" data-cid=\"{1}\"/></div></div></dd>", commModel.UserName, commModel.Comment.ID);

                    GetReplysHtml(pid, commModel.Comment.ID, ContentHtml);
                }
            }
            totalrows = work.Context.Comments.Where(m => m.Comm_P_Id == pid && m.Comm_Status == 1 && m.Comm_Type == 0 && m.Comm_ReplyId == 0 && m.Comm_ZanId == 0).Count();
            if (totalrows > 0)
            {
                totalpages = Convert.ToInt32((totalrows - 1) / pagesize) + 1;//总页数
            }
            //pagerHtml = PagerHelper.GetPageHtmlArticle(int.Parse(pageindex), totalpages, int.Parse(pagesize), totalrows, "GetPageComments");
            if (ContentHtml.ToString() == "")
            {
                json.Data = new { status = "success", msg = "加载成功!" };
            }
            else
            {
                //ContentHtml.AppendFormat("<dd><div class=\"fenye\">{0}</div></dd>", pagerHtml);
                json.Data = new { status = "success", msg = "加载成功!", body = ContentHtml.ToString(), totalCount = totalrows };
            }
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获得当前评论下回复内容
        /// </summary>
        /// <param name="pid">当前文章</param>
        /// <param name="com_id">当前评论</param>
        /// <param name="ContentHtml"></param>
        /// <returns></returns>
        public void GetReplysHtml(int pid, int com_id, StringBuilder ContentHtml)
        {
            var pageRst = work.Context.Comments.Where(m => m.Comm_P_Id == pid && m.Comm_Status == 1 && m.Comm_Type == 0 && m.Comm_ReplyId == com_id)
               .Join(work.Context.Users, c => c.Comm_UserId, u => u.ID, (c, u) => new CommentGoodsVModel
               {
                   Comment = c,
                   UserName = u.U_NickName == "" ? u.U_UserName : u.U_NickName,
                   GoodsName = "",
                   UserIMG = u.U_Thumbnail
               }).Take(100).OrderByDescending(m => m.Comment.ID)
                .ToList();
            if (pageRst == null || pageRst.Count() < 1)
                return;
            foreach (CommentGoodsVModel replyModel in pageRst)
            {
                int replyCount = work.Context.Comments.Where(m => m.Comm_P_Id == pid && m.Comm_ReplyId == replyModel.Comment.ID).Count();
                ContentHtml.AppendFormat("<dd class=\"replyline\"><a href=\"javascript:void(0);\" class=\"headpic\"><img src=\"{0}\" /></a>", replyModel.UserIMG);
                ContentHtml.AppendFormat("<div class=\"message_info\"><a href=\"javascript:void(0);\" class=\"name\">{0}</a><span class=\"time\">{1}</span><br><p>{2}</p>", replyModel.UserName, replyModel.Comment.Comm_Time.ToString("yyyy-MM-dd"), replyModel.Comment.Comm_Content);
                //<a href=\"javascript:void(0);\" class=\"zan\" data-writer=\"{2}\" data-cid=\"{3}\">赞(<span>{0}</span>)</a>
                ContentHtml.AppendFormat("<div class=\"operate\"> <a href=\"javascript:void(0);\" class=\"reply\" data-writer=\"{2}\" data-cid=\"{3}\">回复(<span>{1}</span>)</a></div>", replyModel.Comment.Comm_Zan, replyCount, replyModel.UserName, replyModel.Comment.ID);
                ContentHtml.AppendFormat("</div></dd>");
                ContentHtml.AppendFormat("<dd class=\"reply_box_line\"><div class=\"reply_box\" data-writer=\"{0}\" data-cid=\"{1}\" ><textarea name=\"content\" cols=\"10\" rows=\"10\" placeholder=\"请输入回复内容\"></textarea>", replyModel.UserName, replyModel.Comment.ID);
                ContentHtml.AppendFormat("<div class=\"sendline\"><input type=\"button\" class=\"btn btnReply\" value=\"回复\" data-writer=\"{0}\" data-cid=\"{1}\"/></div></div></dd>", replyModel.UserName, replyModel.Comment.ID);

                GetReplysHtml(pid, replyModel.Comment.ID, ContentHtml);
            }

        }


        /// <summary>
        /// 评价
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="zanId"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
       [ValidateInput(false)]
        public ActionResult AddComment(int pid = 0, int replyId = 0, string title = "", string content = "")
        {
            int type = 0;//0产品 1新闻

            if (LoginedUserModel == null)
            {
                json.Data = new { status = "error", msg = "请先登录!" };
                return Json(json.Data, JsonRequestBehavior.AllowGet);
            }

            Comment newModel = new Comment();

            newModel.Comm_Content = content;
            newModel.Comm_P_Id = pid;
            newModel.Comm_Time = DateTime.Now;
            newModel.Comm_Title = title;
            newModel.Comm_UserId = LoginedUserModel.ID;
            newModel.Comm_ReplyId = replyId;
            newModel.Comm_ZanId = 0;
            newModel.Comm_Status = 1;
            newModel.Comm_Type = type;

            work.CommentRepository.Insert(newModel);
            work.Save();

            json.Data = new { status = "success", msg = "发布成功!" };
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="zanId"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public ActionResult AddZan(int pid = 0, int zanId = 0, string title = "", string content = "点赞")
        {
            int type = 0;//0产品 1新闻

            if (LoginedUserModel == null)
            {
                json.Data = new { status = "error", msg = "请先登录!" };
                return Json(json.Data, JsonRequestBehavior.AllowGet);
            }

            int zanCount = work.Context.Comments.Where(m => m.Comm_UserId == LoginedUserModel.ID && m.Comm_ZanId == zanId && m.Comm_P_Id == pid).Count();
            if (zanCount > 0)
            {
                json.Data = new { status = "error", msg = "您已点过赞!" };
                return Json(json.Data, JsonRequestBehavior.AllowGet);
            }

            Comment newModel = new Comment();

            newModel.Comm_Content = content;
            newModel.Comm_P_Id = pid;
            newModel.Comm_Time = DateTime.Now;
            newModel.Comm_Title = title;
            newModel.Comm_UserId = LoginedUserModel.ID;
            newModel.Comm_ReplyId = 0;
            newModel.Comm_ZanId = zanId;
            newModel.Comm_Status = 1;
            newModel.Comm_Type = type;

            work.CommentRepository.Insert(newModel);
            work.Save();

            json.Data = new { status = "success", msg = "点赞成功!" };
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}