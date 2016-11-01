using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Identity;
using cnodejs.BLL;
using cnodejs.Model;

public class HomeBaseController : Controller {
	public ISession Session { get { return HttpContext.Session; } }
	public HttpRequest Req { get { return Request; } }
	public HttpResponse Res { get { return Response; } }

	public UsersInfo LoginUser { get; private set; }
	public override void OnActionExecuting(ActionExecutingContext context) {
		string username = Session.GetString("login.username");
		//if (string.IsNullOrEmpty(username))
		//	Session.SetString("login.username", username = "2881099");
		if (!string.IsNullOrEmpty(username))
			LoginUser = Users.GetItemByUsername(username);

		if ((context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttribute<需要登陆Attribute>() != null && LoginUser == null)
			context.Result = new RedirectResult("/signin");

		ViewBag.user = LoginUser;
		base.OnActionExecuting(context);
	}
	public override void OnActionExecuted(ActionExecutedContext context) {
		if (context.Exception != null) {
			// 错误拦截，在这里记录日志
			//this.Json(new APIReturn(-1, context.Exception.Message)).ExecuteResultAsync(context).Wait();
			//context.Exception = null;
		}
		base.OnActionExecuted(context);
	}
}

public class 需要登陆Attribute : Attribute {
}