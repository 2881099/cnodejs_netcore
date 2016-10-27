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
	public class SysdocController : BaseController {
		private readonly ILogger<SysdocController> _logger;
		public SysdocController(ILogger<SysdocController> logger) { _logger = logger; }

		[HttpGet]
		public APIReturn Get_list([FromServices]IConfigurationRoot cfg, [FromQuery] string key, [FromQuery] int limit = 20, [FromQuery] int skip = 0) {
			var select = Sysdoc.Select
				.Where(!string.IsNullOrEmpty(key), "a.content like {0} or a.title like {0}", string.Concat("%", key, "%"));
			int count;
			var items = select.Count(out count).Skip(skip).Limit(limit).ToList();
			return new APIReturn(0, "成功", "items", items.ToBson(), "count", count);
		}

		[HttpGet(@"{Id}/")]
		public APIReturn Get_item(uint? Id) {
			SysdocInfo item = Sysdoc.GetItem(Id);
			if (item == null) return new APIReturn(98, "记录不存在，或者没有权限");
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPost]
		public APIReturn Post_insert([FromForm] string Content, [FromForm] string Title) {
			SysdocInfo item = new SysdocInfo();
			item.Content = Content;
			item.Create_time = DateTime.Now;
			item.Title = Title;
			item = Sysdoc.Insert(item);
			return new APIReturn(0, "成功", "item", item.ToBson());
		}

		[HttpPut("{Id}/")]
		public APIReturn Put_update(uint? Id, [FromForm] string Content, [FromForm] string Title) {
			SysdocInfo item = new SysdocInfo();
			item.Id = Id;
			item.Content = Content;
			item.Create_time = DateTime.Now;
			item.Title = Title;
			int affrows = Sysdoc.Update(item);
			if (affrows > 0) return new APIReturn(0, "成功");
			return new APIReturn(99, "失败");
		}

		[HttpDelete("{Id}/")]
		public APIReturn Delete_delete(uint? Id) {
			int affrows = Sysdoc.Delete(Id);
			if (affrows > 0) return new APIReturn(0, string.Format("删除成功，影响行数：{0}", affrows));
			return new APIReturn(99, "失败");
		}
	}
}
