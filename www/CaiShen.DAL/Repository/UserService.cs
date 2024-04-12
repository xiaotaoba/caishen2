using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;
using Maticsoft.DBUtility;
using System.Data.SqlClient;
using System.Data;

namespace Pannet.DAL.Repository
{
    public class UserService
    {
        private static UnitOfWork work = new UnitOfWork();
        private static string fanConfigPath = AppDomain.CurrentDomain.BaseDirectory + "/logs/fantime.txt";

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static User GetModel(int ID)
        {
            //var model = work.Context.Users.AsNoTracking().Where(u => u.ID == ID).FirstOrDefault<User>();
            //if (model != null)
            //{
            //    return model;
            //}
            //return null;
            var rst = work.Context.Users.AsNoTracking().Where(u => u.ID == ID).ToList();
            if (rst != null && rst.Count > 0)
            {
                return rst[0];
            }
            return null;
        }
        /// <summary>
        /// 获取已登录用户实体
        /// </summary>
        /// <returns></returns>
        public static User GetLoginedModel()
        {
            string userName = CookieHelper.GetValue(ConfigHelper.CookieUserName);
            if (!string.IsNullOrEmpty(userName))
            {
                //object cacheUser = GetCacheUser(userName);
                //if (cacheUser != null)
                //{
                //    return cacheUser as User;
                //}
                //else
                //{
                    var user = work.Context.Users.AsNoTracking().Where(u => u.U_UserName == userName).FirstOrDefault<User>();
                    //if (user == null)
                    //{
                    //    return null;
                    //}
                    //SetCacheUser(userName, user);

                    return user;
                //}
            }
            return null;
        }

        /// <summary>
        /// 设置用户缓存
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userName">user</param>
        /// <returns></returns>
        public static void SetCacheUser(string userName, User user)
        {
            Utility.DataCache.SetCache("user_" + userName, user);
        }

        /// <summary>
        /// 获取用户缓存
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static object GetCacheUser(string userName)
        {
            return Utility.DataCache.GetCache("user_" + userName);
        }

        #endregion

        #region 更新余额

        /// <summary>
        /// 更新余额
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="amount">变动的金额，正加，负减</param>
        /// <returns></returns>
        public static bool UpdateAmount(int UserID, decimal amount)
        {
            User user = GetModel(UserID);
            if (user != null)
            {
                user.U_Amount = user.U_Amount + amount;
                work.UserRepository.Update(user);
                work.Save();
            }
            //更新用户缓存
            SetCacheUser(user.U_UserName, user);
            return true;
        }
        /// <summary>
        /// 更新余额
        /// </summary>
        /// <param name="User">用户实体</param>
        /// <param name="amount">变动的金额，正加，负减</param>
        /// <returns></returns>
        public static bool UpdateAmount(User user, decimal amount)
        {
            if (user != null)
            {
                user.U_Amount = user.U_Amount + amount;
                work.UserRepository.Update(user);
                work.Save();
            }
            return true;
        }

        /// <summary>
        /// 更新积分
        /// </summary>
        /// <param name="User">用户实体</param>
        /// <param name="score">变动的积分，正加，负减</param>
        /// <returns></returns>
        public static bool UpdateScore(User user, int score)
        {
            if (user != null)
            {
                user.U_Score = user.U_Score + score;
                work.UserRepository.Update(user);
                work.Save();
            }
            return true;
        }
        /// <summary>
        /// 更新积分
        /// </summary>
        /// <param name="UserID">用户实体</param>
        /// <param name="score">变动的积分，正加，负减</param>
        /// <returns></returns>
        public static bool UpdateScore(int UserID, int score)
        {
            User user = GetModel(UserID);
            if (user != null)
            {
                user.U_Score = user.U_Score + score;
                work.UserRepository.Update(user);
                work.Save();
            }
            return true;
        }
        #endregion

        #region 获取当前用户折扣

