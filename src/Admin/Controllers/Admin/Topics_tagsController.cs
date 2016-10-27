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
	public class Topics_tagsController : BaseController {
		private readonly ILogger<Topics_tagsController> _logger;
		public Topics_tagsController(ILogger<Topics_tagsController> logger) { _logger = logger; }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] uint?[] Tags_id, [FromQuery] ulong?[] Topics_id, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Topics_tags.Select;
			if (Tags_id.Length > 0) select.WhereTags_id(Tags_id);
			if (Topics_id.Length > 0) select.WhereTopics_id(Topics_id);
			int count;
			var items = select.Count(out count)
				.InnerJoin<Tags>("b", "b.id = a.tags_id")
				.InnerJoin<Topics>("c", "c.id = a.topics_id").Skip(skip).Limit(limit).ToList();
			return new APIReturn(0, "成功", "items", items.ToBson(), "count", count, 
				"items_tags", items.Select<Topics_tagsInfo, TagsInfo>(a => a.Obj_tags).ToBson(), 
				"items_topics", items.Select<Topics_tagsInfo, TopicsInfo>(a => a.Obj_topics).ToBson());
		}

		[HttpGet(@"{Tags_id}/{Topics_id}/")]
		public APIReturn Get_item(uint? Tags_id, ulong? Topics_id) {
			Topics_tagsInfo item = Topics_tags.GetItem(Tags_id, Topics_id);
			if (item == null) return new APIReturn(98, "记录不存在，或者没有权限");
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] uint? Tags_id, [FromForm] ulong? Topics_id) {
			Topics_tagsInfo item = new Topics_tagsInfo();
			item.Tags_id = Tags_id;
			item.Topics_id = Topics_id;
			item = Topics_tags.Insert(item);
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPut("{Tags_id}/{Topics_id}/")]
		public APIReturn Put_update(uint? Tags_id, ulong? Topics_id) {
			Topics_tagsInfo item = new Topics_tagsInfo();
			item.Tags_id = Tags_id;
			item.Topics_id = Topics_id;
			int affrows = Topics_tags.Update(item);
			if (affrows > 0) return new APIReturn(0, "成功");
			return new APIReturn(99, "失败");
		}

		[HttpDelete("{Tags_id}/{Topics_id}/")]
		public APIReturn Delete_delete(uint? Tags_id, ulong? Topics_id) {
			int affrows = Topics_tags.Delete(Tags_id, Topics_id);
			if (affrows > 0) return new APIReturn(0, string.Format("删除成功，影响行数：{0}", affrows));
			return new APIReturn(99, "失败");
		}
	}
}
