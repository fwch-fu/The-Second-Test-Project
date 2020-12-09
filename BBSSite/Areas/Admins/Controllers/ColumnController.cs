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
    public class ColumnController : Controller
    {
        // GET: Admins/Column
        public ActionResult Index()
        {
            return View();
        }
        [MyAuthorize(3)]
        public ActionResult ColumnList()
        {
            PublicFunctions.SetUrls(ViewBag, Url);
            return View();
        }
        [MyAuthorize(3)]
        public JsonResult Get_ColumnList_Data(int limit = 10, int offset = 1, string search = "")
        {
            var lstRes = new List<ColumnListEntity>();
            int total = 0;
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                var List = from Column in db.tb_Column
                           where Column.ColumnName.Contains(search)
                           select new ColumnListEntity()
                           {
                               ID = Column.ID,
                               ColumnCode = Column.ColumnCode,
                               ColumnName = Column.ColumnName,
                               Url = Column.Url,
                               LogoClassName = Column.LogoClassName
                           };
                lstRes = List.OrderBy(O => O.ID).Skip(offset).Take(limit).ToList();
                total = List.Count();
            }
            var rows = lstRes;
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }

        [MyAuthorize(4)]
        public ActionResult CreateColumn()
        {
            PublicFunctions.SetUrls(ViewBag, Url);
            PublicService.GetColumnOneGrade(ViewBag);
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, MyAuthorize(4)]
        public ActionResult DoCreateColumn(AddColumnEntity Entity, FormCollection fc)
        {
            int ColumnGrade;
            if (ModelState.IsValid && int.TryParse(fc["ColumnGrade"], out ColumnGrade) && ColumnGrade > 0)
            {
                string ChooseParentColumn = fc["ChooseParentColumn"];
                if (ColumnGrade == 1 || (ColumnGrade == 2 && ChooseParentColumn != null && ChooseParentColumn != ""))
                {
                    tb_Column Column = PublicService.GetCurrentColumnCode(ColumnGrade, ChooseParentColumn);
                    string NewCode = PublicFunctions.CalcColumnCode(ColumnGrade, Column != null ? Column.ColumnCode : null, ChooseParentColumn);
                    tb_Column tc = new tb_Column()
                    {
                        ColumnCode = NewCode,
                        ColumnName = Entity.ColumnName,
                        Url = Entity.Url,
                        LogoClassName = Entity.LogoClassName
                    };
                    using (DB_BBSEntities db = new DB_BBSEntities())
                    {
                        db.tb_Column.Add(tc);
                        if (db.SaveChanges() > 0)
                        {
                            return RedirectToAction("ColumnList", "Column");
                        }
                    }
                    ModelState.AddModelError("ColumnError", "创建失败");
                }
                else
                {
                    ModelState.AddModelError("ColumnError", "参数验证失败");
                }
            }
            else
            {
                ModelState.AddModelError("ColumnError", "参数验证失败");
            }
            return View("CreateColumn");
        }

        [MyAuthorize(4)]
        public ActionResult EditColumn(int id = 0)
        {
            if (id < 1)
            {
                return RedirectToAction("ColumnList", "Column");
            }
            tb_Column Column = null;
            using (DB_BBSEntities db = new DB_BBSEntities())
            {
                Column = db.tb_Column.Where(W => W.ID == id).FirstOrDefault();
            }
            if (Column == null)
            {
                return RedirectToAction("ColumnList", "Column");
            }
            PublicFunctions.SetUrls(ViewBag, Url);
            PublicService.GetColumnOneGrade(ViewBag);
            bool IsColumnA = false;
            int ColumnCodeAID = 0;
            if (Column.ColumnCode.Length == 1)
            {
                IsColumnA = true;
            }
            else
            {
                string ColumnCodeA = Column.ColumnCode.Substring(0, 1);
                using (DB_BBSEntities db = new DB_BBSEntities())
                {
                    ColumnCodeAID = db.tb_Column.Where(W => W.ColumnCode == ColumnCodeA).FirstOrDefault().ID;
                }
            }
            return View(new AddColumnEntity() { ID = Column.ID, ColumnName = Column.ColumnName, Url = Column.Url, LogoClassName = Column.LogoClassName, IsColumnA = IsColumnA, ColumnCodeAID = ColumnCodeAID });
        }

        [HttpPost, ValidateAntiForgeryToken, MyAuthorize(4)]
        public ActionResult DoEditColumn(AddColumnEntity Entity)
        {
            PublicFunctions.SetUrls(ViewBag, Url);
            PublicService.GetColumnOneGrade(ViewBag);

            if (ModelState.IsValid)
            {
                var Column = new tb_Column() { ID = Entity.ID };
                using (DB_BBSEntities db = new DB_BBSEntities())
                {
                    var updateColumninfo = db.tb_Column.Attach(Column);
                    updateColumninfo.ColumnName = Entity.ColumnName;
                    updateColumninfo.Url = Entity.Url;
                    updateColumninfo.LogoClassName = Entity.LogoClassName;

                    db.Entry(updateColumninfo).Property(T => T.ColumnName).IsModified = true;
                    db.Entry(updateColumninfo).Property(T => T.Url).IsModified = true;
                    db.Entry(updateColumninfo).Property(T => T.LogoClassName).IsModified = true;
                    db.Entry(updateColumninfo).Property(T => T.ColumnCode).IsModified = false;

                    db.Configuration.ValidateOnSaveEnabled = false;
                    bool isSuc = db.SaveChanges() > 0;
                    db.Configuration.ValidateOnSaveEnabled = true;
                    if (isSuc)
                    {
                        return RedirectToAction("ColumnList", "Column");
                    }
                }
                ModelState.AddModelError("InfoError", "信息修改失败");
            }
            ModelState.AddModelError("InfoError", "参数验证失败");
            return View("EditColumn");
        }
    }
}