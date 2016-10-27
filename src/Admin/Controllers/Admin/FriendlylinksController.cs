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
	public class FriendlylinksController : BaseController {
		private readonly ILogger<FriendlylinksController> _logger;
		public FriendlylinksController(ILogger<FriendlylinksController> logger) { _logger = logger; }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Friendlylinks.Select
				.Where(!string.IsNullOrEmpty(key), "a.link like {0} or a.logo like {0} or a.title like {0}", string.Concat("%", key, "%"));
			int count;
			var items = select.Count(out count).Skip(skip).Limit(limit).ToList();
			return new APIReturn(0, "成功", "items", items.ToBson(), "count", count);
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			FriendlylinksInfo item = Friendlylinks.GetItem(Id);
			if (item == null) return new APIReturn(98, "记录不存在，或者没有权限");
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] string Link, [FromForm] string Logo, [FromForm] uint? Sort, [FromForm] string Title) {
			FriendlylinksInfo item = new FriendlylinksInfo();
			item.Create_time = DateTime.Now;
			item.Link = Link;
			item.Logo = Logo;
			item.Sort = Sort;
			item.Title = Title;
			item = Friendlylinks.Insert(item);
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] string Link, [FromForm] string Logo, [FromForm] uint? Sort, [FromForm] string Title) {
			FriendlylinksInfo item = new FriendlylinksInfo();
			item.Id = Id;
			item.Create_time = DateTime.Now;
			item.Link = Link;
			item.Logo = Logo;
			item.Sort = Sort;
			item.Title = Title;
			int affrows = Friendlylinks.Update(item);
			if (affrows > 0) return new APIReturn(0, "成功");
			return new APIReturn(99, "失败");
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Friendlylinks.Delete(Id);
			if (affrows > 0) return new APIReturn(0, string.Format("删除成功，影响行数：{0}", affrows));
			return new APIReturn(99, "失败");
		}
	}
}
