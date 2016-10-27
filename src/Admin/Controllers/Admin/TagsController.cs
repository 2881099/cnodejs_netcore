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
	public class TagsController : BaseController {
		private readonly ILogger<TagsController> _logger;
		public TagsController(ILogger<TagsController> logger) { _logger = logger; }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] ulong[] Topics_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Tags.Select
				.Where(!string.IsNullOrEmpty(key), "a.keyname like {0} or a.name like {0}", string.Concat("%", key, "%"));
			if (Topics_id.Length > 0) select.WhereTopics_id(Topics_id);
			int count;
			var items = select.Count(out count).Skip(skip).Limit(limit).ToList();
			return new APIReturn(0, "成功", "items", items.ToBson(), "count", count);
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			TagsInfo item = Tags.GetItem(Id);
			if (item == null) return new APIReturn(98, "记录不存在，或者没有权限");
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] string Keyname, [FromForm] string Name, [FromForm] ulong[] mn_Topics) {
			TagsInfo item = new TagsInfo();
			item.Create_time = DateTime.Now;
			item.Keyname = Keyname;
			item.Name = Name;
			item = Tags.Insert(item);
			//关联 Topics
			foreach (ulong mn_Topics_in in mn_Topics)
				item.FlagTopics(mn_Topics_in);
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] string Keyname, [FromForm] string Name, [FromForm] ulong[] mn_Topics) {
			TagsInfo item = new TagsInfo();
			item.Id = Id;
			item.Create_time = DateTime.Now;
			item.Keyname = Keyname;
			item.Name = Name;
			int affrows = Tags.Update(item);
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
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Tags.Delete(Id);
			if (affrows > 0) return new APIReturn(0, string.Format("删除成功，影响行数：{0}", affrows));
			return new APIReturn(99, "失败");
		}
	}
}
