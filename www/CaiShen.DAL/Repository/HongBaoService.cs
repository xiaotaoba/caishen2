using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class HongBaoService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static HongBao GetModel(int ID)
        {
            var list = work.HongBaoRepository.Get(m => m.ID == ID, null).ToList<HongBao>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new HongBao();
        }

        #endregion

        #region 发放红包

        /// <summary>
        /// 发放红包
        /// </summary>
        /// <param name="type">红包类型</param>
        /// <param name="UserID">用户ID</param>
        /// <returns>随机的红包金额</returns>
        public static decimal SendHongBao(DataConfig.HongBaoTypeEnum type, int UserID, string mobile = "")
        {
            int status_begin = Convert.ToInt32(DataConfig.HongBaoStatusEnum.已开始);
            int type_value = Convert.ToInt32(DataConfig.HongBaoTypeEnum.注册红包);
            if (type == DataConfig.HongBaoTypeEnum.注册红包)
            {
                var rst = work.Context.HongBaos.Where(m => m.HB_IsDelete == 0 && m.HB_Status == status_begin && m.HB_BeginTime < DateTime.Now && m.HB_EndTime > DateTime.Now && m.HB_Type == type_value);
                if (rst != null && rst.Count() > 0)
                {
                    HongBao model = rst.FirstOrDefault();
                    if (model.HB_RestCount > 0 && model.HB_RestAmount > 0)
                    {
                        //随机红包金额
                        decimal hb_amount = Assistant.GetRandomNumber(Convert.ToInt32(model.HB_MinAmount), Convert.ToInt32(model.HB_MaxAmount) + 1);

                        //保证红包金额不大于剩余金额，不小于最小红包金额
                        if (model.HB_RestAmount < hb_amount)
                        {
                            hb_amount = model.HB_RestAmount;
                        }
                        if (hb_amount < model.HB_MinAmount)
                        {
                            hb_amount = model.HB_MinAmount;
                        }

                        //新增发放记录
                        UserHongBao newModel = new UserHongBao();
                        newModel.HongBaoID = model.ID;
                        newModel.UBH_Amount = hb_amount;
                        newModel.UBH_Title = model.HB_Name;
                        newModel.UHB_ReceiveTime = DateTime.Now;
                        newModel.UHB_ExpirationTime = DateTime.Now.AddDays(model.HB_ValidDate);
                        newModel.UHB_IsDelete = 0;
                        newModel.UHB_Status = 0;
                        newModel.UserID = UserID;

                        work.UserHongBaoRepository.Insert(newModel);
                        work.Save();

                        //修改红包信息
                        model.HB_RestAmount = model.HB_RestAmount - hb_amount;//有可能为负数，保证红包为最小可用
                        model.HB_RestCount = model.HB_RestCount - 1;
                        if (model.HB_RestAmount <= 0 || model.HB_RestCount <= 0)
                        {
                            model.HB_Status = Convert.ToInt32(DataConfig.HongBaoStatusEnum.已领完);
                        }

                        work.HongBaoRepository.Update(model);
                        work.Save();

                        if (!string.IsNullOrEmpty(mobile))
                        {
                            SmsService.SendHongbaoSms(mobile, hb_amount.ToString(), DateTime.Now.AddDays(model.HB_ValidDate).ToString(), "新用户注册");
                        }
                    }
                }
            }
            return 0;
        }

        #endregion

    }
}
