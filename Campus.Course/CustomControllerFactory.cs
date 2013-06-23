using Autofac;
using Autofac.Configuration;
using Campus.Course.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace Campus.Course
{
    public class CustomControllerFactory : DefaultControllerFactory
    {
        private static IContainer container = null;
        private static readonly object lockobject = new object();

        public CustomControllerFactory()
        {
            if (container == null)
            {
                lock (lockobject)
                {
                    if (container == null)
                    {
                        ContainerBuilder builder = new ContainerBuilder();
                        //register controller
                        builder.RegisterType<LoginController>();
                        
                        builder.RegisterType<HomeController>();
                        builder.RegisterType<TimeSheetController>();
                        builder.RegisterModule(new ConfigurationSettingsReader("autofac"));
                        container = builder.Build();
                    }
                }
            }

        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            //skip /favicon.ico which will cause exception
            if (requestContext.HttpContext.Request.Path == "/favicon.ico")
                return null;

            if (controllerType == null)
            {
                throw new HttpException(404,
                    String.Format(
                        CultureInfo.CurrentUICulture,
                        "The controller for path '{0}' was not found or does not implement IController.",
                        requestContext.HttpContext.Request.Path));
            }
            
            if (!typeof(IController).IsAssignableFrom(controllerType))
            {
                throw new ArgumentException(
                    String.Format(
                        CultureInfo.CurrentUICulture,
                        "The controller type '{0}' must implement IController.",
                        controllerType),
                    "controllerType");
            }
            try
            {
               // Activator.CreateInstance(controllerType);
                return (IController)container.Resolve(controllerType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentUICulture,
                        "An error occurred when trying to create a controller of type '{0}'. Make sure that the controller has a parameterless public constructor.",
                        controllerType),
                    ex);
            }
        }
       
    }

    internal class CustomControllerActionInvoker : ControllerActionInvoker
    {
        protected override ActionResult InvokeActionMethod(ControllerContext controllerContext, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters)
        {
            if (controllerContext.Controller is BaseController)
            {
                BaseController controller = (BaseController)controllerContext.Controller;
                //controller.User;
                controller.ViewBag.aa = "11";
            }
            return base.InvokeActionMethod(controllerContext, actionDescriptor, parameters);
        }
    }
}