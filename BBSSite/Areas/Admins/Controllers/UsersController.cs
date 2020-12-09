using BBSSite.Areas.Admins.ViewModels;
using BBSSite.Models;
using BBSSite.MyPublic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BBSSite.Areas.Admins.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            PublicFunctions.SetUrls(ViewBag, Url);
            return View();
        }
        [MyAuthorize(7)]
        public ActionResult CreateAdmins()
        {
            PublicFunctions.SetUrls(ViewBag, Url);
            PublicService.GetRole(ViewBag, 0);
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, MyAuthorize(7)]
        public ActionResult DoCreateAdmins(RegisAdminEntity UserEntity)
        {
            PublicFunctions.SetUrls(ViewBag, Url);
            //通过实体类的验证特性判断是否验证通过
            if (ModelState.IsValid)
            {
                //创建表实体数据对象
                tb_UsersBySystem ub = new tb_UsersBySystem()
                {
                    UserName = UserEntity.UserName,
                    UserPassword = Encoding.Unicode.GetBytes(PublicFunctions.MD5(UserEntity.UserPassword)),
                    NickName = UserEntity.NickName,
                    RoleID = UserEntity.RoleID,
                    Email = UserEntity.Email
                };
                //保存注册的用户信息
                using (DB_BBSEntities db = new DB_BBSEntities())
                {
                    db.tb_UsersBySystem.Add(ub);
                    if (db.SaveChanges() == 0)
                    {
                        ModelState.AddModelError("LoginError", "创建失败");
                        //绑定页面角色下拉框（如果注册失败）
                        PublicService.GetRole(ViewBag, 0);
                        return View("CreateAdmins");
                    }
                    else
                    {
                        //注册成功保存用户信息到session
                        MyPublic.ILoginStatus ILoginStatus = new MyPublic.AdminLoginStatus();
                        ILoginStatus.LoginSuccess(UserEntity.UserName, Session);
                        //返回列表页
                        return RedirectToAction("UserList", "Users");
                    }
                }
            }
            else
            {
                //绑定页面角色下拉框（如果参数验证失败）
                PublicService.GetRole(ViewBag, 0);
                return View("CreateAdmins");
            }
        }
        [MyAuthorize(7)]
        public ActionResult EditAdmins(int id)
        {
            if (id < 1)
            {
                return RedirectToAction("UserList", "Users");
            }
            tb_UsersBySystem UsersBySystem = null;
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                UsersBySystem = db.tb_UsersBySystem.Where(W => W.ID == id).FirstOrDefault();
            }
            if (UsersBySystem == null)
            {
                return RedirectToAction("UserList", "Users");
            }
            PublicService.GetRole(ViewBag, UsersBySystem.RoleID);
            PublicFunctions.SetUrls(ViewBag, Url);
            return View(new EditAdminEntity() { ID = UsersBySystem.ID, RoleID = UsersBySystem.RoleID, UserName = UsersBySystem.UserName, NickName = UsersBySystem.NickName, Email = UsersBySystem.Email });
        }
        [HttpPost, ValidateAntiForgeryToken, MyAuthorize(7)]
        public ActionResult DoEditAdmins(EditAdminEntity Entity)
        {
            if (ModelState.IsValid)
            {
                bool IsExists = true;
                using (DB_BBSEntities db = new DB_BBSEntities())
                {
                    IsExists = db.tb_UsersBySystem.Count(C => C.Email == Entity.Email && C.ID != Entity.ID) > 0;
                }
                if (IsExists)
                {
                    ModelState.AddModelError("InfoError", "参数验证失败");
                }
                else
                {
                    var UsersBySystem = new tb_UsersBySystem() { ID = Entity.ID };
                    using (DB_BBSEntities db = new DB_BBSEntities())
                    {
                        var updateUsersBySystem = db.tb_UsersBySystem.Attach(UsersBySystem);
                        updateUsersBySystem.NickName = Entity.NickName;
                        updateUsersBySystem.RoleID = Entity.RoleID;
                        updateUsersBySystem.Email = Entity.Email;

                        db.Entry(updateUsersBySystem).Property(T => T.NickName).IsModified = true;
                        db.Entry(updateUsersBySystem).Property(T => T.RoleID).IsModified = true;
                        db.Entry(updateUsersBySystem).Property(T => T.Email).IsModified = true;
                        db.Entry(updateUsersBySystem).Property(T => T.UserName).IsModified = false;
                        db.Entry(updateUsersBySystem).Property(T => T.UserPassword).IsModified = false;

                        db.Configuration.ValidateOnSaveEnabled = false;
                        bool isSuc = db.SaveChanges() > 0;
                        db.Configuration.ValidateOnSaveEnabled = true;
                        if (isSuc)
                        {
                            return RedirectToAction("UserList", "Users");
                        }
                    }
                    ModelState.AddModelError("InfoError", "信息修改失败");
                }
            }
            else
            {
                ModelState.AddModelError("InfoError", "参数验证失败");
            }
            tb_UsersBySystem UsersBySystem1 = null;
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                UsersBySystem1 = db.tb_UsersBySystem.Where(W => W.ID == Entity.ID).FirstOrDefault();
            }
            if (UsersBySystem1 == null)
            {
                return RedirectToAction("UserList", "Users");
            }
            PublicService.GetRole(ViewBag, Entity.RoleID);
            PublicFunctions.SetUrls(ViewBag, Url);
            return View("EditAdmins", new EditAdminEntity() { ID = UsersBySystem1.ID, RoleID = UsersBySystem1.RoleID, UserName = UsersBySystem1.UserName, NickName = UsersBySystem1.NickName, Email = UsersBySystem1.Email });
        }

        [HttpGet, MyAuthorize(10)]
        public ActionResult CreateRole()
        {
            PublicFunctions.SetUrls(ViewBag, Url);
            //绑定页面角色复选框
            PublicService.GetColumn(ViewBag);
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, MyAuthorize(10)]
        public ActionResult DoCreateRole(AddRoleEntity Entity, FormCollection fc)
        {
            PublicFunctions.SetUrls(ViewBag, Url);
            string Column = null;
            if (ModelState.IsValid && (Column = fc["Column"]) != null && Column != "")
            {
                IList<int> ColumnList = Column.Split(',').Select(S => Convert.ToInt32(S)).ToList();
                tb_UserByRole Roles = new tb_UserByRole()
                {
                    RoleName = Entity.RoleName
                };
                foreach (var ColumnID in ColumnList)
                {
                    Roles.tb_UserByRoleJoinColumn.Add(new tb_UserByRoleJoinColumn() { ColumnID = ColumnID });
                }
                using (DB_BBSEntities db = new DB_BBSEntities())
                {
                    db.tb_UserByRole.Add(Roles);
                    if (db.SaveChanges() > 0)
                    {
                        return RedirectToAction("RoleList", "Users");
                    }
                }
                ModelState.AddModelError("RoleError", "创建失败");
            }
            else
            {
                ModelState.AddModelError("RoleError", "参数验证失败");
            }
            //绑定页面角色复选框
            PublicService.GetColumn(ViewBag);
            return View("CreateRole");
        }

        [HttpGet, MyAuthorize(10)]
        public ActionResult EditRole(int id = 0)
        {
            if (id < 1)
            {
                return RedirectToAction("RoleList", "Users");
            }
            tb_UserByRole UserByRole = null;
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                UserByRole = db.tb_UserByRole.Where(W => W.ID == id).FirstOrDefault();
            }
            if (UserByRole == null)
            {
                return RedirectToAction("RoleList", "Users");
            }
            //绑定页面角色复选框
            PublicFunctions.SetUrls(ViewBag, Url);
            return View(new AddRoleEntity() { ID = UserByRole.ID, RoleName = UserByRole.RoleName });
        }
        [HttpPost, ValidateAntiForgeryToken, MyAuthorize(10)]
        public ActionResult DoEditRole(AddRoleEntity Entity)
        {
            if (ModelState.IsValid)
            {
                var UserByRole = new tb_UserByRole() { ID = Entity.ID };
                using (DB_BBSEntities db = new DB_BBSEntities())
                {
                    var updateUserByRole = db.tb_UserByRole.Attach(UserByRole);
                    updateUserByRole.RoleName = Entity.RoleName;
                    db.Entry(updateUserByRole).Property(T => T.RoleName).IsModified = true;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    bool isSuc = db.SaveChanges() > 0;
                    db.Configuration.ValidateOnSaveEnabled = true;
                    if (isSuc)
                    {
                        return RedirectToAction("RoleList", "Users");
                    }
                    ModelState.AddModelError("RoleError", "信息修改失败");
                }
            }
            else
            {
                ModelState.AddModelError("RoleError", "参数验证失败");
            }
            PublicFunctions.SetUrls(ViewBag, Url);
            return View("EditRole");
        }

        [HttpGet, MyAuthorize(6)]
        public ActionResult UserList()
        {
            PublicFunctions.SetUrls(ViewBag, Url);
            return View();
        }
        [MyAuthorize(6)]
        public ActionResult Get_UserList_Data(int limit = 10, int offset = 1, string search = "")
        {
            var lstRes = new List<UserListEntity>();
            int total = 0;
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                var List = from UBS in db.tb_UsersBySystem
                           join UBR in db.tb_UserByRole on UBS.RoleID equals UBR.ID
                           where UBS.UserName.Contains(search)
                           select new UserListEntity()
                           {
                               ID = UBS.ID,
                               RoleName = UBR.RoleName,
                               UserName = UBS.UserName,
                               NickName = UBS.NickName,
                               Email = UBS.Email
                           };
                lstRes = List.OrderBy(O => O.ID).Skip(offset).Take(limit).ToList();
                total = List.Count();
            }
            var rows = lstRes;
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, MyAuthorize(8)]
        public ActionResult CustomerUserList()
        {
            PublicFunctions.SetUrls(ViewBag, Url);
            return View();
        }
        [MyAuthorize(8)]
        public ActionResult Get_CustomerUserList_Data(int limit = 10, int offset = 1, string search = "")
        {
            var lstRes = new List<CustomerUserListEntity>();
            int total = 0;
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                var List = from UBC in db.tb_UsersByCustomer
                           join ZYS in db.tb_ZY_Sex on UBC.SexID equals ZYS.ID
                           where UBC.UserName.Contains(search)
                           select new CustomerUserListEntity()
                           {
                               ID = UBC.ID,
                               UserName = UBC.UserName,
                               NickName = UBC.NickName,
                               Sex = ZYS.Content,
                               Age = UBC.Age,
                               Fatieshu = UBC.Fatieshu ?? 0,
                               Huitieshu = UBC.Fatieshu ?? 0,
                               Email = UBC.Email,
                               IsModerator = UBC.IsModerator,
                               IsModeratorStr = UBC.IsModerator ? "是" : "否"
                           };
                lstRes = List.OrderBy(O => O.ID).Skip(offset).Take(limit).ToList();
                total = List.Count();
            }
            var rows = lstRes;
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, MyAuthorize(9)]
        public ActionResult RoleList()
        {
            PublicFunctions.SetUrls(ViewBag, Url);
            return View();
        }
        [MyAuthorize(9)]
        public ActionResult Get_RoleList_Data(int limit = 10, int offset = 1, string search = "")
        {
            var lstRes = new List<RoleListEntity>();
            int total = 0;
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                var List = from UBR in db.tb_UserByRole
                           where UBR.RoleName.Contains(search)
                           select new RoleListEntity()
                           {
                               ID = UBR.ID,
                               RoleName = UBR.RoleName,
                               ColumnCount = UBR.tb_UserByRoleJoinColumn.Count(C => C.RoleID == UBR.ID)
                           };
                lstRes = List.OrderBy(O => O.ID).Skip(offset).Take(limit).ToList();
                total = List.Count();
            }
            var rows = lstRes;
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, MyAuthorize(8)]
        public JsonResult TargetForumArea(FormCollection fc)
        {
            int CurUserID = 0;
            bool IsCancel = false;
            if (int.TryParse(fc["UserID"], out CurUserID) && bool.TryParse(fc["IsCancel"], out IsCancel))
            {
                bool isSuc = false;
                using (DB_BBSEntities db = new DB_BBSEntities())
                {
                    var UsersByCustomer = new tb_UsersByCustomer() { ID = CurUserID };
                    var S_UsersByCustomer = db.tb_UsersByCustomer.Attach(UsersByCustomer);
                    S_UsersByCustomer.IsModerator = IsCancel;
                    db.Entry(S_UsersByCustomer).Property(T => T.IsModerator).IsModified = true;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    isSuc = db.SaveChanges() > 0;
                    db.Configuration.ValidateOnSaveEnabled = true;
                }
                if (isSuc && !IsCancel)
                {
                    DBUpdate du = new DBUpdate();
                    DBSaveChanges dbSaveChanges = du.Set("UserID", 1, false)
                      .Where("UserID", CurUserID, false);
                    isSuc = dbSaveChanges.SaveChanges("tb_ForumArea");
                }
                if (isSuc)
                {
                    return Json(new { ResultStatus = 1, ResultMsg = "标记成功!" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { ResultStatus = 0, ResultMsg = "标记失败!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ResultStatus = -1, ResultMsg = "参数验证失败!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, MyAuthorize(8)]
        public JsonResult EditRoleJoinColumn(FormCollection fc)
        {
            int RoleID = 0;
            if (int.TryParse(fc["RoleID"], out RoleID) && RoleID > 0)
            {
                IList<SP_Get_RoleJoinColumn_Result> RoleJoinColumns = null;
                using (DB_BBSEntities db = new DB_BBSEntities())
                {
                    RoleJoinColumns = db.SP_Get_RoleJoinColumn(RoleID).ToList();
                }
                if (RoleJoinColumns != null && RoleJoinColumns.Count > 0)
                {
                    return Json(new { ResultStatus = 1, ResultMsg = PublicFunctions.ToJson(RoleJoinColumns) }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { ResultStatus = 0, ResultMsg = "系统中未定义栏目" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ResultStatus = -1, ResultMsg = "参数验证失败!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, MyAuthorize(8)]
        public JsonResult OnSaveRoleJoinColumn(FormCollection fc)
        {
            bool Isadd;
            if (bool.TryParse(fc["Isadd"], out Isadd))
            {
                if (!Isadd)
                {
                    int JoinID;
                    if (int.TryParse(fc["JoinID"], out JoinID))
                    {
                        using (DB_BBSEntities db = new DB_BBSEntities())
                        {
                            var Remove = new tb_UserByRoleJoinColumn() { ID = JoinID };
                            db.tb_UserByRoleJoinColumn.Attach(Remove);
                            db.tb_UserByRoleJoinColumn.Remove(Remove);
                            if (db.SaveChanges() > 0)
                            {
                                return Json(new { ResultStatus = 1, ResultMsg = "权限已成功删除" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { ResultStatus = 0, ResultMsg = "权限删除失败!" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                }
                else
                {
                    int RoleID, ColumnID;
                    if (int.TryParse(fc["RoleID"], out RoleID) && int.TryParse(fc["ColumnID"], out ColumnID))
                    {
                        using (DB_BBSEntities db = new DB_BBSEntities())
                        {
                            db.tb_UserByRoleJoinColumn.Add(new tb_UserByRoleJoinColumn() { RoleID = RoleID, ColumnID = ColumnID });
                            if (db.SaveChanges() > 0)
                            {
                                return Json(new { ResultStatus = 1, ResultMsg = "权限已成功附加" }, JsonRequestBehavior.AllowGet);
                            }
                            return Json(new { ResultStatus = 0, ResultMsg = "权限附加失败!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            return Json(new { ResultStatus = -1, ResultMsg = "参数验证失败!" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoginOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("InnerLogin", "Account");
        }
    }
}