using BBSSite.Areas.Admins.ViewModels;
using BBSSite.Models;
using BBSSite.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace BBSSite.MyPublic
{
    /// <summary>
    /// 公共读取数据库表类
    /// </summary>
    public class PublicService
    {
        //递归时的索引值
        int Index = 0;
        //递归时的父贴列表
        List<List<ChildReplyEntity>> EachArray = new List<List<ChildReplyEntity>>();
        //返回已按时间排序的对话回复列表
        List<ChildReplyEntity> ReturnList = new List<ChildReplyEntity>();
        /// <summary>
        /// 读取主题回复的对话回复数据
        /// </summary>
        /// <param name="ForumSecondID">回复贴的ID</param>
        /// <param name="IsData">返回是否存在对话回复数据</param>
        /// <returns>返回对话回复数据列表</returns>
        public List<ChildReplyEntity> GetChildReply(int ForumSecondID, out bool IsData)
        {
            List<ChildReplyEntity> TempChildReplyEntity = new List<ChildReplyEntity>();
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                IQueryable<tb_ForumSecond> WhereForumSecond = db.tb_ForumSecond.Where(W => W.ReplySequenceID == ForumSecondID && W.IsDelete == false && W.CurSequence == 0);
                TempChildReplyEntity = WhereForumSecond.Select(S => new ChildReplyEntity { ForumSecond = S, UsersByCustomer = S.tb_UsersByCustomer, ByUsersByCustomer = null }).ToList();
                IsData = TempChildReplyEntity.Count > 0;
                EachArray.Add(TempChildReplyEntity);
            }
            While();
            foreach (var InEachArray in EachArray)
            {
                ReturnList.AddRange(InEachArray);
            }
            return ReturnList.OrderBy(O => O.ForumSecond.CreateTime).ToList();
        }
        /// <summary>
        /// 通过递归获取对话评论内容
        /// </summary>
        /// <returns>返回是否结束递归</returns>
        public bool While()
        {
            List<ChildReplyEntity> Es = EachArray[Index];
            List<ChildReplyEntity> TempChildReplyEntity = new List<ChildReplyEntity>();
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                foreach (ChildReplyEntity CE in Es)
                {
                    IQueryable<tb_ForumSecond> WhereForumSecond = db.tb_ForumSecond.Where(W => W.ReplySequenceID == CE.ForumSecond.ID && W.IsDelete == false && W.CurSequence == 0);
                    var list = db.tb_ForumSecond.Where(W => W.ReplySequenceID == CE.ForumSecond.ID && W.IsDelete == false && W.CurSequence == 0)
                        .Select(S => new ChildReplyEntity
                        {
                            ForumSecond = S,
                            UsersByCustomer = S.tb_UsersByCustomer
                        }).ToList().Select(S => new ChildReplyEntity { ForumSecond = S.ForumSecond, UsersByCustomer = S.UsersByCustomer, ByUsersByCustomer = CE.UsersByCustomer }).ToList();
                    TempChildReplyEntity.AddRange(list);
                }
            }
            if (TempChildReplyEntity == null || TempChildReplyEntity.Count == 0)
            {
                return true;
            }
            EachArray.Add(TempChildReplyEntity);
            Index++;
            return While();
        }
        /// <summary>
        /// 获取举报类型资源表tb_ZY_ReportType数据
        /// </summary>
        /// <returns>返回多种条件类型集合属性数据</returns>
        public ReportTypeEntity GetReportType()
        {
            ReportTypeEntity RTE = new ReportTypeEntity();
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                RTE.PersonalReportType = db.tb_ZY_ReportType.Where(W => W.ReportType == 1).ToList();
                RTE.GarbageReportType = db.tb_ZY_ReportType.Where(W => W.ReportType == 2).ToList();
            }
            return RTE;
        }
        /// <summary>
        /// 读取性别资源表tb_ZY_Sex数据,并赋值给页面SexID变量
        /// </summary>
        public static void RegistSexIDBind(dynamic ViewBag)
        {
            IEnumerable<SelectListItem> SexChoose = null;
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                SexChoose = db.tb_ZY_Sex.Select(S => new SelectListItem { Text = S.Content, Value = S.ID.ToString() }).ToList();
            }
            ViewBag.SexID = SexChoose;
        }
        /// <summary>
        /// 读取角色表tb_UserByRole数据,并赋值给页面RoleID变量
        /// </summary>
        /// <param name="ViewBag"></param>
        public static void GetRole(dynamic ViewBag, int SelectID)
        {
            IList<SelectListItem> RoleChoose = null;
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                RoleChoose = db.tb_UserByRole.Select(S => new SelectListItem { Text = S.RoleName, Value = S.ID.ToString(), Selected = (S.ID == SelectID ? true : false) }).ToList();
            }
            ViewBag.DropDownListRoleID = RoleChoose;
        }
        /// <summary>
        /// 读取栏目表tb_Column数据
        /// </summary>
        /// <returns>返回栏目表所有数据</returns>
        public static void GetColumn(dynamic ViewBag)
        {
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                ViewBag.Columns = db.tb_Column.ToList();
            }
        }
        /// <summary>
        /// 读取栏目表tb_Column数据
        /// </summary>
        /// <returns>返回栏目表一级栏目数据</returns>
        internal static void GetColumnOneGrade(dynamic ViewBag)
        {
            Expression<Func<tb_Column, bool>> Exp = where => where.ColumnCode.Length == 1;
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                ViewBag.Columns = db.tb_Column.Where(Exp.Compile()).ToList();
            }
        }
        public static tb_Column GetCurrentColumnCode(int ColumnGrade, string ChooseParentColumn)
        {
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                if (ColumnGrade == 1)
                {
                    Expression<Func<tb_Column, bool>> Exp = where => where.ColumnCode.Length == 1;
                    var list = db.tb_Column.Where(Exp.Compile()).OrderByDescending(O => O.ColumnCode).Take(1).ToList();
                    if (list != null && list.Count > 0)
                    {
                        return list.First();
                    }
                }
                else
                {
                    Expression<Func<tb_Column, bool>> Exp1 = where => where.ColumnCode.Length > 1 && where.ColumnCode.Substring(0, 1) == ChooseParentColumn;
                    var list = db.tb_Column.Where(Exp1.Compile()).OrderByDescending(O => O.ColumnCode).ToList();
                    if (list != null && list.Count > 0)
                    {
                        return list.First();
                    }
                }
            }
            return null;
        }
        public static void GetUsersByCustomer(dynamic ViewBag, int UserID)
        {
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                ViewBag.Users = db.tb_UsersByCustomer.Where(W => W.IsModerator).Select(S => new SelectListItem() { Text = S.UserName, Value = S.ID.ToString(), Selected = (S.ID == UserID ? true : false) }).ToList();
            }
        }
        public static IList<SP_Get_UsersByCustomer_NotTargetForumArea_Result> GetUsersByCustomer_NotTargetForumArea()
        {
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                ObjectResult<SP_Get_UsersByCustomer_NotTargetForumArea_Result> O = db.SP_Get_UsersByCustomer_NotTargetForumArea();
                return O.ToList();
            }
        }
        public static IList<SP_Get_UsersByCustomer_NotTargetForumClassify_Result> GetUsersByCustomer_NotTargetForumClassify()
        {
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                ObjectResult<SP_Get_UsersByCustomer_NotTargetForumClassify_Result> O = db.SP_Get_UsersByCustomer_NotTargetForumClassify();
                return O.ToList();
            }
        }
        //public static IList<RoleJoinColumnEntity> GettingColumn()
        //{
        //    int UserRoleID = new AdminLoginStatus().LoginStatusEntity.RoleID;
        //    using (DB_BBSEntities db = new DB_BBSEntities())
        //    {
        //        var ColumnRecord = from UserByRoleJoinColumn in db.tb_UserByRoleJoinColumn.DefaultIfEmpty()
        //                           join Column in db.tb_Column on UserByRoleJoinColumn.ColumnID equals Column.ID
        //                           where UserByRoleJoinColumn.RoleID == UserRoleID
        //                           select new RoleJoinColumnEntity { ColumnID = UserByRoleJoinColumn.ColumnID, ColumnCode = Column.ColumnCode, ColumnName = Column.ColumnName, Url = Column.Url };
        //        IList<RoleJoinColumnEntity> RoleJoinColumnEntitys = ColumnRecord.ToList();
        //        return RoleJoinColumnEntitys;
        //    }
        //}
        public static IList<RoleJoinColumnEntity> GettingColumn(string AbsoluteUri)
        {
            IList<RoleJoinColumnEntity> RJCE = null;
            Expression<Func<tb_Column, bool>> Exp = where => where.Url != null && where.Url.ToLower() == AbsoluteUri.ToLower();
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                tb_Column Column = db.tb_Column.SingleOrDefault(Exp.Compile());
                if (Column == null)
                {
                    if (MyCache.Current.Contains("SingleOrDefault"))
                    {
                        Column = MyCache.Current.Get<tb_Column>("SingleOrDefault");
                        AbsoluteUri = Column.Url;
                    }
                }
                string ColumnCode = "";
                if (Column != null)
                {
                    ColumnCode = Column.ColumnCode.Substring(0, 1);
                    if (!MyCache.Current.Contains("SingleOrDefault"))
                    {
                        MyCache.Current.Add(Column, "SingleOrDefault");
                    }
                }
                else
                {
                    ColumnCode = "A";
                }
                RJCE = db.tb_Column.Select(S => new RoleJoinColumnEntity
                {
                    ColumnID = S.ID,
                    ColumnCode = S.ColumnCode,
                    ColumnName = S.ColumnName,
                    Url = S.Url,
                    LogoClassName = S.LogoClassName,
                    IsClassA = S.ColumnCode.Length == 1,
                    IsSelectedA = (S.ColumnCode == ColumnCode),
                    IsSelected = S.Url.ToLower() == AbsoluteUri.ToLower(),
                    IsUrlNull = (S.Url == null || S.Url == "")
                }).ToList();
                return RJCE;
            }
        }
    }
}