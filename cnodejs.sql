/*
Navicat MySQL Data Transfer

Source Server         : 192.168.131.62
Source Server Version : 50541
Source Host           : 192.168.131.62:3306
Source Database       : cnodejs

Target Server Type    : MYSQL
Target Server Version : 50541
File Encoding         : 65001

Date: 2016-10-27 20:28:51
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for friendlylinks
-- ----------------------------
DROP TABLE IF EXISTS `friendlylinks`;
CREATE TABLE `friendlylinks` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `title` varchar(96) DEFAULT NULL COMMENT '标题',
  `logo` varchar(96) DEFAULT NULL COMMENT 'LOGO',
  `link` varchar(255) DEFAULT NULL COMMENT '链接地址',
  `sort` int(10) unsigned DEFAULT NULL COMMENT '排序',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of friendlylinks
-- ----------------------------

-- ----------------------------
-- Table structure for posts
-- ----------------------------
DROP TABLE IF EXISTS `posts`;
CREATE TABLE `posts` (
  `id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `users_id` bigint(20) unsigned DEFAULT NULL COMMENT '作者',
  `topics_id` bigint(20) unsigned DEFAULT NULL COMMENT '主题',
  `posts_id` bigint(20) unsigned DEFAULT NULL,
  `index` int(10) unsigned DEFAULT NULL COMMENT '楼层',
  `content` text COMMENT '内容',
  `count_good` int(11) DEFAULT NULL COMMENT '顶',
  `count_notgood` int(11) DEFAULT NULL COMMENT '踩',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`),
  UNIQUE KEY `a` (`topics_id`,`index`),
  KEY `fk_posts_users_1` (`users_id`),
  KEY `fk_posts_posts_1` (`posts_id`),
  CONSTRAINT `fk_posts_posts_1` FOREIGN KEY (`posts_id`) REFERENCES `posts` (`id`),
  CONSTRAINT `fk_posts_topics_1` FOREIGN KEY (`topics_id`) REFERENCES `topics` (`id`),
  CONSTRAINT `fk_posts_users_1` FOREIGN KEY (`users_id`) REFERENCES `users` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of posts
-- ----------------------------
INSERT INTO `posts` VALUES ('1', '1', '1', null, '1', 'bbbbbbbbbbbbbbbbbvvvvvvvvvvvvvvvvvvv', '0', '0', '2016-10-27 17:01:48');
INSERT INTO `posts` VALUES ('3', '1', '1', null, '2', '1222', '0', '0', '2016-10-27 18:06:18');
INSERT INTO `posts` VALUES ('4', '1', '1', null, '3', '3333', '0', '0', '2016-10-27 18:06:26');
INSERT INTO `posts` VALUES ('5', '1', '1', null, '4', '777777777777777777', '0', '0', '2016-10-27 18:07:01');
INSERT INTO `posts` VALUES ('6', '1', '2', null, '1', 'ddddddddddddddd', '0', '0', '2016-10-27 19:34:15');
INSERT INTO `posts` VALUES ('7', '1', '3', null, '1', 'crypto-js\r\n纯javascript写的加密算法类库，包括 MD5 SHA-1 SHA-256 AES Rabbit MARC4 HMAC (HMAC-MD5 HMAC-SHA1 HMAC-SHA256) PBKDF2\r\nes6-promise\r\n轻量级库，用于组织异步代码的工具\r\nES6 Promise 的 polyfill（见附件）\r\njs-md5\r\n一个支持utf-8编码的简洁md5哈希算法方法\r\nnode-sass\r\n一个库\r\n把node绑定到libsass（见附件）\r\nprompt\r\n终端命令行输入控件\r\n可根据用户输入的信息做相应的操作', '0', '0', '2016-10-27 19:38:30');
INSERT INTO `posts` VALUES ('8', '1', '4', null, '1', '按照网上部署heroku node项目时，遇到问题。在push的时候报错。\r\nD88C.tm.png\r\n\r\n请问有部署过的能解决这个问题吗。。谢谢！', '0', '0', '2016-10-27 19:39:46');
INSERT INTO `posts` VALUES ('9', '1', '5', null, '1', '使用vue+vuex+vue-router+webpack重构了cnode社区，基本实现所有的功能。\r\ngithub地址：\r\n	https://github.com/Ocean1509/vue-mycnode\r\n\r\n演示地址：\r\n	https://ocean1509.github.io/vue-mycnode/#!/', '0', '0', '2016-10-27 19:40:11');
INSERT INTO `posts` VALUES ('10', '1', '6', null, '1', 'What is Node Party?\r\n每月组织JavaScript/Node开发者聚会，关注热门的前端、后端框架，开发工具和方法。\r\n\r\n＊ 学习新东西\r\n＊ 认识新朋友\r\n＊ 聚餐\r\n\r\n我们会为你准备：\r\n咖啡，甜点，红酒，爆米花和动听的音乐，一起度过一个愉快的下午。\r\n\r\n时间\r\n\r\n11月06日 星期日 下午13:00 ～ 18:00\r\n\r\n地点\r\n\r\n科技寺（三里屯）\r\n\r\n 地址: 朝阳区南三里屯路北京机电院10号楼2层凯富大厦正南\r\n\r\n报名地址\r\n\r\n 为了提升活动质量，鼓励分享，本期nodeparty进行售票，早鸟票和标准票只是价格上不同，分别是29元和49元。\r\n\r\nhttp://www.bagevent.com/event/252533', '0', '0', '2016-10-27 19:41:24');
INSERT INTO `posts` VALUES ('11', '1', '7', null, '1', 'let buf = Buffer.from([0xa4,0xfb,0x02,0x12,0x9a,0x90,0x7c,0x4a]);\r\n这个buffer是一个小端的大整数，5367323847320337316\r\n用buf.readUIntLE(0,8)只能得到5367323847320337000\r\n\r\n如何得到正确的结果？求大概描述一下思路。\r\n\r\n补充，这个问题可以用以下代码表示，把一个小端的十六进制字符串转化为一个十进制的大整数字符串\r\n\r\n// return {@string}\r\nfunction converHexStrToDecStrLittleEnd(str){\r\n	//求这中间的实现思路\r\n}\r\n\r\n//test case \r\nassert(  converHexStrToDecStrLittleEnd(\'a4fb02129a907c4a\') ===  \'5367323847320337316\' )\r\n求 converHexStrToDecStrLittleEnd 这个函数怎么实现', '0', '0', '2016-10-27 19:41:41');
INSERT INTO `posts` VALUES ('12', '1', '8', null, '1', '比如数据保存在一个全局变量 var message里\r\n这样多个woker和 master都同时读写这个数据会不会出错呢？\r\napp.js\r\n\r\nvar cluster = require(\'cluster\');\r\nvar io = require(\'socket.io\')();\r\nvar numCPUs = require(\'os\').cpus().length;\r\nconsole.log(\' numCPUs is \',numCPUs);\r\nvar connections=0;\r\nif (cluster.isMaster) {\r\n   console.log(\"master start...\");\r\n\r\n   // Fork workers.\r\n   for (var i = 0; i < numCPUs; i++) {\r\n       cluster.fork();\r\n   }\r\n\r\n   cluster.on(\'listening\',function(worker,address){\r\n       console.log(\'[Master ]listening: worker \' + worker.process.pid +\', Address: \'+address.address+\":\"+address.port);\r\n   });\r\n\r\n   cluster.on(\'exit\', function(worker, code, signal) {\r\n       console.log(\'[Master]  worker exit \' + worker.process.pid + \' died\');\r\n   });\r\n} else if (cluster.isWorker) {\r\n\r\n\r\n\r\n   io.on(\'connection\', function(socket) {\r\n\r\n      console.log(\'this is process\',process.pid);\r\n     connections++;\r\n\r\n      console.log(\"client ++\",connections);\r\n\r\n\r\n      socket.on(\'disconnect\', function() {\r\n connections--;\r\n   console.log(\"client --\",connections);\r\n      });\r\n\r\n     socket.on(\'message\',function (msg) {\r\n     	console.log(msg);\r\n     	 \r\n     });\r\n\r\n   });\r\nio.listen(9000);\r\n\r\n\r\n}\r\n\r\n\r\ntest.html\r\n\r\n<!doctype html>\r\n<html lang=\"en\">\r\n<head>\r\n	<meta charset=\"UTF-8\">\r\n	<title>\r\n		\r\n	</title>\r\n	<script src=\"/js/socket.io.js\"></script>\r\n</head>\r\n<body>\r\n	\r\n\r\n\r\n\r\n<script type=\"text/javascript\">\r\n\r\n\r\nvar socket = io.connect(\'ws://localhost:9000\',{\'reconnection\':true});\r\n\r\n\r\nsocket.on(\'message\', function(msg){\r\n\r\n  console.log(msg);\r\n\r\n});\r\n\r\n\r\n socket.emit(\'message\',{foo:11,bar:22});\r\n \r\n\r\n</script>\r\n\r\n</body>\r\n</html>\r\n这样启动app.js ，用第一个浏览器刷页面，能看到 控制台显示  client ++ 但是第二个浏览器刷页面，没有反应', '0', '0', '2016-10-27 19:42:00');
INSERT INTO `posts` VALUES ('13', '1', '9', null, '1', 'nodejs Buffer 的encoding 好像是没有gbk的, 如果我想把汉字转成Buffer,\r\nvar buf = new Buffer(‘我的设备’),   这样的话是3个字节表示一个汉字, 怎样才能2个字节表示一个汉字呢?', '0', '0', '2016-10-27 19:42:16');
INSERT INTO `posts` VALUES ('14', '1', '10', null, '1', '用到的技术都比较基础, jQuery什么的。\r\n###运行截图\r\n\r\n登录 image\r\n主页 image\r\n话题 image\r\n消息 image\r\n收藏 image\r\n用户信息 image\r\n创建话题 image\r\n有很多功能还未实现, 具体见github。有问题的话欢迎指出。 Note:目前还没提供windows的下载，但是安装包貌似太大, 超过github的上限，所以还是推荐自己编译安装的方式。 现已提供Mac, windows64位和32位的打包命令', '0', '0', '2016-10-27 19:42:50');
INSERT INTO `posts` VALUES ('15', '1', '11', null, '1', '现在很多公司的服务器语言还是java,而且老程序员貌似很看不起node,表示node只适合小打小闹?\r\n\r\n我自己也是从java(4y) 转node(3y) …我个人编程感觉上来讲,还是觉得node是未来趋势,java目前可能是如日中天,但是我赶紧它就像git—>svn一样.\r\n\r\n好东西注定会从非主流变成主流~\r\n\r\n期待大家用你的观点,来碰撞出火花…各抒己见', '0', '0', '2016-10-27 19:43:05');
INSERT INTO `posts` VALUES ('16', '1', '12', null, '1', '解析中国身份证号码（地址，性别，生日，星座），支持 18 位和 15 位身份证\r\n\r\n本项目 Github 主页\r\n\r\nUsage\r\n\r\n使用示例：\r\n\r\nvar parse_id = require(\'parse-cn-idcard\');\r\nvar result = parse_id(\'37010219940709292X\');\r\nconsole.log(result);\r\n输出结果：\r\n\r\n{\r\n  \"area\": {\r\n    \"id\": \"370102\",\r\n    \"name\": \"山东省济南市历下区\"\r\n  },\r\n  \"birthday\": {\r\n    \"date\": \"19940709\",\r\n    \"constellation_cn\": \"巨蟹座\",\r\n    \"constellation_en\": \"Cancer\"\r\n  },\r\n  \"gender\": {\r\n    \"cn\": \"女\",\r\n    \"en\": \"Female\"\r\n  }\r\n}', '0', '0', '2016-10-27 19:43:21');
INSERT INTO `posts` VALUES ('17', '1', '13', null, '1', '初学者，想用bootstrap模板，在网上看到很多很不错的网站前端模板，但是有一个问题是这些网页模板都是现成的html写好了的页面，如果我不用jade这样的模板引擎，怎么使用现成的html？', '0', '0', '2016-10-27 19:44:18');
INSERT INTO `posts` VALUES ('18', '1', '14', null, '1', 'function SocketIO() {\r\n    this.socket = null;\r\n	this.reconnect=null\r\n}\r\n\r\nSocketIO.prototype.start = (server)=> {\r\n    let self = this;\r\n    this.socket = so(server);\r\n	this.socket.on(\'connect\', ()=> {\r\n		self.waitType()\r\n    });\r\n    this.socket.on(\'disconnect\', ()=> {\r\n            self.socket = self.start(self.reconnect);\r\n    });\r\n};\r\n\r\nSocketIO.prototype.waitType = ()=> {\r\n    let self = this;\r\n    process.stdin.setEncoding(\'utf8\');\r\n    process.stdin.on(\'readable\', () => {\r\n        let chunk = process.stdin.read();\r\n        let cmds = chunk.trim().split(\' \');\r\n        switch (cmds[0]) {\r\n            case \'connect\':\r\n                self.reconnect = cmds[1];\r\n                break;\r\n        }\r\n    });\r\n};', '0', '0', '2016-10-27 19:44:40');
INSERT INTO `posts` VALUES ('19', '1', '15', null, '1', '刚接触rabbitmq，查看它的一个例子  Hello World，发现必须要等待sendToQueue()结束之后才开始consumer，一般mq不是一边生产一边消费的么？是我代码的问题，还是我理解的问题？\r\n代码基本与上面的链接中的代码类似，只不过在sendToQueue()的外边加了一个循环：\r\n\r\n	for (var i = 0; i < 400000; i++) {\r\n       	   ch.sendToQueue(q, new Buffer(i + \'\'));\r\n      	   console.log(\'whwhwhw\' + i);\r\n   	  }\r\n想上面这样，将 i依次的打入queue中，consumer应该不断的收到数据才对啊，怎么会在这个for完成之后才开始consumer？', '0', '0', '2016-10-27 19:44:55');
INSERT INTO `posts` VALUES ('20', '1', '16', null, '1', '在做学习@nswbmw的第十五章的过程中发现，点击转载会出现转载成功数据库有保存但是跳转有错误。\r\n看了代码发现是跳转的url有问题。\r\n代码如下，更改了url。比如：post.name为req.params.name。\r\n\r\napp.get(\'/reprint/:name/:day/:title\', checkLogin);\r\napp.get(\'/reprint/:name/:day/:title\', function (req, res) {\r\n  Post.edit(req.params.name, req.params.day, req.params.title, function (err, post) {\r\n    if (err) {\r\n      req.flash(\'error\', err); \r\n      return res.redirect(back);\r\n    }\r\n    var currentUser = req.session.user,\r\n        reprint_from = {name: post.name, day: post.time.day, title: post.title},\r\n        reprint_to = {name: currentUser.name, head: currentUser.head};\r\n    Post.reprint(reprint_from, reprint_to, function (err, post) {\r\n      if (err) {\r\n        req.flash(\'error\', err); \r\n        return res.redirect(\'back\');\r\n      }\r\n      req.flash(\'success\', \'转载成功!\');\r\n      var url = encodeURI(\'/u/\' + req.params.name + \'/\' + req.params.day + \'/\' + req.params.title);\r\n      //跳转到转载后的文章页面\r\n      res.redirect(url);\r\n    });\r\n  });\r\n});', '0', '0', '2016-10-27 19:45:13');
INSERT INTO `posts` VALUES ('21', '1', '15', null, '2', '1111', '0', '0', '2016-10-27 19:45:25');

-- ----------------------------
-- Table structure for sysdoc
-- ----------------------------
DROP TABLE IF EXISTS `sysdoc`;
CREATE TABLE `sysdoc` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `title` varchar(255) DEFAULT NULL COMMENT '标题',
  `content` text COMMENT '内容',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sysdoc
-- ----------------------------
INSERT INTO `sysdoc` VALUES ('1', 'Node.js 新手入门', 'Node.js 入门\r\n\r\n《快速搭建 Node.js 开发环境以及加速 npm》\r\n\r\nhttp://fengmk2.com/blog/2014/03/node-env-and-faster-npm.html\r\n\r\n《Node.js 包教不包会》\r\n\r\nhttps://github.com/alsotang/node-lessons\r\n\r\n《ECMAScript 6入门》\r\n\r\nhttp://es6.ruanyifeng.com/\r\n\r\n《七天学会NodeJS》\r\n\r\nhttps://github.com/nqdeng/7-days-nodejs\r\n\r\nNode.js 资源\r\n\r\n《前端资源教程》\r\n\r\nhttps://cnodejs.org/topic/56ef3edd532839c33a99d00e\r\n\r\n《国内的 npm 镜像源》\r\n\r\nhttp://cnpmjs.org/\r\n\r\n《node weekly》\r\n\r\nhttp://nodeweekly.com/issues\r\n\r\n《node123-node.js中文资料导航》\r\n\r\nhttps://github.com/youyudehexie/node123\r\n\r\n《A curated list of delightful Node.js packages and resources》\r\n\r\nhttps://github.com/sindresorhus/awesome-nodejs\r\n\r\n《Node.js Books》\r\n\r\nhttps://github.com/pana/node-books\r\n\r\nNode.js 名人\r\n\r\n《名人堂》\r\n\r\nhttps://github.com/cnodejs/nodeclub/wiki/名人堂\r\n\r\nNode.js 服务器\r\n\r\n新手搭建 Node.js 服务器，推荐使用无需备案的 DigitalOcean(https://www.digitalocean.com/)', '2016-10-27 15:06:03');
INSERT INTO `sysdoc` VALUES ('2', 'API', '以下 api 路径均以 https://cnodejs.org/api/v1 为前缀\r\n\r\n主题\r\n\r\nget /topics 主题首页\r\n\r\n接收 get 参数\r\n\r\npage Number 页数\r\ntab String 主题分类。目前有 ask share job good\r\nlimit Number 每一页的主题数量\r\nmdrender String 当为 false 时，不渲染。默认为 true，渲染出现的所有 markdown 格式文本。\r\n示例：/api/v1/topics\r\n\r\nget /topic/:id 主题详情\r\n\r\n接收 get 参数\r\n\r\nmdrender String 当为 false 时，不渲染。默认为 true，渲染出现的所有 markdown 格式文本。\r\naccesstoken String 当需要知道一个主题是否被特定用户收藏时，才需要带此参数。会影响返回值中的 is_collect 值。\r\n示例：/api/v1/topic/5433d5e4e737cbe96dcef312\r\n\r\npost /topics 新建主题\r\n\r\n接收 post 参数\r\n\r\naccesstoken String 用户的 accessToken\r\ntitle String 标题\r\ntab String 目前有 ask share job\r\ncontent String 主体内容\r\n返回值示例\r\n\r\n{success: true, topic_id: \'5433d5e4e737cbe96dcef312\'}\r\npost /topics/update 编辑主题\r\n\r\n接收 post 参数\r\n\r\naccesstoken String 用户的 accessToken\r\ntopic_id String 主题id\r\ntitle String 标题\r\ntab String 目前有 ask share job\r\ncontent String 主体内容\r\n返回值示例\r\n\r\n{success: true, topic_id: \'5433d5e4e737cbe96dcef312\'}\r\n主题收藏\r\n\r\npost /topic_collect/collect 收藏主题\r\n\r\n接收 post 参数\r\n\r\naccesstoken String 用户的 accessToken\r\ntopic_id String 主题的id\r\n返回值示例\r\n\r\n{\"success\": true}\r\npost /topic_collect/de_collect 取消主题\r\n\r\n接收 post 参数\r\n\r\naccesstoken String 用户的 accessToken\r\ntopic_id String 主题的id\r\n返回值示例\r\n\r\n{success: true}\r\nget /topic_collect/:loginname 用户所收藏的主题\r\n\r\n示例：/api/v1/topic_collect/alsotang\r\n\r\n评论\r\n\r\npost /topic/:topic_id/replies 新建评论\r\n\r\n接收 post 参数\r\n\r\naccesstoken String 用户的 accessToken\r\ncontent String 评论的主体\r\nreply_id String 如果这个评论是对另一个评论的回复，请务必带上此字段。这样前端就可以构建出评论线索图。\r\n返回值示例\r\n\r\n{success: true, reply_id: \'5433d5e4e737cbe96dcef312\'}\r\npost /reply/:reply_id/ups 为评论点赞\r\n\r\n接受 post 参数\r\n\r\naccesstoken String\r\n接口会自动判断用户是否已点赞，如果否，则点赞；如果是，则取消点赞。点赞的动作反应在返回数据的 action 字段中，up or down。\r\n\r\n返回值示例\r\n\r\n{\"success\": true, \"action\": \"down\"}\r\n用户\r\n\r\nget /user/:loginname 用户详情\r\n\r\n示例：/api/v1/user/alsotang\r\n\r\npost /accesstoken 验证 accessToken 的正确性\r\n\r\n接收 post 参数\r\n\r\naccesstoken String 用户的 accessToken\r\n如果成功匹配上用户，返回成功信息。否则 403。\r\n\r\n返回值示例\r\n\r\n{success: true, loginname: req.user.loginname}\r\n消息通知\r\n\r\nget /message/count 获取未读消息数\r\n\r\n接收 get 参数\r\n\r\naccesstoken String\r\n返回值示例\r\n\r\n{ data: 3 }\r\nget /messages 获取已读和未读消息\r\n\r\n接收 get 参数\r\n\r\naccesstoken String\r\nmdrender String 当为 false 时，不渲染。默认为 true，渲染出现的所有 markdown 格式文本。\r\n返回值示例\r\n\r\n{\r\n  data: {\r\n    has_read_messages: [],\r\n    hasnot_read_messages: [\r\n      {\r\n        id: \"543fb7abae523bbc80412b26\",\r\n        type: \"at\",\r\n        has_read: false,\r\n        author: {\r\n          loginname: \"alsotang\",\r\n          avatar_url: \"https://avatars.githubusercontent.com/u/1147375?v=2\"\r\n        },\r\n        topic: {\r\n          id: \"542d6ecb9ecb3db94b2b3d0f\",\r\n          title: \"adfadfadfasdf\",\r\n          last_reply_at: \"2014-10-18T07:47:22.563Z\"\r\n        },\r\n        reply: {\r\n          id: \"543fb7abae523bbc80412b24\",\r\n          content: \"[@alsotang](/user/alsotang) 哈哈\",\r\n          ups: [ ],\r\n          create_at: \"2014-10-16T12:18:51.566Z\"\r\n          }\r\n        },\r\n    ...\r\n    ]\r\n  }\r\n}\r\npost /message/mark_all 标记全部已读\r\n\r\n接收 post 参数\r\n\r\naccesstoken String\r\n返回值示例\r\n\r\n{ success: true,\r\n  marked_msgs: [ { id: \'544ce385aeaeb5931556c6f9\' } ] }\r\n知识点\r\n\r\n如何获取 accessToken？ 用户登录后，在设置页面可以看到自己的 accessToken。 建议各移动端应用使用手机扫码的形式登录，验证使用 /accesstoken 接口，登录后长期保存 accessToken。', '2016-10-27 15:06:29');
INSERT INTO `sysdoc` VALUES ('3', '关于', '关于\r\n\r\nCNode 社区为国内最大最具影响力的 Node.js 开源技术社区，致力于 Node.js 的技术研究。\r\n\r\nCNode 社区由一批热爱 Node.js 技术的工程师发起，目前已经吸引了互联网各个公司的专业技术人员加入，我们非常欢迎更多对 Node.js 感兴趣的朋友。\r\n\r\nCNode 的 SLA 保证是，一个9，即 90.000000%。\r\n\r\n社区目前由 @alsotang 在维护，有问题请联系：https://github.com/alsotang\r\n\r\n请关注我们的官方微博：http://weibo.com/cnodejs\r\n\r\n移动客户端\r\n\r\n客户端由 @soliury 开发维护。\r\n\r\n源码地址： https://github.com/soliury/noder-react-native 。\r\n\r\n立即体验 CNode 客户端，直接扫描页面右侧二维码。\r\n\r\n另，安卓用户同时可选择：https://github.com/TakWolf/CNode-Material-Design ，这是 Java 原生开发的安卓客户端。\r\n\r\n友情链接\r\n\r\n              \r\nLOGO\r\n\r\n白底： /public/images/cnodejs.svg\r\n\r\n黑底： /public/images/cnodejs_light.svg', '2016-10-27 15:06:46');

-- ----------------------------
-- Table structure for tags
-- ----------------------------
DROP TABLE IF EXISTS `tags`;
CREATE TABLE `tags` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(96) DEFAULT NULL COMMENT '标签',
  `keyname` varchar(32) DEFAULT NULL,
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`),
  UNIQUE KEY `a` (`keyname`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tags
-- ----------------------------
INSERT INTO `tags` VALUES ('1', '精华', 'good', '2016-10-27 16:35:21');
INSERT INTO `tags` VALUES ('2', '分享', 'share', '2016-10-27 16:35:35');
INSERT INTO `tags` VALUES ('3', '问答', 'ask', '2016-10-27 16:35:50');
INSERT INTO `tags` VALUES ('4', '招聘', 'job', '2016-10-27 16:36:02');

-- ----------------------------
-- Table structure for topics
-- ----------------------------
DROP TABLE IF EXISTS `topics`;
CREATE TABLE `topics` (
  `id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `owner_users_id` bigint(20) unsigned DEFAULT NULL COMMENT '作者',
  `last_posts_id` bigint(20) unsigned DEFAULT NULL,
  `title` varchar(255) DEFAULT NULL COMMENT '标题',
  `count_views` int(10) unsigned DEFAULT NULL COMMENT '浏览数',
  `count_posts` int(11) DEFAULT NULL COMMENT '回复数',
  `top` bigint(20) unsigned DEFAULT NULL COMMENT '排序(置顶)',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  `update_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_topics_users_1` (`owner_users_id`) USING BTREE,
  KEY `fk_topics_users_2` (`last_posts_id`),
  CONSTRAINT `fk_topics_posts_1` FOREIGN KEY (`last_posts_id`) REFERENCES `posts` (`id`),
  CONSTRAINT `fk_topics_users_1` FOREIGN KEY (`owner_users_id`) REFERENCES `users` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of topics
-- ----------------------------
INSERT INTO `topics` VALUES ('1', '1', null, 'aaaaaaaaaaaaaaaaaaaaa', '4037', '3', '0', '2016-10-27 17:01:48', null);
INSERT INTO `topics` VALUES ('2', '1', null, 'ccccccccccccccccccccccccccccccccccccccc', '1', '0', '0', '2016-10-27 19:34:15', null);
INSERT INTO `topics` VALUES ('3', '1', null, 'webpack + vue 开发涉及到的包', '1', '0', '0', '2016-10-27 19:38:30', null);
INSERT INTO `topics` VALUES ('4', '1', null, 'heroku线上部署node的项目时出现问题', '1', '0', '0', '2016-10-27 19:39:46', null);
INSERT INTO `topics` VALUES ('5', '1', null, '使用vue+vuex+vue-router+webpack重构了cnode社区', '1', '0', '0', '2016-10-27 19:40:11', null);
INSERT INTO `topics` VALUES ('6', '1', null, '［ 北京］11月6日 NodeParty@科技寺，报名从速 !', '0', '0', '0', '2016-10-27 19:41:24', null);
INSERT INTO `topics` VALUES ('7', '1', null, 'bignum打印的问题，求个思路', '0', '0', '0', '2016-10-27 19:41:41', null);
INSERT INTO `topics` VALUES ('8', '1', null, '用cluster的master可以共享多个worker的全局数据吗？', '0', '0', '0', '2016-10-27 19:42:00', null);
INSERT INTO `topics` VALUES ('9', '1', null, '汉字如何用2个字节表示', '0', '0', '0', '2016-10-27 19:42:16', null);
INSERT INTO `topics` VALUES ('10', '1', null, '新人做了一个cnodejs的桌面客户端，欢迎大家指点', '0', '0', '0', '2016-10-27 19:42:50', null);
INSERT INTO `topics` VALUES ('11', '1', null, '目前行情中,作为后端服务器语言,到底node好还是java好?', '0', '0', '0', '2016-10-27 19:43:05', null);
INSERT INTO `topics` VALUES ('12', '1', null, '解析中国身份证号码（地址，性别，生日，星座），支持 18 位和 15 位身份证', '0', '0', '0', '2016-10-27 19:43:21', null);
INSERT INTO `topics` VALUES ('13', '1', null, '不用模板引擎，可以直接使用bootstrap前端html模板么', '0', '0', '0', '2016-10-27 19:44:18', null);
INSERT INTO `topics` VALUES ('14', '1', null, 'js中成员函数如何调用自身', '0', '0', '0', '2016-10-27 19:44:40', null);
INSERT INTO `topics` VALUES ('15', '1', null, 'node.js + rabbitmq，消费者等待生产者结束之后再消费？', '2', '1', '0', '2016-10-27 19:44:55', null);
INSERT INTO `topics` VALUES ('16', '1', null, '小问题解决的改进 《第15章 增加转载功能和转载统计》 @nswbmw', '1', '0', '0', '2016-10-27 19:45:13', null);

-- ----------------------------
-- Table structure for topics_tags
-- ----------------------------
DROP TABLE IF EXISTS `topics_tags`;
CREATE TABLE `topics_tags` (
  `topics_id` bigint(20) unsigned NOT NULL,
  `tags_id` int(10) unsigned NOT NULL,
  PRIMARY KEY (`topics_id`,`tags_id`),
  KEY `fk_topics_tags_tags_1` (`tags_id`),
  CONSTRAINT `fk_topics_tags_topics_1` FOREIGN KEY (`topics_id`) REFERENCES `topics` (`id`),
  CONSTRAINT `fk_topics_tags_tags_1` FOREIGN KEY (`tags_id`) REFERENCES `tags` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of topics_tags
-- ----------------------------
INSERT INTO `topics_tags` VALUES ('1', '2');
INSERT INTO `topics_tags` VALUES ('3', '2');
INSERT INTO `topics_tags` VALUES ('5', '2');
INSERT INTO `topics_tags` VALUES ('6', '2');
INSERT INTO `topics_tags` VALUES ('10', '2');
INSERT INTO `topics_tags` VALUES ('12', '2');
INSERT INTO `topics_tags` VALUES ('16', '2');
INSERT INTO `topics_tags` VALUES ('4', '3');
INSERT INTO `topics_tags` VALUES ('7', '3');
INSERT INTO `topics_tags` VALUES ('8', '3');
INSERT INTO `topics_tags` VALUES ('9', '3');
INSERT INTO `topics_tags` VALUES ('11', '3');
INSERT INTO `topics_tags` VALUES ('13', '3');
INSERT INTO `topics_tags` VALUES ('14', '3');
INSERT INTO `topics_tags` VALUES ('15', '3');
INSERT INTO `topics_tags` VALUES ('2', '4');

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `username` varchar(64) DEFAULT NULL COMMENT '用户名',
  `email` varchar(64) DEFAULT NULL COMMENT '电子邮件',
  `password` varchar(32) DEFAULT NULL,
  `point` int(10) unsigned DEFAULT NULL COMMENT '积分',
  `website` varchar(96) DEFAULT NULL COMMENT '个人网站',
  `location` varchar(64) DEFAULT NULL COMMENT '所在地点',
  `weibo` varchar(96) DEFAULT NULL COMMENT '微博',
  `github` varchar(96) DEFAULT NULL COMMENT 'GitHub',
  `sign` varchar(255) DEFAULT NULL COMMENT '个性签名',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`),
  UNIQUE KEY `a` (`username`),
  UNIQUE KEY `b` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of users
-- ----------------------------
INSERT INTO `users` VALUES ('1', '2881099', '2881099@qq.com', null, '0', null, '广州', null, 'https://github.com/2881099', '111', '2016-10-27 16:51:03');

-- ----------------------------
-- Table structure for users_topics
-- ----------------------------
DROP TABLE IF EXISTS `users_topics`;
CREATE TABLE `users_topics` (
  `users_id` bigint(20) unsigned NOT NULL,
  `topics_id` bigint(20) unsigned NOT NULL,
  PRIMARY KEY (`users_id`,`topics_id`),
  KEY `fk_users_topics_topics_1` (`topics_id`),
  CONSTRAINT `fk_users_topics_topics_1` FOREIGN KEY (`topics_id`) REFERENCES `topics` (`id`),
  CONSTRAINT `fk_users_topics_users_1` FOREIGN KEY (`users_id`) REFERENCES `users` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of users_topics
-- ----------------------------
