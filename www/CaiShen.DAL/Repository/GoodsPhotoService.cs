using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class GoodsPhotoService
    {
        private static UnitOfWork work = new UnitOfWork();

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static GoodsPhoto GetModel(int ID)
        {
            var list = work.GoodsPhotoRepository.Get(m => m.ID == ID, null).ToList<GoodsPhoto>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new GoodsPhoto();
        }

        /// <summary>
        /// 取消当前产品其他主图，并设置当前图片为主图
        /// </summary>
        /// <returns></returns>
        public static bool SetFirst(int GoodsID, int ID)
        {
            var list = work.GoodsPhotoRepository.Get(m => m.GoodsID == GoodsID).ToList<GoodsPhoto>();
            foreach (var item in list)
            {
                if (item.ID == ID)
                {
                    item.GP_IsFirst = 1;
                }
                else
                {
                    item.GP_IsFirst = 0;
                }
                work.GoodsPhotoRepository.Update(item);
                work.Save();
            }
            return true;
        }
        /// <summary>
        /// 获取当前主图
        /// </summary>
        /// <param name="GoodsID"></param>
        /// <returns></returns>
        public static string GetFirstPhoto(int GoodsID)
        {
            GoodsPhoto photoFirst = work.GoodsPhotoRepository.Get(m => m.GoodsID == GoodsID && m.GP_IsFirst==1).FirstOrDefault();
            if (photoFirst != null)
            {
                return photoFirst.GP_Image;
            }
            
            return "";
        }
    }
}
