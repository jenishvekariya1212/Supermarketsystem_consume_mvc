using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Supermarketsystem.Areas.Admin.Models;
using Supermarketsystem.BAL;

namespace Supermarketsystem.Areas.Admin.Controllers
{
    [CheckAccess]
    [Area("Admin")]
    [Route("Admin/{Controller}/{Action}")]
    public class ProductController : Controller
    {
        Uri baseuri = new Uri("http://localhost:5071/api");
        private readonly HttpClient _Client;
        public ProductController()
        {
            _Client = new HttpClient();
            _Client.BaseAddress = baseuri;
        }
        public IActionResult GET()
        {
            List<ProductModel> products = new List<ProductModel>();
            HttpResponseMessage response = _Client.GetAsync($"{_Client.BaseAddress}/Product/Get").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonobject = JsonConvert.DeserializeObject(data);
                var dataofobject = jsonobject.data;
                var extractedDtaJson = JsonConvert.SerializeObject(dataofobject, Formatting.Indented);
                products = JsonConvert.DeserializeObject<List<ProductModel>>(extractedDtaJson);
            }
            return View("ProductList", products);
        }

        public IActionResult Delete(int ProductID)
        {
            HttpResponseMessage response = _Client.DeleteAsync($"{_Client.BaseAddress}/Product/Delete?ProductID={ProductID}").Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Person Delete";
            }
            else
            {
                TempData["Message"] = "Error deleting person. Please try again.";
            }

            return RedirectToAction("GET");

        }


      
        public IActionResult GetCascading(int CategoryID)
        {
            List<SubcategoryDropDown> productModel = new List<SubcategoryDropDown>();
            HttpResponseMessage response = _Client.GetAsync($"{_Client.BaseAddress}/Product/Cascading/{CategoryID}").Result;
            if (response.IsSuccessStatusCode)
            {

                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonobject = JsonConvert.DeserializeObject(data);
                var dataofobject = jsonobject.data;
                var extractedDtaJson = JsonConvert.SerializeObject(dataofobject, Formatting.Indented);
                productModel = JsonConvert.DeserializeObject<List<SubcategoryDropDown>>(extractedDtaJson);

               
               
            }

           var product= productModel;
            return Json(product);
        }


        [HttpGet]
        public IActionResult Edit(int ProductID)
        {



            /* Dropdown start*/
            List<Categorydropdown> Dropdown = new List<Categorydropdown>();
            HttpResponseMessage response1 = _Client.GetAsync($"{_Client.BaseAddress}/Product/GetCategoryDropdown").Result;
            if (response1.IsSuccessStatusCode)
            {

                string data = response1.Content.ReadAsStringAsync().Result;
                dynamic jsonobject = JsonConvert.DeserializeObject(data);
                var dataofobject = jsonobject.data;
                var extractedDtaJson = JsonConvert.SerializeObject(dataofobject, Formatting.Indented);
                Dropdown = JsonConvert.DeserializeObject<List<Categorydropdown>>(extractedDtaJson);
                ViewBag.DropDown = Dropdown;

            }


            /*Dropdown end*/


            /*Dropdown start*/
            List<SubcategoryDropDown> subcategoryDropDowns = new List<SubcategoryDropDown>();
            HttpResponseMessage response2 = _Client.GetAsync($"{_Client.BaseAddress}/Product/GetSubCategoryDropdown").Result;
            if (response2.IsSuccessStatusCode)
            {

                string data = response2.Content.ReadAsStringAsync().Result;
                dynamic jsonobject = JsonConvert.DeserializeObject(data);
                var dataofobject = jsonobject.data;
                var extractedDtaJson = JsonConvert.SerializeObject(dataofobject, Formatting.Indented);
                subcategoryDropDowns = JsonConvert.DeserializeObject<List<SubcategoryDropDown>>(extractedDtaJson);
                ViewBag.SubcategoryDropDown = subcategoryDropDowns;
            }

            /* Dropdown end*/




            ProductModel productModel = new ProductModel();
            HttpResponseMessage response = _Client.GetAsync($"{_Client.BaseAddress}/Product/GetById/{ProductID}").Result;
            if (response.IsSuccessStatusCode)
            {

                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonobject = JsonConvert.DeserializeObject(data);
                var dataofobject = jsonobject.data;
                var extractedDtaJson = JsonConvert.SerializeObject(dataofobject, Formatting.Indented);
                productModel = JsonConvert.DeserializeObject<ProductModel>(extractedDtaJson);



            }
            return View("ProductAdd", productModel);
        }



    


[HttpPost]
        public async Task<IActionResult> Save(ProductModel productModel)
        {
            try
            {
                MultipartFormDataContent fromdata = new MultipartFormDataContent();
                fromdata.Add(new StringContent(Convert.ToString(productModel.CategoryID)), "CategoryID");
                fromdata.Add(new StringContent(Convert.ToString(productModel.SubCategoryID)), "SubCategoryID");
                fromdata.Add(new StringContent(productModel.ProductName), "ProductName");
                fromdata.Add(new StringContent(Convert.ToString(productModel.ProductQuantity)), "ProductQuantity");
                fromdata.Add(new StringContent(Convert.ToString(productModel.ProductPrice)), "ProductPrice");
                fromdata.Add(new StringContent(Convert.ToString(productModel.ProductExpiryDate)), "ProductExpiryDate");
                fromdata.Add(new StringContent(productModel.ProductImage), "ProductImage");
                
                if (productModel.ProductID == 0)
                {
                    HttpResponseMessage response = await _Client.PostAsync($"{_Client.BaseAddress}/Product/Post", fromdata);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Person Inserted";
                        return RedirectToAction("GET");
                    }
                }
                else
                {
                    HttpResponseMessage response = await _Client.PutAsync($"{_Client.BaseAddress}/Product/Put?ProductID={productModel.ProductID}", fromdata);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Person updated";
                        return RedirectToAction("GET");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An Error Occured" + ex.Message;
            }
            return RedirectToAction("GET");
        }

    }
}
