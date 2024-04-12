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
using Pannet.DAL.Repository;

namespace Pannet.Web.Controllers
{
    public class TiXianController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 申请提现记录

        //默认列表
        [CheckPermission]
        public ActionResult Index(int page = 1)
        {
            var rst = work.Context.TiXians
                .Join(work.Context.Users, tx => tx.UserID, u => u.ID, (tx, u) => new { tx, u })
                .Select(m => new TiXianVModel
                {
                    TiXian = m.tx,
                    UserName = m.u.U_UserName
                })
                .OrderByDescending(m => m.TiXian.ID);

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
                var model = work.TiXianRepository.Get(m => m.ID == ID).FirstOrDefault<TiXian>();
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
        public ActionResult Detail(TiXian newModel, int ID = 0)
        {
            ViewBag.ID = ID;
            if (ID != 0)//新增
            {
                var existModel = work.Context.TiXians.AsNoTracking().Where(m => m.ID == ID).FirstOrDefault();
                if (existModel == null || existModel.ID == 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    existModel.TX_BankName = newModel.TX_BankName;
                    existModel.TX_BankNumber = newModel.TX_BankNumber;
                    existModel.TX_Remark = newModel.TX_Remark;
                    existModel.TX_Status = newModel.TX_Status;

                    if (newModel.TX_Status == Convert.ToInt16(DataConfig.TiXianStatusEnum.完成))
                    {
                        existModel.TX_PayTime = DateTime.Now;

                        User user = work.Context.Users.Where(m => m.ID == existModel.UserID).FirstOrDefault();
                        if (user != null)
                        {
                            //减少可提现额度
                            UserAmountHistoryService.Insert(user.ID, 0, user.U_Amount, 0, user.U_LockAmount, Convert.ToInt16(DataConfig.AmountHistoryTypeEnum.减少), "减少额度", "提现减少额度", 0, "系统", "");

                            //user.U_LockAmount = user.U_LockAmount - existModel.TX_Amount;
                            //添加金额调整记录
                            //UserAmountHistoryService.Insert(user.ID, 0, user.U_Amount, existModel.TX_Amount, user.U_LockAmount, 0, "提现完成", "提现处理完成扣款", existModel.ID, user.U_UserName);
                            //UserAmountHistoryService.Insert(user.ID, 0, user.U_Amount, newModel.TX_Amount, user.U_LockAmount, 1, "提现", "申请提现锁定", user.ID, user.U_UserName);
                            //work.UserRepository.Update(user);
                        }
                    }
                    else if (newModel.TX_Status == Convert.ToInt16(DataConfig.TiXianStatusEnum.已取消))
                    {
                        User user = work.Context.Users.AsNoTracking().Where(m => m.ID == existModel.UserID).FirstOrDefault();
                        if (user != null)
                        {
                            user.U_Amount = user.U_Amount + existModel.TX_Amount;
                            UserAmountHistoryService.Insert(user.ID, existModel.TX_Amount, user.U_Amount, 0, user.U_LockAmount, 1, "取消提现", "返还提现金额", existModel.ID, LoginedAdminModel.UserName);
                            //user.U_LockAmount = user.U_LockAmount - existModel.TX_Amount;
                            //UserAmountHistoryService.Insert(user.ID, 0, user.U_Amount, existModel.TX_Amount, user.U_LockAmount, 0, "取消提现", "取消提现解除锁定金额", existModel.ID, LoginedAdminModel.UserName);
                            work.UserRepository.Update(user);
                        }
                    }
                    work.TiXianRepository.Update(existModel);
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
        /// 删除
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Delete(int ID = 0)
        {
            if (ID != 0)
            {
                //var model = work.TiXianRepository.Get(m => m.ID == ID).FirstOrDefault<TiXian>();
                //work.TiXianRepository.Delete(model);
                //work.Save();
                //work.Dispose();
            }

            return RedirectToAction("Index");
        }

        #endregion
    }
}