        /// <summary>
        /// 获取当前用户折扣，根据用户等级及角色，取最大优惠折扣返回
        /// </summary>
        /// <returns></returns>
        public static double GetUserPercent()
        {
            User user = GetLoginedModel();
            if (user != null)
            {
                //用户等级
                UserLevel userlevel = work.UserLevelRepository.GetByID(user.UserLevelID);
                //用户角色
                UserRole userRole = work.UserRoleRepository.GetByID(user.UserRoleID);
                if (userlevel.Level_Discount_Percent == 0)
                {
                    userlevel.Level_Discount_Percent = 1;
                }
                if (userRole.Role_Discount_Percent == 0)
                {
                    userRole.Role_Discount_Percent = 1;
                }
                return userlevel.Level_Discount_Percent > userRole.Role_Discount_Percent ? userRole.Role_Discount_Percent : userlevel.Level_Discount_Percent;

            }
            return 1;
        }

        #endregion

        #region 获取头像

        /// <summary>
        /// 获取头像
        /// </summary>
        /// <param name="u_thumbnail">数据库保存头像</param>
        /// <returns></returns>
        public static string GetThumbnail(string u_thumbnail)
        {
            if (string.IsNullOrEmpty(u_thumbnail))
            {
                u_thumbnail = "/Areas/Mobile/images/default_logo.jpg";// "/Content/images/head_default.jpg";
            }
            return u_thumbnail;
        }


        #endregion

        #region 获取隐藏的昵称

        /// <summary>
        /// 获取隐藏的昵称
        /// </summary>
        /// <param name="isHidden">1是</param>
        /// <returns></returns>
        public static string GetNickName(string nickName, int isHidden = 0)
        {
            if (isHidden == 0)
            {
                return nickName;
            }
            else
            {
                return string.Format("{0}***{1}", nickName.Substring(0, 1), nickName.Substring(nickName.Length - 2, 1));
            }
        }

        #endregion


        #region 定时返

        /// <summary>
        /// 定时返
        /// </summary>
        /// <returns>1今天是否已返,0返利成功,2无需返利记录</returns>
        public static int FanJiFen()
        {
            if (IsFanToday())
            {
                Log.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":今天已返利-返回(文件判断)！", "定时任务", DateTime.Now.ToString("yyyyMMdd"));
                return 1;
            }

            //今天是否已返利
            string sql1 = string.Format("select count(*) from Ks_JiFen where jf_time > '{0}'  and JF_Category = '返还' ", DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime today =Convert.ToDateTime( DateTime.Now.ToString("yyyy-MM-dd"));
            int count = work.Context.UserAmountHistorys.Where(m => m.Category == "返还" && m.Time > today).Count();
            if (count > 0)
            {
                Log.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":今天已返利-返回！", "定时任务", DateTime.Now.ToString("yyyyMMdd"));
                return 1;
            }

            SqlParameter parameter = new SqlParameter("@mydate", SqlDbType.DateTime);
            parameter.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //现金返利
            DbHelperSQL.MyRunProcedure("[MyFanliProc]", parameter);//[sq_yatoushop].[MyFanliProc]

            Log.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":积分返利执行完毕！", "定时任务", DateTime.Now.ToString("yyyyMMdd"));
            UpdateFanConfig();

            return 0;
        }

        /// <summary>
        /// 更新返利时间文件
        /// </summary>
        protected static bool UpdateFanConfig()
        {
            if (!FileHelper.IsExistFile(fanConfigPath))
            {
                FileHelper.CreateFile(fanConfigPath);
            }
            string fantime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            FileHelper.WriteText(fanConfigPath, "{\"time\":\"" + fantime + "\"}");
            return true;
        }
        /// <summary>
        /// 检测返利记录文件是否存在，不存在创建，存在则返回今天是否返利
        /// </summary>
        /// <returns></returns>
        protected static bool IsFanToday()
        {
            if (!FileHelper.IsExistFile(fanConfigPath))
            {
                FileHelper.CreateFile(fanConfigPath);
            }
            string jsonStr = FileHelper.FileToString(fanConfigPath);
            if (!string.IsNullOrEmpty(jsonStr))
            {
                FanTime ft =JsonHelper.DeserializeJsonToObject<FanTime>(jsonStr);
                if (ft != null)
                {
                    string mytime = ft.time;
                    if (Convert.ToDateTime(mytime) > Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))//时间大于今天凌晨，说明今天已返利
                    {
                        return true;
                    }
                }
            }
            else
            {
                UpdateFanConfig();
            }
            return false;
        }

        #endregion
    }
    /// <summary>
    /// 定时返利时间
    /// </summary>
    public class FanTime
    {
        public string time { get; set; }
    }
}
