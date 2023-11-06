using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MercuryShoes.Models;
using System.Data.Entity;

namespace MercuryShoes.Controllers
{
    public class CustomerController : Controller
    {
        MecuryshopEntities db = new MecuryshopEntities();
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection collection, tbl_Customer kh)
        {
            var sHoTen = collection["Name"];
            var sMatKhau = collection["Pass"];
            var sNhapLaiMK = collection["rePass"];
            var sDiaChi = collection["Address"];
            var sEmail = collection["Email"];
            var sDienThoai = collection["Numberphone"];
            if (String.IsNullOrEmpty(sHoTen))
            {
                ViewData["err1"] = "Họ tên không được rỗng";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err2"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(sNhapLaiMK))
            {
                ViewData["err2"] = "Phải nhập lại mật khẩu";
            }
            else if (sMatKhau != sNhapLaiMK)
            {
                ViewData[""] = "Mật khẩu nhập lại không khớp";
            }
            else if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err2"] = "Email không được rỗng";
            }
            else if (String.IsNullOrEmpty(sDienThoai))
            {
                ViewData["err2"] = "Số điện thoại không được rỗng";
            }
            else if (db.tbl_Customer.SingleOrDefault(n => n.Customer_Email == sEmail) != null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }
            else
            {
                kh.Customer_Id = 0;
                kh.Customer_Name = sHoTen;
                kh.Customer_Password = sMatKhau;
                kh.Customer_Email = sEmail;
                kh.Customer_Address = sDiaChi;
                kh.Customer_Phone = sDienThoai;
                db.tbl_Customer.Add(kh);
                db.SaveChanges();
                return RedirectToAction("DangNhap");
            }
            return this.DangKy();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var sTenDN = collection["Email"];
            var sMatKhau = collection["Pass"];
            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu";
            }
            else
            {
                tbl_Customer kh = db.tbl_Customer.SingleOrDefault(n => n.Customer_Email == sTenDN && n.Customer_Password == sMatKhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["TaiKhoan"] = kh;
                    return RedirectToAction("Index","MercuryHome");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
    }
}