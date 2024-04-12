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
    /// 按需定制留言
    /// </summary>
    public class OrderCustomMessageController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 按需定制留言

        //默认按需定制留言列表
        [CheckPermission]
        public ActionResult Index(string keyword = "", int page = 1)
        {
            ViewBag.keyword = keyword;

            var rst = work.Context.OrderCustomMessages.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.OC_Name.Contains(keyword) || m.OC_UserName.Contains(keyword) || m.OC_Tel.Contains(keyword) || m.OC_Type.Contains(keyword));
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
        public ActionResult Add(int ID = 0)
        {
            ViewBag.ID = ID;

            if (ID != 0)
            {
                var model = work.OrderCustomMessageRepository.Get(m => m.ID == ID).FirstOrDefault<OrderCustomMessage>();
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
        [CheckPermission]
        [HttpPost]
        public ActionResult Add(OrderCustomMessage newModel, int ID = 0)
        {
            ViewBag.ID = ID;
            if (ModelState.IsValid)
            {
                if (ID == 0)//新增
                {
                    work.OrderCustomMessageRepository.Insert(newModel);
                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                }
                else
                {
                    OrderCustomMessage existModel = work.OrderCustomMessageRepository.GetByID(ID);
                    if (existModel != null)
                    {
                        existModel.OC_Price = newModel.OC_Price;
                        existModel.OC_Count = newModel.OC_Count;
                        existModel.OC_Size = newModel.OC_Size;
                        existModel.OC_Remark = newModel.OC_Remark;
                        existModel.OC_Address = newModel.OC_Address;
                        existModel.OC_ShippingInfo = newModel.OC_ShippingInfo;
                        existModel.OC_Reply = newModel.OC_Reply;
                        existModel.OC_Status = newModel.OC_Status;

                        work.OrderCustomMessageRepository.Update(existModel);
                        work.Save();
                        work.Dispose();
                    }

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    return RedirectToAction("Index");
                }
            }
            return View(newModel);
        }

        /// <summary>
        /// 删除按需定制留言
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
                var role = work.OrderCustomMessageRepository.Get(m => m.ID == ID).FirstOrDefault<OrderCustomMessage>();
                work.OrderCustomMessageRepository.Delete(role);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("Index");
        }

        #endregion

    }
}