using Atomus.Database;
using Atomus.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Atomus.Page.Menu.Controllers
{
    internal static class DefaultMenuControllerController
    {
        internal static async Task<IResponse> SearchAsync(this ICore core, decimal START_MENU_ID, decimal ONLY_PARENT_MENU_ID, decimal ASSEMBLY_ID)
        {
            IServiceDataSet serviceDataSet;

            serviceDataSet = new ServiceDataSet
            {
                ServiceName = core.GetAttribute("ServiceName"),
                TransactionScope = false
            };
            serviceDataSet["LoadMenu"].ConnectionName = core.GetAttribute("DatabaseName");
            serviceDataSet["LoadMenu"].CommandText = core.GetAttribute("ProcedureMenu");
            serviceDataSet["LoadMenu"].AddParameter("@START_MENU_ID", DbType.Decimal, 18);
            serviceDataSet["LoadMenu"].AddParameter("@ONLY_PARENT_MENU_ID", DbType.Decimal, 18);
            serviceDataSet["LoadMenu"].AddParameter("@ASSEMBLY_ID", DbType.Decimal, 18);
            serviceDataSet["LoadMenu"].AddParameter("@USER_ID", DbType.Decimal, 18);

            serviceDataSet["LoadMenu"].NewRow();
            serviceDataSet["LoadMenu"].SetValue("@START_MENU_ID", START_MENU_ID.MinusToDBNullValue());
            serviceDataSet["LoadMenu"].SetValue("@ONLY_PARENT_MENU_ID", ONLY_PARENT_MENU_ID.MinusToDBNullValue());
            serviceDataSet["LoadMenu"].SetValue("@ASSEMBLY_ID", ASSEMBLY_ID.MinusToDBNullValue());
            serviceDataSet["LoadMenu"].SetValue("@USER_ID", Config.Client.GetAttribute("Account.USER_ID"));

            return await core.ServiceRequestAsync(serviceDataSet);
        }

        internal static async Task<IResponse> SearchInfoAsync(this ICore core)
        {
            IServiceDataSet serviceDataSet;

            serviceDataSet = new ServiceDataSet
            {
                ServiceName = core.GetAttribute("ServiceName"),
                TransactionScope = false
            };
            serviceDataSet["LoadMenu"].ConnectionName = core.GetAttribute("DatabaseName");
            serviceDataSet["LoadMenu"].CommandText = core.GetAttribute("ProcedureInfo");
            serviceDataSet["LoadMenu"].AddParameter("@USER_ID", DbType.Decimal, 18);

            serviceDataSet["LoadMenu"].NewRow();
            serviceDataSet["LoadMenu"].SetValue("@USER_ID", Config.Client.GetAttribute("Account.USER_ID"));

            return await core.ServiceRequestAsync(serviceDataSet);
        }
    }
}
