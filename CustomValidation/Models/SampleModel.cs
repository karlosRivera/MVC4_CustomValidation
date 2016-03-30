using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections;

namespace AuthTest.Models
{

    public class Product
    {
        public int ID { set; get; }
        public string Name { set; get; }
    }

    public class Hobby 
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }

    }

    public class SampleViewModel
    {
        [Display(Name = "Products")]
        public List<Product> Products { set; get; }

        //[AtleastOne(ErrorMessage = "Select at least one checkbox.")]
        public List<Hobby> Hobbies { get; set; }

        [Required(ErrorMessage = "Select any Product")]
        public int SelectedProductId { set; get; }

        [Required(ErrorMessage = "Select Male or Female")]
        public string Gender { get; set; }

        public bool? IsAdult { get; set; }
        public int? Age { get; set; }

        [ConditionalAttribute(SelectedProductID = "SelectedProductId", Products = "Products", Hobbies = "Hobbies",IsAdult="IsAdult",Age="Age")]
        public string ErrorMsg { get; set; }

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class ConditionalAttribute : ValidationAttribute , IClientValidatable
    {
        public string SelectedProductID = "", Products = "", Hobbies="";
        public string IsAdult = "";
        public string Age ="";
        string _productname = "";
        bool _hashobby = false;
        bool _isadult = false;
        int _age = 0;

        public ConditionalAttribute() { }
        public ConditionalAttribute(string SelectedProductId, string Products, string Hobbies, string IsAdult, string Age)
        {
            this.SelectedProductID = SelectedProductId;
            this.Products = Products;
            this.Hobbies = Hobbies;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //getting selected product
            Product oProduct = null;
            ValidationResult validationResult = ValidationResult.Success;
            var containerType = validationContext.ObjectInstance.GetType();

            var SelectedProductID = containerType.GetProperty(this.SelectedProductID);
            Int32 selectedproduct = (Int32)SelectedProductID.GetValue(validationContext.ObjectInstance, null);

            var ProductList = containerType.GetProperty(this.Products);
            List<Product> oProducts = (List<Product>)ProductList.GetValue(validationContext.ObjectInstance, null);

            oProduct = oProducts.Where(e => e.ID == selectedproduct).FirstOrDefault();
            _productname = oProduct.Name;

            if (_productname != "iPod")
            {
                var field2 = containerType.GetProperty(this.Hobbies);
                List<Hobby> hobbies = (List<Hobby>)field2.GetValue(validationContext.ObjectInstance, null);
                foreach (var hobby in hobbies)
                {
                    if (hobby.IsSelected)
                    {
                        _hashobby = true;
                        break;
                    }
                        //return ValidationResult.Success;
                }
                if (!_hashobby)
                {
                    this.ErrorMessage = "Select Any Hobbie's checkbox";
                    return new ValidationResult(ErrorMessage);
                    //return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }

            var PropIsAdult = containerType.GetProperty(this.IsAdult);
            if (PropIsAdult.GetValue(validationContext.ObjectInstance, null) != null)
            {
                _isadult = (bool)PropIsAdult.GetValue(validationContext.ObjectInstance, null);
                if (_isadult)
                {
                    var PropAge = containerType.GetProperty(this.Age);
                    if (PropAge.GetValue(validationContext.ObjectInstance, null) != null)
                    {
                        _age = (Int32)PropAge.GetValue(validationContext.ObjectInstance, null);
                        if (_age != null && _age <= 0)
                        {
                            this.ErrorMessage = "Age is compulsory for adult";
                            return new ValidationResult(ErrorMessage);
                        }
                    }
                    else
                    {
                        this.ErrorMessage = "Age is compulsory for adult";
                        return new ValidationResult(ErrorMessage);
                    }
                }
            }
            return ValidationResult.Success;
        }


        // Implement IClientValidatable for client side Validation
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "customvalidation",
            };
            rule.ValidationParameters.Add("productname", _productname);
            //rule.ValidationParameters.Add("hashobby", _hashobby);
            //rule.ValidationParameters.Add("isadult", _isadult);
            //rule.ValidationParameters.Add("age", _age);
            yield return rule;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class AtleastOneAttribute : ValidationAttribute, IClientValidatable
    {
        // For Server side
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var oHobby=value as IEnumerable;

                foreach (var _object in oHobby)
                {
                    Hobby _oHobby = (Hobby)_object;
                    if (_oHobby.IsSelected)
                    {
                        return ValidationResult.Success;
                    }
                }
                
            }
            return new ValidationResult(ErrorMessage);
        }
        // Implement IClientValidatable for client side Validation
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "atleastonetrue",
            };
            yield return rule;
        }
    }
}