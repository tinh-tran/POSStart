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
    }
}