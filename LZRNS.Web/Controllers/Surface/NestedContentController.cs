using LZRNS.Models.DocumentTypes.Nodes.NestedContent;
using LZRNS.Models.Extensions;
using System;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.Surface
{
    public class NestedContentController: SurfaceController
    {
        [ChildActionOnly]
        public ActionResult Index(NestedContentBaseModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            string viewName = model.GetType().Name.RemoveModelSuffix();
            return PartialView(viewName, model);
        }

        public ActionResult GetTableWidget(TableWidgetModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            string viewName = model.GetType().Name.RemoveModelSuffix();
            return PartialView(viewName, model);
        }
    }
}