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
	public class UserclaimController : BaseController {
		private readonly ILogger<UserclaimController> _logger;
		public UserclaimController(ILogger<UserclaimController> logger) { _logger = logger; }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] ulong?[] Users_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Userclaim.Select
				.Where(!string.IsNullOrEmpty(key), "a.type like {0} or a.value like {0}", string.Concat("%", key, "%"));
			if (Users_id.Length > 0) select.WhereUsers_id(Users_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Users>("b", "b.id = a.users_id").Skip(skip).Limit(limit).ToList();
			return new APIReturn(0, "成功", "items", items.ToBson(), "count", count, 
				"items_users", items.Select<UserclaimInfo, UsersInfo>(a => a.Obj_users).ToBson());
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(ulong? Id) {
			UserclaimInfo item = Userclaim.GetItem(Id);
			if (item == null) return new APIReturn(98, "记录不存在，或者没有权限");
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] ulong? Users_id, [FromForm] string Type, [FromForm] string Value) {
			UserclaimInfo item = new UserclaimInfo();
			item.Users_id = Users_id;
			item.Create_time = DateTime.Now;
			item.Type = Type;
			item.Value = Value;
			item = Userclaim.Insert(item);
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(ulong? Id, [FromForm] ulong? Users_id, [FromForm] string Type, [FromForm] string Value) {
			UserclaimInfo item = new UserclaimInfo();
			item.Id = Id;
			item.Users_id = Users_id;
			item.Create_time = DateTime.Now;
			item.Type = Type;
			item.Value = Value;
			int affrows = Userclaim.Update(item);
			if (affrows > 0) return new APIReturn(0, "成功");
			return new APIReturn(99, "失败");
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(ulong? Id) {
			int affrows = Userclaim.Delete(Id);
			if (affrows > 0) return new APIReturn(0, string.Format("删除成功，影响行数：{0}", affrows));
			return new APIReturn(99, "失败");
		}
	}
}
