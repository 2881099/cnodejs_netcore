using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using cnodejs.BLL;
using cnodejs.Model;

namespace cnodejs.Admin.Controllers {
	[Route("api/[controller]")]
	public class UsersController : BaseController {
		private readonly ILogger<UsersController> _logger;
		public UsersController(ILogger<UsersController> logger) { _logger = logger; }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint[] Roles_id, [FromQuery] ulong[] Topics_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Users.Select
				.Where(!string.IsNullOrEmpty(key), "a.email like {0} or a.github like {0} or a.location like {0} or a.password like {0} or a.sign like {0} or a.username like {0} or a.website like {0} or a.weibo like {0}", string.Concat("%", key, "%"));
			if (Roles_id.Length > 0) select.WhereRoles_id(Roles_id);
			if (Topics_id.Length > 0) select.WhereTopics_id(Topics_id);
			int count;
			var items = select.Count(out count).Skip(skip).Limit(limit).ToList();
			return new APIReturn(0, "成功", "items", items.ToBson(), "count", count);
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(ulong? Id) {
			UsersInfo item = Users.GetItem(Id);
			if (item == null) return new APIReturn(98, "记录不存在，或者没有权限");
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] string Email, [FromForm] string Github, [FromForm] string Location, [FromForm] string Password, [FromForm] uint? Point, [FromForm] string Sign, [FromForm] string Username, [FromForm] string Website, [FromForm] string Weibo, [FromForm] uint[] mn_Roles, [FromForm] ulong[] mn_Topics) {
			UsersInfo item = new UsersInfo();
			item.Create_time = DateTime.Now;
			item.Email = Email;
			item.Github = Github;
			item.Location = Location;
			item.Password = Password;
			item.Point = Point;
			item.Sign = Sign;
			item.Username = Username;
			item.Website = Website;
			item.Weibo = Weibo;
			item = Users.Insert(item);
			//关联 Roles
			foreach (uint mn_Roles_in in mn_Roles)
				item.FlagRoles(mn_Roles_in);
			//关联 Topics
			foreach (ulong mn_Topics_in in mn_Topics)
				item.FlagTopics(mn_Topics_in);
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(ulong? Id, [FromForm] string Email, [FromForm] string Github, [FromForm] string Location, [FromForm] string Password, [FromForm] uint? Point, [FromForm] string Sign, [FromForm] string Username, [FromForm] string Website, [FromForm] string Weibo, [FromForm] uint[] mn_Roles, [FromForm] ulong[] mn_Topics) {
			UsersInfo item = new UsersInfo();
			item.Id = Id;
			item.Create_time = DateTime.Now;
			item.Email = Email;
			item.Github = Github;
			item.Location = Location;
			item.Password = Password;
			item.Point = Point;
			item.Sign = Sign;
			item.Username = Username;
			item.Website = Website;
			item.Weibo = Weibo;
			int affrows = Users.Update(item);
			//关联 Roles
			if (mn_Roles.Length == 0) {
				item.UnflagRolesALL();
			} else {
				List<uint> mn_Roles_list = mn_Roles.ToList();
				foreach (RolesInfo Obj_roles in item.Obj_roless) {
					int idx = mn_Roles_list.FindIndex(a => a == Obj_roles.Id);
					if (idx == -1) item.UnflagRoles(Obj_roles.Id);
					else mn_Roles_list.RemoveAt(idx);
				}
				mn_Roles_list.ForEach(a => item.FlagRoles(a));
			}
			//关联 Topics
			if (mn_Topics.Length == 0) {
				item.UnflagTopicsALL();
			} else {
				List<ulong> mn_Topics_list = mn_Topics.ToList();
				foreach (TopicsInfo Obj_topics in item.Obj_topicss) {
					int idx = mn_Topics_list.FindIndex(a => a == Obj_topics.Id);
					if (idx == -1) item.UnflagTopics(Obj_topics.Id);
					else mn_Topics_list.RemoveAt(idx);
				}
				mn_Topics_list.ForEach(a => item.FlagTopics(a));
			}
			if (affrows > 0) return new APIReturn(0, "成功");
			return new APIReturn(99, "失败");
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(ulong? Id) {
			int affrows = Users.Delete(Id);
			if (affrows > 0) return new APIReturn(0, string.Format("删除成功，影响行数：{0}", affrows));
			return new APIReturn(99, "失败");
		}
	}
}
