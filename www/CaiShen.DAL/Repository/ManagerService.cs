using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class ManagerService
    {
        private static UnitOfWork work = new UnitOfWork();

        /// <summary>
        /// 获取登录管理员实体
        /// </summary>
        /// <returns></returns>
        public static Manager GetLoginModel()
        {
            string userName = CookieHelper.GetValue(ConfigHelper.CookieAdminName);
            if (!string.IsNullOrEmpty(userName))
            {
                var user = work.Context.Managers.AsNoTracking().Where(u => u.UserName == userName).ToList<Manager>();
                if (user.Count() > 0)
                {
                    return user[0];
                }
            }
            return null;
        }
        /// <summary>
        /// 判断是否有权限栏目权限
        /// </summary>
        /// <param name="limit_num"></param>
        /// <param name="limits"></param>
        /// <returns></returns>
        public static bool HasLimit(int limit_num, string limits)
        {
            if (string.IsNullOrEmpty(limits))
            {
                return false;
            }
            if (limits == "all")
            {
                return true;
            }
            List<string> limit_list = new List<string>(limits.Split(','));
            if (limit_list != null && limit_list.Contains(limit_num.ToString()))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断是否有权限栏目权限
        /// </summary>
        /// <param name="limit_num"></param>
        /// <param name="manager"></param>
        /// <returns></returns>
        public static bool HasLimit(int limit_num, Manager manager)
        {
            return HasLimit(limit_num, manager.ManagerWithGroup.First().ManagerGroup.Limits);
        }
        /// <summary>
        /// 判断是否有权限栏目权限
        /// </summary>
        /// <param name="actionName">controler+action名称，如user/add,用户编辑user/add/0</param>
        /// <param name="limits"></param>
        /// <returns></returns>
        public static bool HasLimit(string actionName, Manager manager)
        {
            int limit_num = 1;
            actionName = actionName.ToLower();
            NameValueModel valueModel = DataConfig.RoleLimits.Find(m => m.Name.ToLower() == actionName);
            if (valueModel != null)
            {
                limit_num = Convert.ToInt16(valueModel.Value);
            }
            return HasLimit(limit_num, manager.ManagerWithGroup.First().ManagerGroup.Limits);
        }
    }
}
