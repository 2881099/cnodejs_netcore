using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using cnodejs.BLL;
using cnodejs.Model;
using Microsoft.AspNetCore.Http;

namespace cnodejs.Admin.Controllers {
	[Route("/")]
	public class HomeController : HomeBaseController {

		[HttpGet]
		public ViewResult 首页([FromServices] IConfigurationRoot cfg, [FromQuery] string tab, [FromQuery] int page = 1) {
			if (string.IsNullOrEmpty(tab)) tab = "all";
			var tab2 = tab == "all" ? null : Tags.GetItemByKeyname(tab);
			int count;
			ViewBag.curtag = tab2;
			ViewBag.topics = Topics.SelectByTags(tab2).Count(out count).Skip((page - 1) * 40).Limit(40).OrderBy("id desc").ToList(60 * 1, "indextopics_" + tab + page);
			ViewBag.count = count;
			ViewBag.author_nonereply = Topics.Select.WhereCount_posts(0).OrderBy("id desc").Limit(10).ToList(60 * 30, "author_nonereply");
			ViewBag.users_rank10 = Users.Select.OrderBy("point desc").Limit(10).ToList(60 * 30, "users_rank10");
			ViewBag.friendlylinks4 = Friendlylinks.Select.OrderBy("sort desc").Limit(4).ToList(60 * 30, "friendlylinks4");
			ViewBag.tags = Tags.Select.ToList(60 * 30, "tags");
			return View();
		}
		[HttpGet("topic/{id}")]
		public ViewResult 内容页(ulong id) {
			var topic = Topics.GetItem(id);
			var author = topic.Obj_owner_users;
			ViewBag.topic = topic;
			ViewBag.posts1 = Posts.GetItemByIndexAndTopics_id(1, topic.Id);
			ViewBag.posts = Posts.SelectByTopics_id(id).Where("a.`index` > 1").OrderBy("id desc").ToList(60 * 30, "posts_ByTopics_id" + id);
			ViewBag.author = author;
			ViewBag.author_recent_topics = Topics.SelectByOwner_users_id(author.Id).OrderBy("id desc").Limit(10).ToList(60 * 30, "author_recent_topics" + author.Id);
			ViewBag.author_nonereply = Topics.Select.WhereCount_posts(0).OrderBy("id desc").Limit(10).ToList(60 * 30, "author_nonereply");
			topic.UpdateDiy.SetCount_viewsIncrement(1).ExecuteNonQuery();
			return View();
		}
		[需要登陆]
		[HttpGet("topic/create")]
		public ViewResult 发布话题页() {
			ViewBag.tags = Tags.Select.ToList(60 * 30, "tags").Where<TagsInfo>(a => a.Id > 1); //非精华
			return View();
		}
		[需要登陆]
		[HttpPost("topic/create")]
		public IActionResult 发布话题([FromForm] string tab, [FromForm] string title, [FromForm] string t_content, [FromForm] uint[] topic_tags, [FromForm] string _csrf) {
			SqlHelper.Transaction(() => {
				TopicsInfo topic = Topics.Insert(new TopicsInfo {
					Title = title,
					Count_posts = 0,
					Count_views = 0,
					Create_time = DateTime.Now,
					Owner_users_id = LoginUser.Id,
					Top = 0
				});
				PostsInfo post = Posts.Insert(new PostsInfo {
					Content = t_content,
					Count_good = 0,
					Count_notgood = 0,
					Create_time = DateTime.Now,
					Topics_id = topic.Id,
					Users_id = LoginUser.Id,
					Index = 1
				});
				var tags_id = topic_tags.ToList();
				TagsInfo tag = Tags.GetItemByKeyname(tab);
				if (tag.Id > 1) tags_id.Add(tag.Id.Value);
				foreach (uint tagid in tags_id) topic.FlagTags(tagid);
			});
			RedisHelper.Remove("author_nonereply", "indextopics_all1", "indextopics_all2", "indextopics_all3", "indextopics_all4",
				"indextopics_good1", "indextopics_good2", "indextopics_good3", "indextopics_good4",
				"indextopics_share1", "indextopics_share2", "indextopics_share3", "indextopics_share4",
				"indextopics_ask1", "indextopics_ask2", "indextopics_ask3", "indextopics_ask4",
				"indextopics_job1", "indextopics_job2", "indextopics_job3", "indextopics_job4");
			return new RedirectResult("/");
		}
		[需要登陆]
		[HttpPost("topic/{id}/reply")]
		public IActionResult 内容回复(ulong id, [FromForm] ulong? reply_id, [FromForm] string r_content, [FromForm] string _csrf) {
			TopicsInfo topic = Topics.GetItem(id);
			if (topic == null) return new ContentResult { Content = "参数错误，回复的话题不能为空" };
			SqlHelper.Transaction(() => {
				PostsInfo post = Posts.Insert(new PostsInfo {
					Content = r_content,
					Count_good = 0,
					Count_notgood = 0,
					Create_time = DateTime.Now,
					Topics_id = topic.Id,
					Users_id = LoginUser.Id,
					Index = (uint)topic.Count_posts + 2,
					Posts_id = reply_id > 0 ? reply_id : null
				});
				topic.UpdateDiy.SetCount_postsIncrement(1).ExecuteNonQuery();
			});
			RedisHelper.Remove("posts_ByTopics_id" + id, "author_recent_topics" + LoginUser.Id, "author_nonereply");
			return new RedirectResult("/topic/" + id);
		}
		[需要登陆]
		[HttpPost("topic/collect")]
		public IActionResult 收藏([FromForm] ulong topic_id) {
			LoginUser.FlagTopics(topic_id);
			return View();
		}
		[需要登陆]
		[HttpPost("topic/de_collect")]
		public IActionResult 取消收藏([FromForm] ulong topic_id) {
			LoginUser.UnflagTopics(topic_id);
			return View();
		}
		[需要登陆]
		[HttpPost("topic/{id}/delete")]
		public IActionResult 删除(ulong id) {
			TopicsInfo topic = Topics.GetItem(id);
			if (topic.Owner_users_id == LoginUser.Id || LoginUser.Id == 1 //管理员
				) {
				Topics.Delete(id);
				return Json(new { success = true, message = "成功" });
			}
			return Json(new { success = false, message = "没有权限删除" });
		}
		[HttpPost("reply/{id}/up")]
		public IActionResult 点赞(ulong id) {
			Posts.UpdateDiy(id).SetCount_goodIncrement(1).ExecuteNonQuery();
			return Json(new { success = true, message = "成功" });
		}

