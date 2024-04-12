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
    public class InvoiceController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 申请开票记录

        //默认列表
        [CheckPermission]
        public ActionResult Index(int page = 1)
        {
            var rst = work.Context.InvoiceLogs.OrderByDescending(m => m.ID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
            //return View(work.HongBaoRepository.Get());
        }

        /// <summary>
        /// 详细页面
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Detail(int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.InvoiceLogRepository.Get(m => m.ID == ID).FirstOrDefault<InvoiceLog>();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// 详细页面 - 保存
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult Detail(InvoiceLog newModel, int ID = 0)
        {
            ViewBag.ID = ID;
            if (ID != 0)//新增
            {
                var existModel = work.Context.InvoiceLogs.AsNoTracking().Where(m => m.ID == ID).FirstOrDefault();
                if (existModel == null || existModel.ID == 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    existModel.Inv_Title = newModel.Inv_Title;
                    existModel.Inv_BusinessTax = newModel.Inv_BusinessTax;
                    existModel.Inv_CompanyAddress = newModel.Inv_CompanyAddress;
                    existModel.Inv_BankName = newModel.Inv_BankName;
                    existModel.Inv_BankNumber = newModel.Inv_BankNumber;
                    existModel.Inv_Address = newModel.Inv_Address;
                    existModel.Inv_Addressee = newModel.Inv_Addressee;
                    existModel.Inv_Tel = newModel.Inv_Tel;
                    existModel.Inv_TaxAmount = newModel.Inv_TaxAmount;
                    existModel.Inv_Express = newModel.Inv_Express;
                    existModel.Inv_ExpressNumber = newModel.Inv_ExpressNumber;
                    existModel.Inv_Remark = newModel.Inv_Remark;
                    existModel.Inv_InvoiceTime = DateTime.Now;
                    existModel.Inv_Status = newModel.Inv_Status;

                    work.InvoiceLogRepository.Update(existModel);
                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    return View(existModel);
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// 删除红包
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Delete(int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.InvoiceLogRepository.Get(m => m.ID == ID).FirstOrDefault<InvoiceLog>();
                work.HongBaoRepository.Delete(model);
                work.Save();
                work.Dispose();
            }

            return RedirectToAction("Index");
        }

        #endregion
    }
}