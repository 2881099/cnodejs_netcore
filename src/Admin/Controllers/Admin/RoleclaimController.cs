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
	public class RoleclaimController : BaseController {
		private readonly ILogger<RoleclaimController> _logger;
		public RoleclaimController(ILogger<RoleclaimController> logger) { _logger = logger; }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] uint?[] Roles_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Roleclaim.Select
				.Where(!string.IsNullOrEmpty(key), "a.type like {0} or a.value like {0}", string.Concat("%", key, "%"));
			if (Roles_id.Length > 0) select.WhereRoles_id(Roles_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Roles>("b", "b.id = a.roles_id").Skip(skip).Limit(limit).ToList();
			return new APIReturn(0, "成功", "items", items.ToBson(), "count", count, 
				"items_roles", items.Select<RoleclaimInfo, RolesInfo>(a => a.Obj_roles).ToBson());
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			RoleclaimInfo item = Roleclaim.GetItem(Id);
			if (item == null) return new APIReturn(98, "记录不存在，或者没有权限");
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Roles_id, [FromForm] string Type, [FromForm] string Value) {
			RoleclaimInfo item = new RoleclaimInfo();
			item.Roles_id = Roles_id;
			item.Create_time = DateTime.Now;
			item.Type = Type;
			item.Value = Value;
			item = Roleclaim.Insert(item);
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] uint? Roles_id, [FromForm] string Type, [FromForm] string Value) {
			RoleclaimInfo item = new RoleclaimInfo();
			item.Id = Id;
			item.Roles_id = Roles_id;
			item.Create_time = DateTime.Now;
			item.Type = Type;
			item.Value = Value;
			int affrows = Roleclaim.Update(item);
			if (affrows > 0) return new APIReturn(0, "成功");
			return new APIReturn(99, "失败");
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Roleclaim.Delete(Id);
			if (affrows > 0) return new APIReturn(0, string.Format("删除成功，影响行数：{0}", affrows));
			return new APIReturn(99, "失败");
		}
	}
}
