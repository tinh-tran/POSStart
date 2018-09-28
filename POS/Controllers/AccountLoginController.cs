using POS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;


namespace POS.Controllers
{
    public class AccountLoginController : Controller
    {
        POSTestingEntities context = new POSTestingEntities();
        // GET: AccountLogin
        
        // Check UserName And Password 
        public bool CheckValid (string username, string password)
        {
            bool CheckValid = false;
            using (var context = new POSTestingEntities())
            {
                var user = context.Users.FirstOrDefault(x => x.UserName == username);
                if (user != null)
                    try
                    {
                        if (user.UserName == username && user.Active == true)
                        {
                            if( user.PassWord == password)
                            {
                                CheckValid = true;
                            }
                        }
                    }
                    catch
                    {
                        CheckValid = false;
                    }
            }
            return CheckValid;
        }
        // Check Login (With Checkvalid)
        public ActionResult Login (User user)
        {
            if(CheckValid(user.UserName , user.PassWord))
            {
                FormsAuthentication.SetAuthCookie(user.UserName, user.Remember);
                Session.Add("S_User", user);
                ViewBag.active = user.UserName;
                return RedirectToAction("Index", "AccountManager");
            }
            else
            {
                ModelState.AddModelError("", "Password incorrect or Account is not active now!!");
            }
            return View(user);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Remove("S_User");
            return RedirectToAction("Login", "Account");
        }

        // Create Json retun check Login 
        [HttpPost]
        public JsonResult CheckUser(FormCollection form)
        {
            System.Threading.Thread.Sleep(1000); // stop thread in 1s
            string name = form["username"];
            var user = context.Users.Where(x => x.UserName == name).Count();
            if (user > 0)
            {
                return Json(new { result = true });
            }
            else
            {
                return Json(new { result = false });
            }
        }
        // Get Layout User and UserName
        public ActionResult GetUserActive(string view = "")
        {
            User user = new User();
            user = (User)Session["S_User"];
            var model = context.Users.Where(x => x.UserName.Equals(user.UserName)).FirstOrDefault();
            return PartialView(view, model);
        }
        public ActionResult GetAvatar(string id = "", string view = "")
        {
            User user = new User();
            if (String.IsNullOrEmpty(id))
                id = User.Identity.Name;
            //user = null thì lấy thông tin của user đang đăng nhập
            if (id != null)
            {
                user = context.Users.Where(x => x.UserName.Equals(id)).FirstOrDefault();
            }
            else
            {
                int ids = int.Parse(id);
                user = context.Users.Where(x => x.UserId == ids).FirstOrDefault();
            }

            return PartialView(view, user);
        }
    }
}