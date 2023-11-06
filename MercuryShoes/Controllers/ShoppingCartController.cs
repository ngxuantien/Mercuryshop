using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MercuryShoes.Models;

namespace MercuryShoes.Controllers
{
    public class ShoppingCartController : Controller
    {
        MecuryshopEntities db = new MecuryshopEntities();
        public List<ShopCart> LayGioHang()
        {
            List<ShopCart> lstGioHang = Session["ShopCart"] as List<ShopCart>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<ShopCart>();
                Session["ShopCart"] = lstGioHang;
            }
            return lstGioHang;
        }

        public ActionResult ThemGioHang(int ms, string url)
        {
            List<ShopCart> lstGioHang = LayGioHang();
            ShopCart sp = lstGioHang.Find(n => n.iMasp == ms);
            if (sp == null)
            {
                sp = new ShopCart(ms);
                lstGioHang.Add(sp);
            }
            else
            {
                sp.iSoLuong++;
            }
            return Redirect(url);
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<ShopCart> lstGioHang = Session["ShopCart"] as List<ShopCart>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);

            }
            return iTongSoLuong;
        }
        private double TongTien()
        {
            double dTongTien = 0;
            List<ShopCart> lstGioHang = Session["ShopCart"] as List<ShopCart>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.dThanhTien);
            }
            return dTongTien;
        }
        public ActionResult GioHang()
        {
            List<ShopCart> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "MercuryHome");
                //return RedirectToAction("GioHang", "ShoppingCart");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

        public ActionResult XoaSPKhoiGioHang(int iMasp)
        {
            List<ShopCart> lstGioHang = LayGioHang();
            ShopCart sp = lstGioHang.SingleOrDefault(n => n.iMasp == iMasp);
            if (sp != null)
            {
                lstGioHang.RemoveAll(n => n.iMasp == iMasp);
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("Index", "MercuryHome");
                }
            }
            return RedirectToAction("ShopCart");
        }

        public ActionResult CapNhatGioHang(int iMasp, FormCollection f)
        {
            List<ShopCart> lstGioHang = LayGioHang();
            ShopCart sp = lstGioHang.SingleOrDefault(n => n.iMasp == iMasp);
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("ShopCart");
        }
        public ActionResult XoaGioHang()
        {
            List<ShopCart> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "MercuryHome");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Customer");
            }
            if (Session["ShopCart"] == null)
            {
                return RedirectToAction("Index", "MeruryHome");
            }
            List<ShopCart> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }

        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            DonHang dh = new DonHang();
            tbl_Customer kh = (tbl_Customer)Session["TaiKhoan"];
            List<ShopCart> lstGioHang = LayGioHang();
            dh.Customer_Id = kh.Customer_Id;
            dh.NgayDat = DateTime.Now;
            var NgayGiao = String.Format("{0:MM/dd/yyyy}", f["NgayGiao"]);
            dh.NgayGiao = DateTime.Parse(NgayGiao);
            dh.TinhTrangGiaoHang = true;
            dh.TinhTrangThanhToan = false;
            db.DonHangs.Add(dh);
            db.SaveChanges();
            foreach (var item in lstGioHang)
            {
                CTDonHang ctdh = new CTDonHang();
                ctdh.MaDonHang = dh.MaDonHang;
                ctdh.Product_Id = item.iMasp;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.dDonGia;
                db.CTDonHangs.Add(ctdh);
            }
            db.SaveChanges();
            Session["ShopCart"] = null;
            return RedirectToAction("XacNhanDonHang", "ShoppingCart");
        }
        public ActionResult XacNhanDonHang()
        {
            return View();
        }
        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }
    }
}