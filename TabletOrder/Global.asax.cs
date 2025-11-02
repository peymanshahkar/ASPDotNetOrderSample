using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace TabletOrder
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);



            
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Data.DataRepository _Repository = new Data.DataRepository(new LoginedData());
            StaticValues.ProductGroups = _Repository.GetALLProductGroups();
            // var result=
            StaticValues.TaxPercent=(int)_Repository.ExecuteScalar("Select top(1) Tax From TblConfig");
            StaticValues.PrinterSetupList = _Repository.GetPrinterSetup();
            StaticValues.SaleMali= _Repository.ExecuteScalar("Select top(1) saleMali From TblSaleMali Where status=1").ToString();

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

    }
}