﻿using SSA_FestivalApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SSA_FestivalApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }


        //protected void Application_Error(object sender, EventArgs e)
        //{

        //    var ex = Server.GetLastError().GetBaseException();

        //    Server.ClearError();
        //    var routeData = new RouteData();
        //    routeData.Values.Add("controller", "Error");
        //    routeData.Values.Add("action", "Index");

        //    if (ex.GetType() == typeof(HttpException))
        //    {
        //        var httpException = (HttpException)ex;
        //        var code = httpException.GetHttpCode();
        //        routeData.Values.Add("status", code);
        //    }
        //    else
        //    {
        //        routeData.Values.Add("status", 500);
        //    }

        //    routeData.Values.Add("error", ex);

        //    IController errorController = new ErrorController();
        //    errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        //}

        //protected void Application_EndRequest(object sender, EventArgs e)
        //{
        //    if (Context.Response.StatusCode == 401)
        //    { 
        //        throw new HttpException(401, "You are not authorised");
        //    }
        //}
    }
}