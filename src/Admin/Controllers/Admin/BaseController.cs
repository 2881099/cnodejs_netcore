using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using cnodejs.BLL;
using cnodejs.Model;

public class BaseController : Controller {
	public ISession Session { get { return HttpContext.Session; } }
	public HttpRequest Req { get { return Request; } }
	public HttpResponse Res { get { return Response; } }

	//public SysuserInfo LoginUser { get; private set; }
	public override void OnActionExecuting(ActionExecutingContext context) {
		//byte[] tryvalue;
		//if (context.HttpContext.Session.TryGetValue("login.username", out tryvalue)) {
		//	string username = Encoding.UTF8.GetString(tryvalue);
		//	this.LoginUser = Sysuser.GetItemByUsername(username);
		//}
		//if (this.LoginUser == null) {
		//	context.Result = new JsonResult(new APIReturn(-12, "未登陆或者没有权限"));
		//}
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

	//public bool sysrole_check(string url) {{
	//	url = url.ToLower();
	//	//Response.Write(url + ""<br>"");
	//	if (url == ""/"" || url.IndexOf(""/default.aspx"") == 0) return true;
	//	foreach(var role in this.LoginUser.Obj_sysroles) {{
	//		//Response.Write(role.ToString());
	//		foreach(var dir in role.Obj_sysdirs) {{
	//			//Response.Write(""-----------------"" + dir.ToString() + ""<br>"");
	//			string tmp = dir.Url;
	//			if (tmp.EndsWith(""/"")) tmp += ""default.aspx"";
	//			if (url.IndexOf(tmp) == 0) return true;
	//		}}
	//	}}
	//	return false;
	//}}
}

public class APIReturn {
	public int Code { get; protected set; }
	public string Message { get; protected set; }
	public Hashtable Data { get; protected set; }
	public bool Success { get { return this.Code == 0; } }

	public APIReturn() {
		this.Data = new Hashtable();
	}
	public APIReturn(int code) : this() { this.Code = code; }
	public APIReturn(string message) : this() { this.Message = message; }
	public APIReturn(int code, string message, params object[] data) : this() {
		this.Code = code;
		this.Message = message;
		if (data != null) {
			for (int a = 0; a < data.Length; a += 2) {
				if (data[a] == null) continue;
				this.Data[data[a]] = a + 1 < data.Length ? data[a + 1] : null;
			}
		}
	}
}
