using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVVCProject.Models;

namespace MVVCProject.Controllers
{
    public class TestController : Controller
    {
        MaamAssignmentDBEntities db = new MaamAssignmentDBEntities();
        // GET: Test
        [HttpGet]
        public ActionResult ShowProduct()
        {
            var data = new SelectList(db.Products, "prodid", "pname");
            Session["pdata"] = data;
            return View();
        }
        [HttpPost]

        public ActionResult ShowProduct(string Command)
        {
            if (Command == "ShowModel")
            {

                int id = int.Parse(Request.Form["ddlprod"].ToString());
                var join = (from p in db.Products
                            join m in db.Models
                            on p.prodid equals m.prodid
                            where m.prodid == id
                            select m).ToList();
                Session["mdata"] = new SelectList(join, "modelid", "modelname");
                return View();
            }
            if (Command == "Confirm")
            {
                Order ord = new Order();
                ord.modelid = int.Parse(Request.Form["ddlmodel"].ToString());
                ord.Productid = int.Parse(Request.Form["ddlprod"].ToString());
                ord.orderdate = DateTime.Now;

                ord.Qty = int.Parse(Request.Form["txtquantity"].ToString());
                ord.address = Request.Form["txtaddress"].ToString();
                db.Orders.Add(ord);
                var r = db.SaveChanges();
                if (r > 0)
                    ModelState.AddModelError("", "Order Confirmed");
                return View();
            }
            return View();

        }
        [HttpGet]

        public ActionResult TrackOrder()
        {

            return View();

        }
        [HttpPost]
        public ActionResult TrackOrder(string Command)
        {
            int orderid = int.Parse(Request.Form["txtorderid"].ToString());
            var data = db.Orders.Where(x => x.OrderId == orderid).SingleOrDefault();
            ViewData["userdata"] = data;
            int pid = data.modelid;
            int qty = data.Qty;
            var result = db.Models.Where(x => x.modelid == pid).SingleOrDefault();
            float price = (float)result.price;
            float amt = price * qty;
            Session["amount"] = amt;
            var mname = data.Model.modelname;
            Session["modelname"] = mname;
            var dt = DateTime.Parse(data.orderdate.Date.ToString());
            var del = dt.AddDays(5);
            Session["delivery"] = del;
            var dt1 = dt.AddDays(1);
            var dt2 = dt.AddDays(2);
            var dt3 = dt.AddDays(3);
            var dt4 = dt.AddDays(4);
            var dt5 = dt.AddDays(5);
            if (DateTime.Now.Year == dt.Year && DateTime.Now.Month == dt.Month)
            {
                if (dt == DateTime.Now.Date)
                {
                    var status = "the order is being Packed";
                    Session["stat"] = status;

                }
                else if (dt1 == DateTime.Now.Date)
                {
                    var status = "It is being Dispatched";
                    Session["stat"] = status;

                }

                else if (dt2 == DateTime.Now.Date)
                {
                    var status = "Shipped";
                    Session["stat"] = status;

                }
                else if (dt3 == DateTime.Now.Date)
                {
                    var status = "Shipped";
                    Session["stat"] = status;

                }
                else if (dt4 == DateTime.Now.Date)
                {
                    var status = "Shipped";
                    Session["stat"] = status;

                }
                else if (dt5 == DateTime.Now.Date)
                {
                    var status = "On the Way";
                    Session["stat"] = status;

                }



            }
           
            return View();

        }
        [HttpGet]
        public ActionResult display()
        {
            
            var data = db.Orders.Max(x => x.OrderId);

            Session["rkdata"] = data;
            return View();
        }

        [HttpPost]
        

        public ActionResult display(string Command,Order or)
        {
            int orderid = int.Parse(Request.Form["txtorder"].ToString());
            
            var take = db.Orders.Where(x => x.OrderId == orderid).SingleOrDefault();
            int model = take.modelid;
            int qty = take.Qty;
            int pid = take.Productid;
            var price = db.Models.Where(x => x.modelid == model).SingleOrDefault();
            float pr = (float)price.price;
            string prname = db.Products.Where(x => x.prodid == pid).Select(x => x.pname).SingleOrDefault();
            float subtotal = pr * qty;
            Session["pname"] = prname;
            Session["iprice"] = pr;
            Session["iqty"] = qty;
            Session["subtotal"] = subtotal;
            return View();

        }

    }
}