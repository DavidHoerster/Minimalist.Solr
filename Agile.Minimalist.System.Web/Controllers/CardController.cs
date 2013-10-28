using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Agile.Minimalist.System.Web.Controllers
{
    public class CardController : Controller
    {
        [HttpGet()]
        public ActionResult Index(String id)
        {
            return View();
        }
	}
}