using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class BrowseRecordService
    {
        private static UnitOfWork work = new UnitOfWork();

        public static void Add(int userid, int shopid, string url, int itemid = 0, int type = 1)
        {
            if (userid != 0)
            {
                //BrowseRecord existModel = work.BrowseRecordRepository.Get(m => m.BR_ItemID == itemid && m.UserID == userid && m.BR_ItemType == type).FirstOrDefault();
                //if (existModel != null)
                //{
                //    existModel.BR_CreateTime = DateTime.Now;
                //    work.BrowseRecordRepository.Update(existModel);
                //}
                //else
                //{
                    BrowseRecord newModel = new BrowseRecord();
                    newModel.ShopID = shopid;
                    newModel.UserID = userid;
                    newModel.BR_ItemID = itemid;
                    newModel.BR_ItemType = type;
                    newModel.BR_IP = IPHelper.GetIP();
                    newModel.BR_CreateTime = DateTime.Now;
                    newModel.BR_URL = url;

                    work.BrowseRecordRepository.Insert(newModel);
                //}
                work.Save();
            }
            //work.Dispose();不能释放
        }
        public static void Add(User loginUser, UserShop currentShop, string url, int itemid = 0, int type = 1)
        {
            if (loginUser != null)
            {
                Add(loginUser.ID, currentShop.ID, url, itemid, type);
            }
        }
    }
}
