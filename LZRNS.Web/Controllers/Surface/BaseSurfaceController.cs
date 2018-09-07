using System;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using LZRNS.Models.Extensions;

namespace LZRNS.Web.Controllers.Surface
{
	public class BaseSurfaceController : SurfaceController
	{
		protected ActionResult RenderActionResult<T>(T model, Func<ActionResult> callback) where T : class
		{
			if (model == default(T)) return new EmptyResult();
			return callback.Invoke();
		}

		protected ActionResult RenderActionResultBasedOnName<T>(T model) where T : class
		{
			if (model == default(T)) return new EmptyResult();
			string viewName = model.GetType().Name.RemoveModelSuffix();
			return PartialView(viewName, model);
		}

		protected override void OnException(ExceptionContext filterContext)
		{
			if (filterContext.ExceptionHandled)
			{
				return;
			}

			//Log the exception.
			Logger.Error(GetType(), $"An unhandled exception occurred on partial controller action '{filterContext.RouteData.Values["action"]}'!!!", filterContext.Exception);

			//Return empty result instead of partial view.
			filterContext.Result = new EmptyResult();
			filterContext.ExceptionHandled = true;
		}
	}
}