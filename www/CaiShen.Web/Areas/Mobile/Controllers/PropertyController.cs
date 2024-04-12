using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Models;
using Pannet.DAL;
using Pannet.Utility;
using PagedList;
using Pannet.DAL.Repository;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class PropertyController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 获取属性名称+属性值 JSON

        /// <summary>
        /// 获取属性 Json
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPropertyJson(int typeid)
        {
            List<Property> listArea = work.PropertyRepository.Get(m => m.GoodsTypeID == typeid).ToList();

            return Json(listArea, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取属性值 Json
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPropertyValueJson(int propertyid)
        {
            List<PropertyValue> listArea = work.PropertyValueRepository.Get(m => m.PropertyID == propertyid).ToList();

            return Json(listArea, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}