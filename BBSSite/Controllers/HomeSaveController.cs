using BBSSite.Models;
using BBSSite.MyPublic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BBSSite.Controllers
{
    /// <summary>
    /// HomeSave控制器 用于处理前台保存数据控制器
    /// </summary>
    public class HomeSaveController : Controller
    {
        // GET: HomeSave
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 发布新帖时接受处理方法
        /// </summary>
        /// <param name="FC">提交的Form表单</param>
        /// <returns>返回原页面(如果存在)或返回首页</returns>
        public ActionResult PulishNewContent(FormCollection FC)
        {
            //接受并验证Form表单提交过来的字段值
            int ForumClassifyID, IntIsRecommend;
            bool IsRecommend;
            string Title, Content;
            if (int.TryParse(FC["ForumClassifyID"], out ForumClassifyID) && ForumClassifyID > 0
                && (Title = FC["mainTitle"]) != null && (Content = FC["content"]) != null && Title != "" && Content != "")
            {
                int.TryParse(FC["IsRecommend"], out IntIsRecommend);
                IsRecommend = Convert.ToBoolean(IntIsRecommend);
                //Base64解码发布的内容
                Content = Encoding.UTF8.GetString(Convert.FromBase64String(Content));
                //查找出该贴所属的大专区板块
                int ForumAreaID = 0;
                using (DB_BBSEntities db = new DB_BBSEntities())
                {
                    ForumAreaID = db.tb_ForumClassify.Where(W => W.ID == ForumClassifyID).Select(S => S.ForumAreaID).First();
                }
                if (ForumAreaID > 0)
                {
                    //取出当前用户ID
                    int CurrentUserID = new LoginStatus().LoginStatusEntity.ID;
                    tb_ForumMain ForumMain = new tb_ForumMain();
                    ForumMain.Title = Title;
                    ForumMain.ForumAreaID = ForumAreaID;
                    ForumMain.ForumClassifyID = ForumClassifyID;
                    ForumMain.CreateUserID = CurrentUserID;
                    ForumMain.CreateTime = DateTime.Now;
                    ForumMain.Content = Content;
                    ForumMain.IsRecommend = IsRecommend;
                    ForumMain.IsExamine = 0;
                    ForumMain.Isdelete = false;
                    ForumMain.Zan = 0;
                    //创建一条与该贴对应的帖子状态表数据
                    ForumMain.tb_ForumInfoStatus = new List<tb_ForumInfoStatus>
                    {
                        new tb_ForumInfoStatus
                        {
                            ForumMainID=ForumMain.ID,
                            ReplyNumber = 0,
                            SeeNumber = 0,
                            LastReplyUserID = CurrentUserID,
                            LastReplytime = DateTime.Now
                        }
                    };
                    //保存数据到数据库
                    using (DB_BBSEntities db = new DB_BBSEntities())
                    {
                        db.tb_ForumMain.Add(ForumMain);
                        if (db.SaveChanges() > 0)
                        {
                            //如果保存成功返回原页面
                            return Redirect(Request.UrlReferrer.AbsolutePath);
                        }
                    }
                }
            }
            //在保存失败或者参数验证未通过时,返回原页面(如果存在)或返回首页,并发送失败消息
            return PublicFunctions.ToRedirect(this, "PulishNewContentError", "未能成功发表帖子,请检查输入信息!", (Url) =>
            {
                return Redirect(Url);
            },
            (Url) =>
            {
                return RedirectToAction("Index", "Home");
            });
        }
        /// <summary>
        /// 回复帖子时接受处理方法
        /// </summary>
        /// <param name="FC">提交的Form表单</param>
        /// <returns>返回原页面(如果存在)或返回首页</returns>
        public ActionResult ReplyContent(FormCollection FC)
        {
            int ForumMainID = 0;//定义主贴id变量
            string Content;//定义回复内容变量
            //接受并验证Form表单提交过来的字段值
            if (int.TryParse(FC["curid"], out ForumMainID) && ForumMainID > 0 && (Content = FC["content"]) != null && Content != "")
            {
                int CurSequence = 0, ReplySequenceID;//定义楼层变量和回复楼层的id                
                int.TryParse(FC["ReplySequenceID"], out ReplySequenceID);//被回复人的帖子ID
                if (ReplySequenceID == 0)//如果该值为0则代表当前回复的是主贴
                {
                    CurSequence = 1;//回复主贴时，查找最大的楼层数并且加1就为该贴的楼层，否则为1
                    using (DB_BBSEntities db = new DB_BBSEntities())//实例化数据库上下文类
                    {
                        //查找该贴的所有回复
                        IQueryable<tb_ForumSecond> Where = db.tb_ForumSecond.Where(W => W.ForumMainID == ForumMainID && W.IsDelete == false);
                        if (Where.Any())//如果能够找到回复信息
                        {
                            CurSequence = Where.Max(S => S.CurSequence);//取出最大楼层数
                            CurSequence++;//最大楼层数加1，则为当前回复贴的楼层
                        }
                    }
                    ReplySequenceID = ForumMainID;//在回复主贴时,回复楼层id值应为主贴ID
                }
                //取出当前用户ID
                int CurrentUserID = new LoginStatus().LoginStatusEntity.ID;
                //创建回复数据实体并赋值
                tb_ForumSecond ForumSecond = new Models.tb_ForumSecond();
                ForumSecond.ForumMainID = ForumMainID;
                ForumSecond.Content = Encoding.UTF8.GetString(Convert.FromBase64String(Content));
                ForumSecond.CreateUserID = CurrentUserID;
                ForumSecond.CreateTime = DateTime.Now;
                ForumSecond.CurSequence = CurSequence;
                ForumSecond.ReplySequenceID = ReplySequenceID;
                ForumSecond.IsDelete = false;
                //保存数据
                using (DB_BBSEntities db = new DB_BBSEntities())
                {
                    db.tb_ForumSecond.Add(ForumSecond);
                    if (db.SaveChanges() > 0)
                    {
                        //如果保存成功返回原页面
                        return Redirect(Request.UrlReferrer.AbsolutePath);
                    }
                }
            }
            //在保存失败或者参数验证未通过时,返回原页面(如果存在)或返回首页,并发送失败消息
            return PublicFunctions.ToRedirect(this, "ReplyContentError", "未能成功回复帖子,请检查输入信息!",
            (Url) => { return Redirect(Url); },
            (Url) => { return RedirectToAction("Index", "Home"); });
        }
        /// <summary>
        /// 举报帖子时接受处理方法
        /// </summary>
        /// <param name="FC">提交的Form表单</param>
        /// <returns>返回原页面(如果存在)或返回首页</returns>
        public string SubWarningInfo(FormCollection FC)
        {
            //接受并验证Form表单提交过来的字段值
            int RdoWarningType, ForumID, ForumTypeID;
            string WarningInfoText = FC["WarningInfoText"];
            if (int.TryParse(FC["RdoWarningType"], out RdoWarningType) && RdoWarningType > 0 &&
                int.TryParse(FC["ForumID"], out ForumID) && ForumID > 0 &&
                int.TryParse(FC["ForumTypeID"], out ForumTypeID) && ForumTypeID > 0 &&
                WarningInfoText != null)
            {
                //取出当前用户ID
                int CurrentUserID = new LoginStatus().LoginStatusEntity.ID;
                tb_ForumReport ForumReport = new tb_ForumReport();
                ForumReport.ForumID = ForumID;
                ForumReport.ReportForumTypeID = ForumTypeID;
                ForumReport.ReportTypeID = RdoWarningType;
                ForumReport.CreateUserID = CurrentUserID;
                ForumReport.CreateTime = DateTime.Now;
                ForumReport.ReportDetailContent = WarningInfoText;
                //保存数据到数据库,并返回Json数据,如果保存成功返回状态为1并提示成功消息,否则状态为0或1并提示失败消息
                using (DB_BBSEntities db = new DB_BBSEntities())
                {
                    db.tb_ForumReport.Add(ForumReport);
                    if (db.SaveChanges() > 0)
                    {
                        return PublicFunctions.ToJson(new HomeSaveResultJson { Status = 1, Content = "已提交举报信息,请等待审核!" });
                    }
                    return PublicFunctions.ToJson(new HomeSaveResultJson { Status = 0, Content = "未能提交举报信息,已失败!" });
                }
            }
            return PublicFunctions.ToJson(new HomeSaveResultJson { Status = -1, Content = "提交举报信息不全,请核查!" });
        }
    }
    /// <summary>
    /// 可以为HomeSave控制器提供的Json响应消息实体,该实体通过序列化字符串后返回
    /// </summary>
    [DataContract]
    public class HomeSaveResultJson
    {
        /// <summary>
        /// 操作状态
        /// </summary>
        [DataMember]
        public int Status;
        /// <summary>
        /// 提示消息
        /// </summary>
        [DataMember]
        public string Content;
    }
}