using BBSSite.Models;
using BBSSite.ViewModels;
using MyPublic;
using Web.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BBSSite.MyPublic;
using System.Linq.Expressions;

namespace BBSSite.Controllers
{
    /// <summary>
    /// Home控制器 用于加载前台帖子信息控制器
    /// </summary>
    public class HomeController : Controller
    {
        public HomeController()
        { }
        /// <summary>
        /// Index action,用于读取首页内容
        /// </summary>
        /// <returns>返回Index视图并传入IList<ForumAreaJoinForumClassifyEntity>模型实例数据</returns>
        [HttpGet]
        public ActionResult Index()
        {
            PublicFunctions.SetUrls(ViewBag, Url);//构造资源路径(js、css、image等)            
            IList<ForumAreaJoinForumClassifyEntity> ModelList = null;//要返回的数据模型            
            using (DB_BBSEntities db = new DB_BBSEntities())//构造数据库上下文
            {
                //查询tb_ForumArea表数据并绑定到ForumAreaJoinForumClassifyEntity自定义实体类
                ModelList = db.tb_ForumArea.Select(S => new ForumAreaJoinForumClassifyEntity
                {
                    ID = S.ID,//赋值ID
                    AreaName = S.AreaName,//赋值大板块区域名称
                    //查询该大板块区域下的所有子板块并绑定到ChildForumClassify自定义实体类，赋值给ChildForumClassify集合
                    ChildForumClassify = S.tb_ForumClassify.Select(S1 => new ChildForumClassify
                    {
                        Classifys = S1,//赋值子版块
                        ForumMain = S1.tb_ForumMain//查询该子版块推荐帖子并且是未删除状态的帖子数据，获取方式按照ID将排序
                        .Where(W2 => W2.IsRecommend == true && W2.Isdelete == false)
                        .OrderByDescending(O => O.ID).Take(3).ToList()
                    }).ToList()
                }).ToList();
            }
            //返回视图，传入包含大板块区域、子版块区域、推荐帖子的集合模型对象
            return View(ModelList);
        }
        /// <summary>
        /// MainContent action,用于读取主贴列表
        /// </summary>
        /// <param name="id">所属专区ID</param>
        /// <param name="CurrentPageindex">分页页码</param>
        /// <returns>根据传入的强类型ForumClassifyJoinForumMainEntity实例模型,返回MainContent视图</returns>
        [HttpGet]
        public ActionResult MainContent(int id = 0, int CurrentPageindex = 1)
        {
            if (id == 0)//判断子专区id值是否为0
            {
                return Goto("Index", "Home");//执行跳转到上一次访问的动作，或跳转到指定的动作
            }
            bool IsLimit = false;//定义权限变量
            ViewBag.IsLimit = false;//将权限值赋予动态类型，用于视图中的访问
            //实例化用户登录状态类
            BBSSite.MyPublic.LoginStatus IStatus = new BBSSite.MyPublic.LoginStatus();
            if (IStatus.IsLogin)//判断用户是否登录
            {
                tb_ForumClassify ForumClassify = null;//定义子专区数据类
                using (DB_BBSEntities db = new DB_BBSEntities())//实例化数据库上下文类
                {
                    //按照子专区id值查询该专区的其他信息
                    ForumClassify = db.tb_ForumClassify.Where(W => W.ID == id).FirstOrDefault();
                    if (ForumClassify != null)//如果查询数据不为空
                    {
                        //获取该专区的所属用户id，改用户可对该专区的帖子列表有执行操作权限
                        int ForumClassifyUserID = ForumClassify.ForumUserID;
                        //取出该子专区所属大板块专区的信息数据
                        tb_ForumArea ForumArea = db.tb_ForumArea.Where(W => W.ID == ForumClassify.ForumAreaID).FirstOrDefault();
                        //取出大板块专区的所属用户id
                        int ForumAreaUserID = ForumArea.UserID;
                        //如果当前登录用户与拥有子专区或大板块专区权限的用户相同
                        if (IStatus.LoginStatusEntity.ID == ForumClassifyUserID || IStatus.LoginStatusEntity.ID == ForumAreaUserID)
                        {
                            ViewBag.IsLimit = true;//该用户有执行操作权限
                            IsLimit = true;
                        }
                    }
                }
            }
            PublicFunctions.SetUrls(ViewBag, Url);//构造资源路径(js、css、image等)            
            const int PageSize = 20, PageCount = 5;//定义每页显示数据总数及最多显示的页码
            //构造分页对象配置类
            ConfigPaging cp = new ConfigPaging(CurrentPageindex, PageSize, PageCount);
            ForumClassifyJoinForumMainEntity Model = null;//要返回的数据模型            
            using (DB_BBSEntities db = new DB_BBSEntities())//实例化数据库上下文类
            {
                //按条件查询子专区表以及所属主贴表数据
                Model = db.tb_ForumClassify.Where(W => W.ID == id).Select(S => new ForumClassifyJoinForumMainEntity
                {
                    ID = S.ID,
                    ClassifyName = S.ClassifyName,
                    ClassifyInnerLogo = S.ClassifyInnerLogo,
                    UsersByBanzhu = S.tb_UsersByCustomer,
                    ForumMain = (ICollection<tb_ForumMain>)S.tb_ForumMain.Where(Where => Where.Isdelete == false && ((!IsLimit && Where.IsExamine == 1) || IsLimit)).OrderByDescending(O => O.ID).Skip(cp.StartRow).Take(PageSize)
                }).FirstOrDefault();
                //以下为与ForumMain(帖子)表对应的外键表信息
                Model.ReplyNumber = Model.ForumMain.Select(S => S.tb_ForumInfoStatus.Where(W => W.ForumMainID == S.ID).First().ReplyNumber).ToList();//统计回复次数
                Model.SeeNumber = Model.ForumMain.Select(S => S.tb_ForumInfoStatus.Where(W => W.ForumMainID == S.ID).First().SeeNumber).ToList();//统计查看次数
                Model.LastReplyUser = Model.ForumMain.Select(S => S.tb_ForumInfoStatus.Where(W => W.ForumMainID == S.ID).First().tb_UsersByCustomer.UserName).ToList();//最后回复人
                Model.UsersByCustomer = Model.ForumMain.Select(S => S.tb_UsersByCustomer).ToList();//发帖人
                Model.ImgUrl = Model.ForumMain.Select(S => (S.IsRecommend ? "pin_1.gif" : "folder_new.gif")).ToList();//推荐帖与普通帖logo
                Model.FMType = Model.ForumMain.Select(S => (S.IsRecommend ? "日月精华" : "最新帖子")).ToList();//推荐贴与普通帖提示标题
                //以下为总的统计数据
                Model.TotalForumCount = db.tb_ForumMain.Count(W => W.Isdelete == false && W.ForumClassifyID == id);//总帖子数
                Model.TotalReplyCount = db.tb_ForumMain.Where(W => W.Isdelete == false && W.ForumClassifyID == id).ToList().Aggregate(0, (count, current) => count + current.tb_ForumInfoStatus.Sum(S => S.ReplyNumber));//总回复数
                Model.TotalSeeCount = db.tb_ForumMain.Where(W => W.Isdelete == false && W.ForumClassifyID == id).ToList().Aggregate(0, (count, current) => count + current.tb_ForumInfoStatus.Sum(S => S.SeeNumber));//只查看数
            }
            cp.GetPaging(ViewBag, Model.TotalForumCount);//绑定分页数据            
            ViewBag.curid = id;//此id为传入的所属专区ID，将在下次分页时带入            
            return View(Model);//返回视图
        }
        /// <summary>
        /// SecondContent action,用于读取贴子详细数据
        /// </summary>
        /// <param name="id">主贴ID</param>
        /// <param name="CurrentPageindex">分页页码</param>
        /// <returns>根据传入的强类型ForumMainJoinForumSecondEntity实例模型,返回SecondContent视图</returns>
        public ActionResult SecondContent(int id = 0, int CurrentPageindex = 1)
        {
            if (id == 0) { return Goto("Index", "Home"); }//执行跳转到上一次访问的动作，或跳转到指定的动作            
            PublicFunctions.SetUrls(ViewBag, Url);//构造资源路径(js、css、image等)            
            const int PageSize = 10, PageCount = 5;//定义每页显示数据总数及最多显示的页码            
            ConfigPaging cp = new ConfigPaging(CurrentPageindex, PageSize, PageCount);//构造分页对象配置类            
            ForumMainJoinForumSecondEntity Model = null;//要返回的数据模型            
            using (DB_BBSEntities db = new DB_BBSEntities())//构造数据库上下文类
            {
                //根据条件,查询主贴表数据
                Model = db.tb_ForumMain.Where(W => W.ID == id && W.Isdelete == false).Select(S => new ForumMainJoinForumSecondEntity
                {
                    ForumMain = S,//主贴信息
                    ForumClassify = S.tb_ForumClassify,//所属子专区信息
                    UsersByCustomer = S.tb_UsersByCustomer,//发帖人信息
                    ZY_Sex = S.tb_UsersByCustomer.tb_ZY_Sex,//发帖人性别（读取资源表）
                    ForumSecondCount = S.tb_ForumSecond.Count(C => C.IsDelete == false),//总回复数
                    //查询该贴的跟帖数据集合，并使用分页进行查询
                    ForumSecond = S.tb_ForumSecond.Where(W1 => W1.IsDelete == false && W1.CurSequence > 0).OrderBy(O => O.CurSequence).Skip(cp.StartRow).Take(PageSize).Select(S1 =>
                        new ChildForumSecondByUsersByCustomer
                        {
                            ForumSecond = S1,//跟帖数据
                            UsersByCustomer = S1.tb_UsersByCustomer,//回复人
                            ZY_Sex = S1.tb_UsersByCustomer.tb_ZY_Sex//回复人性别（读取资源表）
                        }).ToList()
                }).FirstOrDefault();
            }            
            cp.GetPaging(ViewBag, Model.ForumSecondCount);//绑定分页数据            
            ViewBag.curid = id;//此id为传入的所属专区ID，将在下次分页时传回
            return View(Model);//返回视图
        }
        /// <summary>
        /// Recommend action,用于读取精华贴数据
        /// </summary>
        /// <param name="CurrentPageindex">分页页码</param>
        /// <returns>根据传入的强类型ForumMainByRecommendEntity实例模型,返回Recommend视图</returns>
        public ActionResult Recommend(int CurrentPageindex = 1)
        {            
            PublicFunctions.SetUrls(ViewBag, Url);//构造资源路径(js、css、image等)            
            const int PageSize = 20, PageCount = 5;//定义每页显示数据总数及最多显示的页码
            //构造分页对象配置类
            ConfigPaging cp = new ConfigPaging(CurrentPageindex, PageSize, PageCount);
            //要返回的数据模型
            ForumMainByRecommendEntity Model = new ForumMainByRecommendEntity();            
            using (DB_BBSEntities db = new DB_BBSEntities())//构造数据库上下文
            {
                //查询标记为精华帖的列表内容
                Model.ForumMain = db.tb_ForumMain.Where(W => W.IsRecommend == true && W.Isdelete == false)
                    .OrderByDescending(O => O.ID).Skip(cp.StartRow).Take(PageSize).ToList();
                //查询发帖人
                Model.UsersByCustomer = Model.ForumMain.Select(S => S.tb_UsersByCustomer).ToList();
                //查询回复次数列表内容
                Model.ReplyNumber = Model.ForumMain.Select(S => S.tb_ForumInfoStatus.Where(W => W.ForumMainID == S.ID).First().ReplyNumber).ToList();
                //查询查看次数列表内容
                Model.SeeNumber = Model.ForumMain.Select(S => S.tb_ForumInfoStatus.Where(W => W.ForumMainID == S.ID).First().SeeNumber).ToList();
                //查询最后回复人列表内容
                Model.LastReplyUser = Model.ForumMain.Select(S => S.tb_ForumInfoStatus.Where(W => W.ForumMainID == S.ID).First().tb_UsersByCustomer.UserName).ToList();
                //统计精华帖总数
                Model.ForumMainCount = db.tb_ForumMain.Count(C => C.IsRecommend == true && C.Isdelete == false);
            }            
            cp.GetPaging(ViewBag, Model.ForumMainCount);//绑定分页数据
            return View(Model);//返回视图
        }
        public ActionResult Search(string text)
        {
            //构造资源路径(js、css、image等)
            ViewBag.text = text;
            PublicFunctions.SetUrls(ViewBag, Url);
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                //tb_ForumArea
                //tb_ForumClassify
                //tb_ForumMain
                //tb_ForumSecond
                List<tb_ForumMain> ForumMains = db.tb_ForumMain.Where(W => W.Content.Contains(text)).ToList();
                List<tb_ForumSecond> ForumSeconds = db.tb_ForumSecond.Where(W => W.Content.Contains(text)).ToList();
                List<SearchEntity> SearchEntitys = new List<SearchEntity>();
                foreach (tb_ForumMain FM in ForumMains)
                {
                    SearchEntitys.Add(new SearchEntity()
                    {
                        ID = FM.ID,
                        Title = FM.Title.Replace(text, "<em>" + text + "</em>"),
                        Content = FM.Content.Replace(text, "<em>" + text + "</em>")
                    });
                }
                foreach (tb_ForumSecond FS in ForumSeconds)
                {
                    SearchEntitys.Add(new SearchEntity()
                    {
                        ID = FS.tb_ForumMain.ID,
                        Title = FS.tb_ForumMain.Title.Replace(text, "<em>" + text + "</em>"),
                        Content = FS.Content.Replace(text, "<em>" + text + "</em>")
                    });
                }
                return View(SearchEntitys);
            }
            return View();
        }
        [HttpPost]
        public JsonResult SettingRecommend(FormCollection fc)
        {
            BBSSite.MyPublic.LoginStatus IStatus = new BBSSite.MyPublic.LoginStatus();
            if (IStatus.IsLogin)
            {
                int FMID;
                bool IsRecommend;
                if (int.TryParse(fc["FMID"], out FMID) && bool.TryParse(fc["IsRecommend"], out IsRecommend))
                {
                    using (DB_BBSEntities db = new DB_BBSEntities())
                    {
                        tb_ForumMain fm = new tb_ForumMain() { ID = FMID };
                        tb_ForumMain updateForumMain = db.tb_ForumMain.Attach(fm);
                        if (!IsRecommend)
                        {
                            updateForumMain.IsRecommend = true;
                        }
                        else
                        {
                            updateForumMain.IsRecommend = false;
                        }
                        db.Entry(updateForumMain).Property(P => P.IsRecommend).IsModified = true;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        bool IsUpdate = db.SaveChanges() > 0;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        if (IsUpdate)
                        {
                            return Json(new { ResultStatus = 1, ResultMsg = "操作成功!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    return Json(new { ResultStatus = 0, ResultMsg = "操作失败!" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { ResultStatus = -1, ResultMsg = "参数验证失败!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ResultStatus = -2, ResultMsg = "未登录!" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SettingExamine(FormCollection fc)
        {
            BBSSite.MyPublic.LoginStatus IStatus = new BBSSite.MyPublic.LoginStatus();
            if (IStatus.IsLogin)
            {
                int FMID;
                if (int.TryParse(fc["FMID"], out FMID))
                {
                    using (DB_BBSEntities db = new DB_BBSEntities())
                    {
                        tb_ForumMain fm = new tb_ForumMain() { ID = FMID };
                        tb_ForumMain updateForumMain = db.tb_ForumMain.Attach(fm);
                        updateForumMain.IsExamine = 1;
                        db.Entry(updateForumMain).Property(P => P.IsExamine).IsModified = true;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        bool IsUpdate = db.SaveChanges() > 0;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        if (IsUpdate)
                        {
                            return Json(new { ResultStatus = 1, ResultMsg = "操作成功!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    return Json(new { ResultStatus = 0, ResultMsg = "操作失败!" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { ResultStatus = -1, ResultMsg = "参数验证失败!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ResultStatus = -2, ResultMsg = "未登录!" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delrecord(FormCollection fc)
        {
            BBSSite.MyPublic.LoginStatus IStatus = new BBSSite.MyPublic.LoginStatus();
            if (IStatus.IsLogin)
            {
                int FMID;
                if (int.TryParse(fc["FMID"], out FMID))
                {
                    using (DB_BBSEntities db = new DB_BBSEntities())
                    {
                        tb_ForumMain fm = new tb_ForumMain() { ID = FMID };
                        tb_ForumMain updateForumMain = db.tb_ForumMain.Attach(fm);
                        updateForumMain.Isdelete = true;
                        db.Entry(updateForumMain).Property(P => P.Isdelete).IsModified = true;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        bool IsUpdate = db.SaveChanges() > 0;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        if (IsUpdate)
                        {
                            return Json(new { ResultStatus = 1, ResultMsg = "操作成功!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    return Json(new { ResultStatus = 0, ResultMsg = "操作失败!" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { ResultStatus = -1, ResultMsg = "参数验证失败!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ResultStatus = -2, ResultMsg = "未登录!" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 返回上一次访问的页面(如果存在),或跳转到首页
        /// </summary>
        /// <param name="Action">如果无法检测上一次路径时要跳转的Action方法</param>
        /// <param name="Controller">如果无法检测上一次路径时要跳转的控制器</param>
        /// <returns>返回即将要跳转的地址</returns>
        public ActionResult Goto(string Action, string Controller)
        {
            //此处判断如果请求不带有id值,那么将跳转回上一次访问的页面(如果存在),或跳转到首页
            return PublicFunctions.ToRedirect(this, null, null, (Url) =>
            {
                //跳转到上一次访问的页面
                return Redirect(Url);
            },
            (Url) =>
            {
                //跳转到首页
                return RedirectToAction(Action, Controller);
            });
        }
    }
    /// <summary>
    /// 配置分页数据类
    /// </summary>
    public class ConfigPaging
    {
        int CurrentPageindex;
        int PageSize;
        int PageCount;
        /// <summary>
        /// 构造基本分页数据
        /// </summary>
        /// <param name="CurrentPageindex">当前页码</param>
        /// <param name="PageSize">每页显示的总条数</param>
        /// <param name="PageCount">最多显示的页码</param>
        public ConfigPaging(int CurrentPageindex, int PageSize, int PageCount)
        {
            this.CurrentPageindex = CurrentPageindex;
            this.PageSize = PageSize;
            this.PageCount = PageCount;
        }
        /// <summary>
        /// 读取数据起始行
        /// </summary>
        public int StartRow
        {
            get { return (this.CurrentPageindex - 1) * this.PageSize; }
        }
        /// <summary>
        /// 开始构造分页对象
        /// </summary>
        /// <param name="ViewBag">向视图传入的解析后的分页数据</param>
        /// <param name="TotalForumCount">数据总记录</param>
        public void GetPaging(dynamic ViewBag, int TotalForumCount)
        {
            //构造分页对象
            APaging ap = new Paging();
            //DefaultPaging为默认的标签设置及样式
            IBinding Binding = ap.Set(new DefaultPaging(), CurrentPageindex, TotalForumCount, this.PageSize, this.PageCount);
            //一下调用顺序将影响分页按钮显示顺序
            Binding.GetShowChar();
            Binding.GetFrst();
            Binding.GetFront();
            Binding.GetPageNumber();
            Binding.GetAfter();
            Binding.GetLast();
            Binding.GetInputPageNumber();
            Binding.GetGotoPageIndex();
            ViewBag.Paging = ap.Bind();
        }
    }
}