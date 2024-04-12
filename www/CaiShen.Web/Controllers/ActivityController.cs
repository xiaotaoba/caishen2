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
using System.Text;

namespace Pannet.Web.Controllers
{
    public class ActivityController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 广告

        //默认广告列表
        [CheckPermission]
        public ActionResult Index(string keyword = "", int AdvertisementTypeID = 0, int page = 1)
        {
            ViewBag.keyword = keyword;
            ViewBag.AdvertisementTypeID = AdvertisementTypeID;
            ViewBag.AdvertisementType = work.AdvertisementTypeRepository.Get();

            var rst = work.Context.Advertisements.Join(work.Context.AdvertisementTypes, art => art.AdvertisementTypeID, at => at.ID, (art, at) => new { art, at });

            if (AdvertisementTypeID != 0)
            {
                rst = rst.Where(m => m.at.ID == AdvertisementTypeID);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.art.AD_Title.Contains(keyword));
            }
            var list = rst.Select(m => new AdvertisementVModel
             {
                 Advertisement = m.art,
                 AdvertisementType = m.at

             }).OrderByDescending(m => m.Advertisement.ID);

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
            ViewBag.AdvertisementType = work.AdvertisementTypeRepository.Get();
            ViewBag.Departments = work.DepartmentRepository.Get();

