using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using POS.Models;

namespace POS.Controllers
{
    public class AccountManagerController : Controller
    {
        POSTestingEntities db = new POSTestingEntities();
        // GET: AccountManage
        public ActionResult Index()
        {

            HttpContext.Server.ScriptTimeout = 1000;
            var obj = Session["S_User"];
            var activeUser = db.SP_GET_USERS_ALL();
            if (obj == null)
            {
                return RedirectToAction("Login", "AccountLogin");
            }
            return View(activeUser);
        }
        public ActionResult DeleteUser()
        {
            HttpContext.Server.ScriptTimeout = 1000;
            var obj = Session["S_User"];
            var deleteUser = db.SP_GET_USER_DELETED();
            if (obj == null)
            {
                return RedirectToAction("Login", "AccountLogin");
            }
            return View(deleteUser);
        }
        //GET /AccountManager/Details/4
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if(user == null )
            {
                return HttpNotFound();
            }
            return View(user);
        }
        //GET /AccountManager/Create
        public ActionResult Create()
        {
            //HttpContext.Server.ScriptTimeout = 1000;
            //Employe employe = new Employe();
            //employe.Active = true;
            //employe.Gender = true;
            //ViewBag.RoleId = new SelectList(db.Roles.Where(p => p.Actived).OrderBy(p => p.OrderBy), "RoleId", "RolesName");
            return View();
        }
        //POST /AccountManager/Create 
        [HttpPost, ValidateInput(false)]
        public ActionResult Create (User user, HttpPostedFileBase Avatar)
        {
            if (ModelState.IsValid)
            {
                if(Avatar !=null)
                {
                    string name = System.IO.Path.GetFileName(Avatar.FileName); // get file name
                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/Avatar"),name); // get link/filename
                    Avatar.SaveAs(path); // save to link/filename
                    user.Avatar = Avatar.FileName;
                }
                else
                {
                    user.Avatar = "default.png";
                }
                // add Info user 
                // check user is already exits
                var result = db.Users.Where(x => x.UserName == user.UserName && x.Active == true).Count();
                if (result > 0)
                {
                    ModelState.AddModelError("","UserName already Exits");
                }
                else
                {
                    //check roleID, EmployeeID
                    ViewBag.RoleId = new SelectList(db.Roles.Where(p => p.Actived).OrderBy(p => p.OrderBy), "RoleId", "RolesName");
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        // GET: /AccountManager/Edit/3
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        // POST: /AccountManager/Edit/5       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user, HttpPostedFileBase avatar)
        {
            if (ModelState.IsValid)
            {
                if (avatar != null)
                {
                    string name = System.IO.Path.GetFileName(avatar.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/UserAvatar"), name);
                    avatar.SaveAs(path);
                    user.Avatar = avatar.FileName;
                }
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }
    }
}