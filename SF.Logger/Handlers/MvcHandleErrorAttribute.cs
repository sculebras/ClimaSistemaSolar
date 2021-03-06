﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SF.Logger.Handlers
{
    //2014-11-20 01:50:48 sculebras: 
    /// <summary>
    /// Attribute Class designed to override default MVC error handling.
    /// To use this class in Mvc projects as General Error Handling 
    /// you must edit \App_Start\FilterConfig.cs
    /// and replace HandleErrorAttribute with MvcHandleErrorAttribute.
    /// Remember to put <code><customErrors mode="On" /> in <system.web> in web.config</code>
    /// </summary>
    public class MvcHandleErrorAttribute : HandleErrorAttribute
    {


        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            // if the request is AJAX return JSON else view.
            if (IsAjax(filterContext))
            {
                //Because its a exception raised after ajax invocation
                //Lets return Json
                filterContext.Result = new JsonResult()
                {
                    Data = filterContext.Exception.Message,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
            }
            else
            {
                //Normal Exception
                //So let it handle by its default ways.
                base.OnException(filterContext);

            }

            // Write error logging code here if you wish.
            Logger.TraceError(filterContext.Exception);

            //if want to get different of the request
            //var currentController = (string)filterContext.RouteData.Values["controller"];
            //var currentActionName = (string)filterContext.RouteData.Values["action"];
        }

        /// <summary>
        /// Defines if the exception was raised from a Ajax Request
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private bool IsAjax(ExceptionContext filterContext)
        {
            return filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }//End class
}//End Namespace				
				
