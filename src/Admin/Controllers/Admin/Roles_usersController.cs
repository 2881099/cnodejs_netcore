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
	public class Roles_usersController : BaseController {
		private readonly ILogger<Roles_usersController> _logger;
		public Roles_usersController(ILogger<Roles_usersController> logger) { _logger = logger; }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Roles_id, [FromQuery] ulong?[] Users_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Roles_users.Select;
			if (Roles_id.Length > 0) select.WhereRoles_id(Roles_id);
			if (Users_id.Length > 0) select.WhereUsers_id(Users_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Roles>("b", "b.id = a.roles_id")
				.InnerJoin<Users>("c", "c.id = a.users_id").Skip(skip).Limit(limit).ToList();
			return new APIReturn(0, "成功", "items", items.ToBson(), "count", count, 
				"items_roles", items.Select<Roles_usersInfo, RolesInfo>(a => a.Obj_roles).ToBson(), 
				"items_users", items.Select<Roles_usersInfo, UsersInfo>(a => a.Obj_users).ToBson());
		}

		[HttpGet(@"{Roles_id}/{Users_id}/")]
		public APIReturn Get_item(uint? Roles_id, ulong? Users_id) {
			Roles_usersInfo item = Roles_users.GetItem(Roles_id, Users_id);
			if (item == null) return new APIReturn(98, "记录不存在，或者没有权限");
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Roles_id, [FromForm] ulong? Users_id) {
			Roles_usersInfo item = new Roles_usersInfo();
			item.Roles_id = Roles_id;
			item.Users_id = Users_id;
			item = Roles_users.Insert(item);
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPut("{Roles_id}/{Users_id}/")]
		public APIReturn Put_update(uint? Roles_id, ulong? Users_id) {
			Roles_usersInfo item = new Roles_usersInfo();
			item.Roles_id = Roles_id;
			item.Users_id = Users_id;
			int affrows = Roles_users.Update(item);
			if (affrows > 0) return new APIReturn(0, "成功");
			return new APIReturn(99, "失败");
		}

		[HttpDelete("{Roles_id}/{Users_id}/")]
		public APIReturn Delete_delete(uint? Roles_id, ulong? Users_id) {
			int affrows = Roles_users.Delete(Roles_id, Users_id);
			if (affrows > 0) return new APIReturn(0, string.Format("删除成功，影响行数：{0}", affrows));
			return new APIReturn(99, "失败");
		}
	}
}
