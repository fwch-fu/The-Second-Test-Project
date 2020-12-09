using BBSSite.Areas.Admins.ViewModels;
using BBSSite.Models;
using BBSSite.MyPublic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BBSSite.Areas.Admins.Controllers
{
    public class SectionController : Controller
    {
        // GET: Admins/Section
        public ActionResult Index()
        {
            return View();
        }
        [MyAuthorize(12)]
        public ActionResult SectionList()
        {
            PublicFunctions.SetUrls(ViewBag, Url);
            return View();
        }
        [MyAuthorize(12)]
        public JsonResult Get_SectionList_Data(int limit = 10, int offset = 1, string search = "")
        {
            var lstRes = new List<ForumAreaListEntity>();
            int total = 0;
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                var List = from FA in db.tb_ForumArea
                           join UC in db.tb_UsersByCustomer on FA.UserID equals UC.ID
                           where FA.AreaName.Contains(search)
                           select new ForumAreaListEntity()
                           {
                               ID = FA.ID,
                               AreaName = FA.AreaName,
                               UserName = UC.UserName
                           };
                lstRes = List.OrderBy(O => O.ID).Skip(offset).Take(limit).ToList();
                total = List.Count();
            }
            var rows = lstRes;
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize(12)]
        public ActionResult SectionChildList(int id)
        {
            PublicFunctions.SetUrls(ViewBag, Url);
            ViewBag.id = id;
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                ViewBag.AreaName = db.tb_ForumArea.Where(W => W.ID == id).First().AreaName;
            }
            return View();
        }
        [MyAuthorize(12)]
        public JsonResult Get_SectionChildList_Data(int limit = 10, int offset = 1, int id = 0, string search = "")
        {
            if (id < 1)
            {
                return Json(new { total = 0 }, JsonRequestBehavior.AllowGet);
            }
            var lstRes = new List<ForumClassifyListEntity>();
            int total = 0;
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                var List = from FC in db.tb_ForumClassify
                           join UC in db.tb_UsersByCustomer on FC.ForumUserID equals UC.ID
                           where FC.ForumAreaID == id && FC.ClassifyName.Contains(search)
                           select new ForumClassifyListEntity()
                           {
                               ID = FC.ID,
                               ClassifyName = FC.ClassifyName,
                               ForumUser = UC.UserName,
                               ClassifyOrder = FC.ClassifyOrder,
                               ClassifyIsleaf = (FC.ClassifyIsleaf ?? false) ? "是" : "否"
                           };
                lstRes = List.OrderBy(O => O.ID).Skip(offset).Take(limit).ToList();
                total = List.Count();
            }
            var rows = lstRes;
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize(13)]
        public ActionResult CreateSection()
        {
            PublicService.GetUsersByCustomer(ViewBag, 0);
            PublicFunctions.SetUrls(ViewBag, Url);
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, MyAuthorize(13)]
        public ActionResult DoCreateSection(CreateForumAreaEntity Entity)
        {
            if (ModelState.IsValid)
            {
                tb_ForumArea ta = new tb_ForumArea()
                {
                    AreaName = Entity.AreaName,
                    UserID = Entity.UserID
                };
                using (DB_BBSEntities db = new DB_BBSEntities())
                {
                    db.tb_ForumArea.Add(ta);
                    if (db.SaveChanges() > 0)
                    {
                        return RedirectToAction("SectionList", "Section");
                    }
                }
                ModelState.AddModelError("SectionError", "创建失败");
            }
            else
            {
                ModelState.AddModelError("SectionError", "参数验证失败");
            }
            PublicService.GetUsersByCustomer(ViewBag, 0);
            PublicFunctions.SetUrls(ViewBag, Url);
            return View("CreateSection");
        }
        [MyAuthorize(13)]
        public ActionResult EditSection(int id = 0)
        {
            if (id < 1)
            {
                return RedirectToAction("SectionList", "Section");
            }
            tb_ForumArea ForumArea = null;
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                ForumArea = db.tb_ForumArea.Where(W => W.ID == id).FirstOrDefault();
            }
            if (ForumArea != null)
            {
                PublicService.GetUsersByCustomer(ViewBag, ForumArea.UserID);
                PublicFunctions.SetUrls(ViewBag, Url);
                return View(new CreateForumAreaEntity() { ID = ForumArea.ID, AreaName = ForumArea.AreaName, UserID = ForumArea.UserID });
            }
            return RedirectToAction("SectionList", "Section");
        }
        [HttpPost, ValidateAntiForgeryToken, MyAuthorize(13)]
        public ActionResult DoEditSection(CreateForumAreaEntity Entity)
        {
            if (ModelState.IsValid)
            {
                var ForumArea = new tb_ForumArea() { ID = Entity.ID };
                using (DB_BBSEntities db = new DB_BBSEntities())
                {
                    var updateForumArea = db.tb_ForumArea.Attach(ForumArea);
                    updateForumArea.AreaName = Entity.AreaName;
                    updateForumArea.UserID = Entity.UserID;
                    db.Entry(updateForumArea).Property(P => P.AreaName).IsModified = true;
                    db.Entry(updateForumArea).Property(P => P.UserID).IsModified = true;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    bool isSuc = db.SaveChanges() > 0;
                    db.Configuration.ValidateOnSaveEnabled = true;
                    if (isSuc)
                    {
                        return RedirectToAction("SectionList", "Section");
                    }
                }
                ModelState.AddModelError("SectionError", "修改失败");
            }
            else
            {
                ModelState.AddModelError("SectionError", "参数验证失败");
            }
            PublicService.GetUsersByCustomer(ViewBag, Entity.UserID);
            PublicFunctions.SetUrls(ViewBag, Url);
            return View("EditSection");
        }

        [MyAuthorize(13)]
        public ActionResult CreateSectionChild(int id)
        {
            PublicService.GetUsersByCustomer(ViewBag, 0);
            PublicFunctions.SetUrls(ViewBag, Url);
            ViewBag.id = id;
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, MyAuthorize(13)]
        public ActionResult DoCreateSectionChild(CreateForumClassifyEntity Entity)
        {
            if (ModelState.IsValid)
            {
                tb_ForumClassify ta = new tb_ForumClassify()
                {
                    ForumAreaID = Entity.ForumAreaID,
                    ForumUserID = Entity.ForumUserID,
                    ClassifyName = Entity.ClassifyName,
                    ClassifyLogo = Entity.ClassifyLogo,
                    ClassifyInnerLogo = Entity.ClassifyInnerLogo,
                    ClassifyOrder = Entity.ClassifyOrder
                };
                using (DB_BBSEntities db = new DB_BBSEntities())
                {
                    db.tb_ForumClassify.Add(ta);
                    if (db.SaveChanges() > 0)
                    {
                        return RedirectToAction("SectionChildList", "Section", new { id = Entity.ForumAreaID });
                    }
                }
                ModelState.AddModelError("SectionError", "创建失败");
            }
            else
            {
                ModelState.AddModelError("SectionError", "参数验证失败");
            }
            return View("CreateSectionChild");
        }
        [HttpPost, MyAuthorize(12)]
        public JsonResult SettingModerator(FormCollection fc)
        {
            int UserID = 0, ForumAreaID = 0;
            if (int.TryParse(fc["UserID"], out UserID) && int.TryParse(fc["ForumAreaID"], out ForumAreaID) && UserID > 0 && ForumAreaID > 0)
            {
                var ForumArea = new tb_ForumArea() { ID = ForumAreaID };
                using (DB_BBSEntities db = new DB_BBSEntities())
                {
                    var S_ForumArea = db.tb_ForumArea.Attach(ForumArea);
                    S_ForumArea.UserID = UserID;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Entry(S_ForumArea).Property(T => T.UserID).IsModified = true;
                    bool isSuc = db.SaveChanges() > 0;
                    db.Configuration.ValidateOnSaveEnabled = true;
                    if (isSuc)
                    {
                        return Json(new { ResultStatus = 1, ResultMsg = "版主设置成功!" }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { ResultStatus = 0, ResultMsg = "版主设置失败!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ResultStatus = -1, ResultMsg = "参数验证失败!" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, MyAuthorize(12)]
        public JsonResult SettingModeratorChild(FormCollection fc)
        {
            int UserID = 0, ForumClassifyID = 0;
            if (int.TryParse(fc["UserID"], out UserID) && int.TryParse(fc["ForumClassifyID"], out ForumClassifyID) && UserID > 0 && ForumClassifyID > 0)
            {
                var ForumClassify = new tb_ForumClassify() { ID = ForumClassifyID };
                using (DB_BBSEntities db = new DB_BBSEntities())
                {
                    var S_ForumClassify = db.tb_ForumClassify.Attach(ForumClassify);
                    S_ForumClassify.ForumUserID = UserID;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Entry(S_ForumClassify).Property(T => T.ForumUserID).IsModified = true;
                    bool isSuc = db.SaveChanges() > 0;
                    db.Configuration.ValidateOnSaveEnabled = true;
                    if (isSuc)
                    {
                        return Json(new { ResultStatus = 1, ResultMsg = "版主设置成功!" }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { ResultStatus = 0, ResultMsg = "版主设置失败!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ResultStatus = -1, ResultMsg = "参数验证失败!" }, JsonRequestBehavior.AllowGet);
        }
    }
}