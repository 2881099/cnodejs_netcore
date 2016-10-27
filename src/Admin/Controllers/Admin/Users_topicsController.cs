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
	public class Users_topicsController : BaseController {
		private readonly ILogger<Users_topicsController> _logger;
		public Users_topicsController(ILogger<Users_topicsController> logger) { _logger = logger; }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] ulong?[] Topics_id, [FromQuery] ulong?[] Users_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Users_topics.Select;
			if (Topics_id.Length > 0) select.WhereTopics_id(Topics_id);
			if (Users_id.Length > 0) select.WhereUsers_id(Users_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Topics>("b", "b.id = a.topics_id")
				.InnerJoin<Users>("c", "c.id = a.users_id").Skip(skip).Limit(limit).ToList();
			return new APIReturn(0, "成功", "items", items.ToBson(), "count", count, 
				"items_topics", items.Select<Users_topicsInfo, TopicsInfo>(a => a.Obj_topics).ToBson(), 
				"items_users", items.Select<Users_topicsInfo, UsersInfo>(a => a.Obj_users).ToBson());
		}

		[HttpGet(@"{Topics_id}/{Users_id}/")]
		public APIReturn Get_item(ulong? Topics_id, ulong? Users_id) {
			Users_topicsInfo item = Users_topics.GetItem(Topics_id, Users_id);
			if (item == null) return new APIReturn(98, "记录不存在，或者没有权限");
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] ulong? Topics_id, [FromForm] ulong? Users_id) {
			Users_topicsInfo item = new Users_topicsInfo();
			item.Topics_id = Topics_id;
			item.Users_id = Users_id;
			item = Users_topics.Insert(item);
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPut("{Topics_id}/{Users_id}/")]
		public APIReturn Put_update(ulong? Topics_id, ulong? Users_id) {
			Users_topicsInfo item = new Users_topicsInfo();
			item.Topics_id = Topics_id;
			item.Users_id = Users_id;
			int affrows = Users_topics.Update(item);
			if (affrows > 0) return new APIReturn(0, "成功");
			return new APIReturn(99, "失败");
		}

		[HttpDelete("{Topics_id}/{Users_id}/")]
		public APIReturn Delete_delete(ulong? Topics_id, ulong? Users_id) {
			int affrows = Users_topics.Delete(Topics_id, Users_id);
			if (affrows > 0) return new APIReturn(0, string.Format("删除成功，影响行数：{0}", affrows));
			return new APIReturn(99, "失败");
		}
	}
}
