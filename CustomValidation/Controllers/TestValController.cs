using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthTest.Models;



namespace AuthTest.Controllers
{
    public class TestValController : Controller
    {
        // GET: TestVal
        public ActionResult Index()
        {
            var SampleVM = new SampleViewModel();
            SampleVM.ErrorMsg = "";
            SampleVM.Products = new List<Product>
            {
                new Product{ ID=1, Name="IPhone" },
                new Product{ ID=2, Name="MacBook Pro" },
                new Product{ ID=3, Name="iPod" }           
            };

            SampleVM.Hobbies = new List<Hobby>()
            {
                new Hobby(){ Name = "Reading", IsSelected= false },
                new Hobby(){ Name = "Sports" ,IsSelected= false},
                new Hobby(){ Name = "Movies" ,IsSelected= false}
            };
            return View(SampleVM);
        }

        [HttpPost]
        public ActionResult Index(SampleViewModel vm)
        {
            ViewBag.IsPostBack = false;
            if (ModelState.IsValid)
            {
                Product p = vm.Products.Where(x => x.ID == vm.SelectedProductId).FirstOrDefault();
                if (vm.IsAdult == null)
                    vm.IsAdult = false;

                ViewBag.IsPostBack = true;
                ViewBag.ProductID = p.ID.ToString();
                ViewBag.ProductName = p.Name.ToString();
                ViewBag.Gender = vm.Gender.ToString();
                ViewBag.Hobbies = string.Join(",", (string[]) vm.Hobbies.Where(e => e.IsSelected == true).Select(i=> i.Name).ToArray());
                ViewBag.IsAdult = ((bool) vm.IsAdult ? "Yes" : "No");
                ViewBag.Age = (vm.Age < 0 ? 0 : vm.Age);
                return View(vm);
            }
            else
            {
                var SampleVM = new SampleViewModel();
                SampleVM.Products = new List<Product>
                {
                    new Product{ ID=1, Name="IPhone" },
                    new Product{ ID=2, Name="MacBook Pro" },
                    new Product{ ID=3, Name="iPod" }           
                };
                SampleVM.Hobbies = new List<Hobby>()
                {
                    new Hobby(){ Name = "Reading", IsSelected= false },
                    new Hobby(){ Name = "Sports" ,IsSelected= true},
                    new Hobby(){ Name = "Movies" ,IsSelected= true}
                };

                return View(SampleVM);
            }
        }
    }

   
}