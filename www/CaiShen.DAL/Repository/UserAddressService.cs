using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class UserAddressService
    {
        private static UnitOfWork work = new UnitOfWork();

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static UserAddress GetModel(int ID)
        {
            var list = work.UserAddressRepository.Get(m => m.ID == ID, null).ToList<UserAddress>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new UserAddress();
        }

        /// <summary>
        /// 设为默认，其他取消默认
        /// </summary>
        /// <returns></returns>
        public static bool SetDefault(int UserID, int ID)
        {
            var list = work.Context.UserAddresses.Where(m => m.UserID == UserID && m.Is_Default == 1 && m.ID != ID).ToList<UserAddress>();
            foreach (var item in list)
            {
                item.Is_Default = 0;
                work.UserAddressRepository.Update(item);
                work.Save();
            }
            return true;
        }

        /// <summary>
        /// 删除用户地址信息
        /// </summary>
        /// <param name="aid"></param>
        public static void Delete(int aid)
        {
            if (aid != 0)
            {
                var model = work.UserAddressRepository.Get(m => m.ID == aid).FirstOrDefault<UserAddress>();
                work.UserAddressRepository.Delete(model);
                work.Save();
                //work.Dispose();
            }
        }

    }
}
