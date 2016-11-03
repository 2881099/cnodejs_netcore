using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using cnodejs.BLL;
using cnodejs.Model;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Admin.Controllers.auth
{
	[Route("/oauth/[controller]")]
	public class GithubController : HomeBaseController
    {
		[HttpGet]
		public IActionResult 首页([FromServices] IConfigurationRoot cfg) {
			string state = Guid.NewGuid().ToString();
			Session.SetString("github_state", state);
			var client_id = WebUtility.UrlEncode(cfg["oauth:github:client_id"]);
			var redirect_uri = WebUtility.UrlEncode(cfg["oauth:github:redirect_uri"]);
			return Redirect($"https://github.com/login/oauth/authorize?client_id={client_id}&redirect_uri={redirect_uri}&state={state}&allow_signup=false");
		}
		[HttpGet("callback")]
		public IActionResult 回调([FromServices] IConfigurationRoot cfg, [FromQuery] string code,
			[FromQuery] string error, [FromQuery] string error_description, [FromQuery] string error_uri, [FromQuery] string state) {
			if (state != Session.GetString("github_state")) throw new Exception("state 值与 session 不一致");
			HttpClientHandler clientHandler = new HttpClientHandler { ClientCertificateOptions = ClientCertificateOption.Automatic };
			clientHandler.ServerCertificateCustomValidationCallback = (a, b, c, d) => true;
			HttpClient client = new HttpClient(clientHandler);
			var requestJson = JsonConvert.SerializeObject(new {
				client_id = cfg["oauth:github:client_id"],
				client_secret = cfg["oauth:github:client_secret"],
				code = code,
				redirect_uri = cfg["oauth:github:redirect_uri"],
				state = state
			});
			StringContent content = new StringContent(requestJson);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			var res = client.PostAsync("https://github.com/login/oauth/access_token", content).Result;
			string rt = res.Content.ReadAsStringAsync().Result;
			//access_token=e72e16c7e42f292c6912e7710c838347ae178b4a&scope=user%2Cgist&token_type=bearer
			Dictionary<string, string> rtnv = new Dictionary<string, string>();
			foreach(string tmp1 in rt.Replace("&amp;", "&").Split('&')) {
				string[] tmp2 = tmp1.Split(new char[] { '=' }, 2);
				if (tmp2.Length != 2) continue;
				string k = WebUtility.UrlDecode(tmp2[0].Trim());
				string v = WebUtility.UrlDecode(tmp2[1].Trim());
				if (rtnv.ContainsKey(k)) rtnv[k] = v;
				else rtnv.Add(k, v);
			}
			string access_token;
			rtnv.TryGetValue("access_token", out access_token);

			return Redirect($"https://api.github.com/user?access_token?{access_token}");
		}
		bool serverCertificateCustomValidationCallback(HttpRequestMessage sender, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors errors) {
			return true;
		}
	}
}
