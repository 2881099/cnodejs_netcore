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
	public class RolesController : BaseController {
		private readonly ILogger<RolesController> _logger;
		public RolesController(ILogger<RolesController> logger) { _logger = logger; }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] ulong[] Users_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Roles.Select
				.Where(!string.IsNullOrEmpty(key), "a.name like {0}", string.Concat("%", key, "%"));
			if (Users_id.Length > 0) select.WhereUsers_id(Users_id);
			int count;
			var items = select.Count(out count).Skip(skip).Limit(limit).ToList();
			return new APIReturn(0, "成功", "items", items.ToBson(), "count", count);
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			RolesInfo item = Roles.GetItem(Id);
			if (item == null) return new APIReturn(98, "记录不存在，或者没有权限");
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] string Name, [FromForm] ulong[] mn_Users) {
			RolesInfo item = new RolesInfo();
			item.Name = Name;
			item = Roles.Insert(item);
			//关联 Users
			foreach (ulong mn_Users_in in mn_Users)
				item.FlagUsers(mn_Users_in);
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] string Name, [FromForm] ulong[] mn_Users) {
			RolesInfo item = new RolesInfo();
			item.Id = Id;
			item.Name = Name;
			int affrows = Roles.Update(item);
			//关联 Users
			if (mn_Users.Length == 0) {
				item.UnflagUsersALL();
			} else {
				List<ulong> mn_Users_list = mn_Users.ToList();
				foreach (UsersInfo Obj_users in item.Obj_userss) {
					int idx = mn_Users_list.FindIndex(a => a == Obj_users.Id);
					if (idx == -1) item.UnflagUsers(Obj_users.Id);
					else mn_Users_list.RemoveAt(idx);
				}
				mn_Users_list.ForEach(a => item.FlagUsers(a));
			}
			if (affrows > 0) return new APIReturn(0, "成功");
			return new APIReturn(99, "失败");
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Roles.Delete(Id);
			if (affrows > 0) return new APIReturn(0, string.Format("删除成功，影响行数：{0}", affrows));
			return new APIReturn(99, "失败");
		}
	}
}
