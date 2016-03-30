using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomValidation.Models;

namespace CustomValidation.Controllers
{
    public class CheckboxListController : Controller
    {
        //
        // GET: /CheckboxList/

        public ActionResult Index()
        {
            CitiesViewModel vm = new CitiesViewModel()
            {
                AvailableCities = new List<City>
                {
                    new City()
                    {
                        Id=1,
                        IsSelected=false,
                        Name="Bombay",
                        Tags="Bombay"
                    },
                    new City()
                    {
                        Id=2,
                        IsSelected=false,
                        Name="kolkata",
                        Tags="kolkata"
                    },
                    new City()
                    {
                        Id=3,
                        IsSelected=false,
                        Name="Madras",
                        Tags="Madras"
                    },
                    new City()
                    {
                        Id=4,
                        IsSelected=false,
                        Name="Delhi",
                        Tags="Delhi"
                    },
                }
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(CitiesViewModel vm)
        {
            return View(vm);
        }
    }
}
