using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;

namespace Pannet.DAL
{
    public class PannetDBInitializer : DropCreateDatabaseIfModelChanges<PannetContext>
    {
        protected override void Seed(PannetContext context)
        {
            //var manager = new List<Manager>(){
            //    new Manager{UserName="admin", Password="123456", Phone="15166668888" },
            //    new Manager{UserName="pannet", Password="123456", Phone="15888888888" }
            //};
            //manager.ForEach(m => context.Managers.Add(m));
            //context.SaveChanges();
            //var managerGroup = new List<ManagerGroup>(){
            //    new ManagerGroup{ Name="超级管理员",Desc="1" },
            //    new ManagerGroup{ Name="业务员",Desc="2" },
            //    new ManagerGroup{ Name="财务",Desc="3" },
            //    new ManagerGroup{ Name="库管",Desc="4" },
            //    new ManagerGroup{ Name="城市合伙人",Desc="5" }
            //};
            //managerGroup.ForEach(m => context.ManagerGroups.Add(m));
            //context.SaveChanges();
            //base.Seed(context);
        }
    }
}
