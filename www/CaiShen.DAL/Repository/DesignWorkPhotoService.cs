using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class DesignWorkPhotoService
    {
        private static UnitOfWork work = new UnitOfWork();

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static DesignWorkPhoto GetModel(int ID)
        {
            var list = work.DesignWorkPhotoRepository.Get(m => m.ID == ID, null).ToList<DesignWorkPhoto>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new DesignWorkPhoto();
        }

        /// <summary>
        /// 取消当前产品其他主图，并设置当前图片为主图
        /// </summary>
        /// <returns></returns>
        public static bool SetFirst(int DesignWorkID, int ID)
        {
            var list = work.DesignWorkPhotoRepository.Get(m => m.DesignWorkID == DesignWorkID).ToList<DesignWorkPhoto>();
            foreach (var item in list)
            {
                if (item.ID == ID)
                {
                    item.DWP_IsFirst = 1;
                }
                else
                {
                    item.DWP_IsFirst = 0;
                }
                work.DesignWorkPhotoRepository.Update(item);
                work.Save();
            }
            return true;
        }
        /// <summary>
        /// 获取当前主图
        /// </summary>
        /// <param name="DesignWorkID"></param>
        /// <returns></returns>
        public static string GetFirstPhoto(int DesignWorkID)
        {
            DesignWorkPhoto photoFirst = work.DesignWorkPhotoRepository.Get(m => m.DesignWorkID == DesignWorkID && m.DWP_IsFirst==1).FirstOrDefault();
            if (photoFirst != null)
            {
                return photoFirst.DWP_Image;
            }
            
            return "";
        }
    }
}