            if (ID != 0)
            {
                var model = work.AdvertisementRepository.Get(m => m.ID == ID).FirstOrDefault<Advertisement>();
                //if (model != null)
                //{
                //    ViewBag.AdvertisementTypeID = model.AdvertisementTypeID;
                //}
                return View(model);
            }
            else
            {
            }
            return View(new Advertisement());
        }

        /// <summary>
        ///  添加/编辑处理
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <param name="isSendWxMessage">是否发送微信模板消息</param>
        /// <param name="first">导语</param>
        /// <param name="remark">结束语</param>
        /// <param name="address">活动地址</param>
        /// <param name="content">活动简介</param>
        /// <param name="DepartmentID">发送员工所属部门</param>
        /// <returns></returns>
        [CheckPermission]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(Advertisement newModel, int ID = 0, int isSendWxMessage = 0, string first = "", string remark = "", string address = "", string content = "", int DepartmentID = 0)
        {
            ViewBag.ID = ID;
            ViewBag.AdvertisementType = work.AdvertisementTypeRepository.Get();
            ViewBag.Departments = work.DepartmentRepository.Get();

            if (ModelState.IsValid)
            {
                newModel.AD_WX_First = first;
                newModel.AD_WX_Remark = remark;
                newModel.AD_WX_Address = address;
                newModel.AD_WX_Content = content;
                newModel.AD_WX_DepartmentID = DepartmentID;
                newModel.AD_IsSendWxMessage = isSendWxMessage;

                if (ID == 0)//新增
                {
                    work.AdvertisementRepository.Insert(newModel);
                    work.Save();
                    //work.Dispose();


                    if (isSendWxMessage == 1) //只有新增时才发送，
                    {
                        #region 发送微信模板处理

                        string openid = "";//接收人openid
                        string url = "";//模板消息详情链接

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

                        if (!string.IsNullOrEmpty(newModel.AD_URL))
                        {
                            url = newModel.AD_URL;
                        }
                        else
                        {
                            url = string.Format("{0}/Mobile/Activity/Detail/{1}", ConfigHelper.GetConfigString("WebSiteDomain"), newModel.ID);
                        }

                        List<string> openids = openid.Split(',').ToList();
                        int index = 0;
                        foreach (var item in openids)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                WxTemplateMessage.SendPeiXunMessage(item, first, newModel.AD_Title, address, newModel.AD_BeginTime.ToString(), newModel.AD_EndTime.ToString(), content, remark, url);
                                index++;
                            }
                        }
                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", string.Format("保存成功，共发送{0}条模板消息！", index));

                        #endregion
                    }
                    else
                    {
                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    }

                    work.Dispose();

                }
                else
                {
                    work.AdvertisementRepository.Update(newModel);
                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    return RedirectToAction("Index", new { AdvertisementTypeID = newModel.AdvertisementTypeID });
                }
            }
            return View(newModel);
        }

        /// <summary>
        /// 删除广告
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Delete(int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.AdvertisementRepository.Get(m => m.ID == ID).FirstOrDefault<Advertisement>();
                if (model != null)
                {
                    work.AdvertisementRepository.Delete(model);
                    work.Save();
                    work.Dispose();
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region 广告报名记录

        //广告报名记录
        [CheckPermission]
        public ActionResult Record(string keyword = "", int a_id = 0, int page = 1, string action = "")
        {
            ViewBag.keyword = keyword;
            ViewBag.a_id = a_id;
            ViewBag.action = action;

            var rst = work.Context.AdvertisementRecords
                .Join(work.Context.Advertisements, ar => ar.AdvertisementID, a => a.ID, (ar, a) => new { ar, a })
                .Select(m => new AdvertisementRecordVModel
                {
                    AdvertisementRecord = m.ar,
                    Title = m.a.AD_Title
                });
            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.Title.Contains(keyword) || m.AdvertisementRecord.ADR_UserName.Contains(keyword) || m.AdvertisementRecord.ADR_Tel.Contains(keyword));
            }
            if (a_id != 0)
            {
                rst = rst.Where(m => m.AdvertisementRecord.AdvertisementID == a_id);
            }

            rst = rst.OrderByDescending(m => m.AdvertisementRecord.ID);
            if (action == "export")//导出
            {
                string fileName = "活动报名记录" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                ExportRecord(rst.ToList(), fileName);
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
        /// 删除活动报名记录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult RecordDelete(int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.AdvertisementRecordRepository.Get(m => m.ID == ID).FirstOrDefault<AdvertisementRecord>();
                if (model != null)
                {
                    work.AdvertisementRecordRepository.Delete(model);
                    work.Save();
                    work.Dispose();
                }
            }
            return RedirectToAction("Record");
        }

        #endregion

        #region 导出

        public void ExportRecord(List<AdvertisementRecordVModel> list, string fileName)
        {
            HttpResponseBase resp;
            resp = HttpContext.Response;
            resp.Charset = "utf-8";
            resp.ContentEncoding = System.Text.Encoding.GetEncoding("GBK");
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            resp.ContentType = "application/ms-excel";
            string colHeaders = "", ls_item = "";

            colHeaders += "用户" + "\t";
            colHeaders += "姓名" + "\t";
            colHeaders += "身份" + "\t";
            colHeaders += "所属地区" + "\t";
            colHeaders += "联系人" + "\t";
            colHeaders += "电话" + "\t";
            colHeaders += "报名活动" + "\t";
            colHeaders += "报名时间" + "\t";
            colHeaders += "职位" + "\t";
            colHeaders += "地区" + "\t";
            colHeaders += "审核状态" + "\n";

            resp.Write(colHeaders);
            //向HTTP输出流中写入取得的数据信息 

            //逐行处理数据   
            foreach (var itemv in list)
            {
                AdvertisementRecord item = itemv.AdvertisementRecord;

                User user = work.Context.Users.Where(m => m.ID == item.UserID).FirstOrDefault();
                if (user != null)
                {

                    ls_item += user.U_UserName + "\t";
                    ls_item += user.U_RealName + "\t";
                    Department department = work.Context.Departments.Where(m => m.ID == user.U_DepartmentID).FirstOrDefault();
                    if (department != null)
                    {
                        ls_item += department.Dep_Name + "\t";
                    }
                    else
                    {
                        ls_item += " " + "\t";
                    }
                    Area province = work.Context.Areas.Where(m => m.ID == user.U_Province).FirstOrDefault();
                    if (province != null)
                    {
                        ls_item += province.Area_Name + "-";

                    }
                    else
                    {
                        ls_item += " " + "-";
                    }
                    Area city = work.Context.Areas.Where(m => m.ID == user.U_City).FirstOrDefault();
                    if (city != null)
                    {
                        ls_item += city.Area_Name + "\t";

                    }
                    else
                    {
                        ls_item += " " + "\t";
                    }
                }
                else
                {
                    ls_item += "未登录" + "\t";
                    ls_item += " " + "\t";
                    ls_item += " " + "\t";
                    ls_item += " " + "\t";
                }

                ls_item += item.ADR_UserName + "\t";
                ls_item += item.ADR_Tel + "\t";
                ls_item += itemv.Title + "\t";
                ls_item += item.ADR_CreateTime + "\t";
                ls_item += item.ADR_Position + "\t";
                ls_item += item.ADR_Address + "\t";
                ls_item += DataConfig.AdvertisementRecordStatus.Find(m => m.Value == item.ADR_State.ToString()).Name + "\n";

                resp.Write(ls_item);
                ls_item = "";

            }
            resp.End();
        }

        #endregion

        #region 广告报名审核

        //广告报名审核
        //[CheckPermission]
        public ActionResult RecordState(int ID = 0, int state = 1)
        {
            if (ID != 0)
            {
                var model = work.AdvertisementRecordRepository.Get(m => m.ID == ID).FirstOrDefault();
                if (model != null)
                {
                    model.ADR_State = state;
                    work.AdvertisementRecordRepository.Update(model);
                    work.Save();
                    work.Dispose();
                }
            }
            return RedirectToAction("Record");
        }

        #endregion
    }
}