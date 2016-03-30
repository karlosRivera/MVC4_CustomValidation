using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CustomValidation.Models
{

    public class City
    {
        public int Id { get; set; }           // Integer value of a checkbox
        public string Name { get; set; }      // String name of a checkbox
        public object Tags { get; set; }      // Object of html tags to be applied to checkbox, e.g.: 'new { tagName = "tagValue" }'
        public bool IsSelected { get; set; }  // Boolean value to select a checkbox on the list
    }

    public class CitiesViewModel
    {
        public IList<City> AvailableCities { get; set; }
        [Required(ErrorMessage = "Select any check box")]
        public IList<City> SelectedCities { get; set; }
        public PostedCities PostedCities { get; set; }
    }

    // Helper class to make posting back selected values easier
    public class PostedCities
    {
        public string[] CityIDs { get; set; }
    } 
}