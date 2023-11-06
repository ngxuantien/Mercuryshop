using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using MercuryShoes.Models;

namespace MercuryShoes.Controllers
{
    public class MercuryHomeController : Controller
    {
        MecuryshopEntities db = new MecuryshopEntities();
        // GET: MercuryHome
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Product(int? page, int? pageSize)
        {
            if(page == null)
            {
                page = 1;
            }
            if(pageSize == null)
            {
                pageSize = 2;
            }
            var products = db.tbl_Product.ToList();
            return View(products.ToPagedList((int)page, (int)pageSize));
        }
    }
}