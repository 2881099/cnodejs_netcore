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
	public class PostsController : BaseController {
		private readonly ILogger<PostsController> _logger;
		public PostsController(ILogger<PostsController> logger) { _logger = logger; }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] ulong?[] Posts_id, [FromQuery] ulong?[] Topics_id, [FromQuery] ulong?[] Users_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Posts.Select
				.Where(!string.IsNullOrEmpty(key), "a.content like {0}", string.Concat("%", key, "%"));
			if (Posts_id.Length > 0) select.WherePosts_id(Posts_id);
			if (Topics_id.Length > 0) select.WhereTopics_id(Topics_id);
			if (Users_id.Length > 0) select.WhereUsers_id(Users_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Topics>("b", "b.id = a.topics_id")
				.InnerJoin<Users>("c", "c.id = a.users_id").Skip(skip).Limit(limit).ToList();
			return new APIReturn(0, "成功", "items", items.ToBson(), "count", count, 
				"items_topics", items.Select<PostsInfo, TopicsInfo>(a => a.Obj_topics).ToBson(), 
				"items_users", items.Select<PostsInfo, UsersInfo>(a => a.Obj_users).ToBson());
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(ulong? Id) {
			PostsInfo item = Posts.GetItem(Id);
			if (item == null) return new APIReturn(98, "记录不存在，或者没有权限");
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] ulong? Posts_id, [FromForm] ulong? Topics_id, [FromForm] ulong? Users_id, [FromForm] string Content, [FromForm] int? Count_good, [FromForm] int? Count_notgood, [FromForm] uint? Index) {
			PostsInfo item = new PostsInfo();
			item.Posts_id = Posts_id;
			item.Topics_id = Topics_id;
			item.Users_id = Users_id;
			item.Content = Content;
			item.Count_good = Count_good;
			item.Count_notgood = Count_notgood;
			item.Create_time = DateTime.Now;
			item.Index = Index;
			item = Posts.Insert(item);
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(ulong? Id, [FromForm] ulong? Posts_id, [FromForm] ulong? Topics_id, [FromForm] ulong? Users_id, [FromForm] string Content, [FromForm] int? Count_good, [FromForm] int? Count_notgood, [FromForm] uint? Index) {
			PostsInfo item = new PostsInfo();
			item.Id = Id;
			item.Posts_id = Posts_id;
			item.Topics_id = Topics_id;
			item.Users_id = Users_id;
			item.Content = Content;
			item.Count_good = Count_good;
			item.Count_notgood = Count_notgood;
			item.Create_time = DateTime.Now;
			item.Index = Index;
			int affrows = Posts.Update(item);
			if (affrows > 0) return new APIReturn(0, "成功");
			return new APIReturn(99, "失败");
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(ulong? Id) {
			int affrows = Posts.Delete(Id);
			if (affrows > 0) return new APIReturn(0, string.Format("删除成功，影响行数：{0}", affrows));
			return new APIReturn(99, "失败");
		}
	}
}
