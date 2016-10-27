using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using cnodejs.BLL;
using cnodejs.Model;

namespace cnodejs.Admin.Controllers {
	[Route("api/[controller]")]
	public class SysController : Controller {
		[HttpGet(@"connection")]
		public object Get_connection() {
			List<Hashtable> ret = new List<Hashtable>();
			foreach (var conn in SqlHelper.Instance.Pool.AllConnections) {
				ret.Add(new Hashtable() {
						{ "数据库", conn.SqlConnection.Database },
						{ "状态", conn.SqlConnection.State },
						{ "最后活动", conn.LastActive },
						{ "获取次数", conn.UseSum }
					});
			}
			return new {
				FreeConnections = SqlHelper.Instance.Pool.FreeConnections.Count,
				AllConnections = SqlHelper.Instance.Pool.AllConnections.Count,
				List = ret
			};
		}

		[HttpGet(@"init_sysdir")]
		public APIReturn Get_init_sysdir() {
			/*
			if (Sysdir.SelectByParent_id(null).Count() > 0)
				return new APIReturn(-33, "本系统已经初始化过，页面没经过任何操作退出。");

			SysdirInfo dir1, dir2, dir3;
			dir1 = Sysdir.Insert(null, DateTime.Now, "运营管理", 1, null);

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "friendlylinks", 1, "/friendlylinks/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/friendlylinks/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/friendlylinks/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/friendlylinks/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/friendlylinks/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "posts", 2, "/posts/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/posts/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/posts/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/posts/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/posts/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "sysdoc", 3, "/sysdoc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/sysdoc/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/sysdoc/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/sysdoc/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/sysdoc/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "tags", 4, "/tags/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/tags/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/tags/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/tags/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/tags/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "topics", 5, "/topics/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/topics/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/topics/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/topics/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/topics/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "topics_tags", 6, "/topics_tags/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/topics_tags/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/topics_tags/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/topics_tags/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/topics_tags/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "users", 7, "/users/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/users/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/users/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/users/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/users/del.aspx");

			dir2 = Sysdir.Insert(dir1.Id, DateTime.Now, "users_topics", 8, "/users_topics/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "列表", 1, "/users_topics/");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "添加", 2, "/users_topics/add.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "编辑", 3, "/users_topics/edit.aspx");
			dir3 = Sysdir.Insert(dir2.Id, DateTime.Now, "删除", 4, "/users_topics/del.aspx");
			*/
			return new APIReturn(0, "管理目录已初始化完成。");
		}
	}
}
