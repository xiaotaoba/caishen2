using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class ShippingCompanyService
    {
        private static UnitOfWork work = new UnitOfWork();

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static ShippingCompany GetModel(int ID)
        {
            var model = work.ShippingCompanyRepository.Get(m => m.ID == ID, null).FirstOrDefault();
            if (model != null)
            {
                return model;
            }
            return new ShippingCompany();
        }

    }
}
