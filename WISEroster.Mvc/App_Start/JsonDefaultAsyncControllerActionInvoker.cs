using System.Web.Mvc;
using System.Web.Mvc.Async;

namespace DPI.Common.Web.UI
{

    /// <summary>
    /// Change the MVC default ActionResult from ContentResult to JsonResult
    /// </summary>
    public class JsonDefaultAsyncControllerActionInvoker : AsyncControllerActionInvoker
    {
        protected override ActionResult CreateActionResult(ControllerContext controllerContext, ActionDescriptor actionDescriptor,
            object actionReturnValue)
        {
            if (actionReturnValue is ActionResult actionResult) return actionResult;

            return new JsonResult
            {
                Data = actionReturnValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}