		[HttpGet("getstart")]
		public ViewResult 新手入门页() {
			ViewBag.doc = Sysdoc.GetItem(1);
			return View("sysdoc");
		}
		[HttpGet("api")]
		public ViewResult API页() {
			ViewBag.doc = Sysdoc.GetItem(2);
			return View("sysdoc");
		}
		[HttpGet("about")]
		public ViewResult 关于页() {
			ViewBag.doc = Sysdoc.GetItem(3);
			ViewBag.fls = Friendlylinks.Select.OrderBy("sort desc").Skip(4).Limit(20).ToList(60 * 30, "about_fls");
			return View("sysdoc");
		}

		[HttpGet("signin")]
		public ViewResult 登陆页() {
			return View();
		}
		[HttpPost("signin")]
		public IActionResult 登陆([FromForm] string name, [FromForm] string pass, [FromForm] string _csrf) {
			UsersInfo user = Users.GetItemByUsername(name);
			if (user == null) user = Users.GetItemByEmail(name);
			if (user == null) return Json(new { success = false, message = "账号不存在" });
			if (user.Password != pass) return Json(new { success = false, message = "密码不正确" });
			Session.SetString("login.username", user.Username);
			return new RedirectResult("/");
		}
		[需要登陆]
		[HttpPost("signout")]
		public IActionResult 退出登陆() {
			Session.Remove("login.username");
			return new RedirectResult("/");
		}
		[HttpGet("search_pass")]
		public ViewResult 找回密码页() {
			return View();
		}
		[HttpPost("search_pass")]
		public IActionResult 找回密码([FromForm] string email, [FromForm] string _csrf) {
			return View();
		}

		[需要登陆]
		[HttpGet("my/messages")]
		public ViewResult 未读消息页() {
			return View();
		}
		[需要登陆]
		[HttpGet("setting")]
		public ViewResult 设置页() {
			ViewBag.friendlylinks4 = Friendlylinks.Select.OrderBy("sort desc").Limit(4).ToList(60 * 30, "friendlylinks4");
			return View();
		}
		[需要登陆]
		[HttpPost("setting")]
		public ViewResult 设置([FromForm] string name, [FromForm] string email, [FromForm] string url, [FromForm] string location, [FromForm] string weibo, [FromForm] string github, [FromForm] string signature, 
			[FromForm] string old_pass, [FromForm] string new_pass, [FromForm] string _csrf) {
			return View();
		}
	}
}
