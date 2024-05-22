using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Supermarketsystem.Areas.Admin.Models;
using Supermarketsystem.BAL;
using System.Security.Cryptography.Xml;

namespace Supermarketsystem.Areas.Admin.Controllers
{
    [CheckAccess]
    [Area("Admin")]
    [Route("Admin/{Controller}/{Action}")]

    public class CategoryController : Controller
    {
        Uri baseuri = new Uri("http://localhost:5071/api");
        private readonly HttpClient _Client;
        public CategoryController() { 
            _Client = new HttpClient();
            _Client.BaseAddress = baseuri;
        }
        public IActionResult GET()
        {
            List<CategoryModel> categories = new List<CategoryModel>();
            HttpResponseMessage response = _Client.GetAsync($"{_Client.BaseAddress}/Category/Get").Result;
            if (response.IsSuccessStatusCode) { 
            
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonobject = JsonConvert.DeserializeObject(data);
                var dataofobject = jsonobject.data;
                var extractedDtaJson = JsonConvert.SerializeObject(dataofobject, Formatting.Indented);
                categories = JsonConvert.DeserializeObject<List<CategoryModel>>(extractedDtaJson);
            }
            return View("CategoryList",categories);
        }


        public IActionResult Delete(int CategoryID)
        {
            HttpResponseMessage response = _Client.DeleteAsync($"{_Client.BaseAddress}/Category/Delete?CategoryID={CategoryID}").Result;
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

        [HttpGet]
        public IActionResult Edit(int CategoryID) { 
        CategoryModel category = new CategoryModel();
            HttpResponseMessage response = _Client.GetAsync($"{_Client.BaseAddress}/Category/GetById/{CategoryID}").Result;
            if (response.IsSuccessStatusCode)
            {

                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonobject = JsonConvert.DeserializeObject(data);
                var dataofobject = jsonobject.data;
                var extractedDtaJson = JsonConvert.SerializeObject(dataofobject, Formatting.Indented);
                category = JsonConvert.DeserializeObject<CategoryModel>(extractedDtaJson);
            }
            return View("CategoryAdd",category);
               
        }

        [HttpPost]
        public async Task<IActionResult> Save(CategoryModel categoryModel)
        {
            try
            {
                MultipartFormDataContent fromdata = new MultipartFormDataContent();
                fromdata.Add(new StringContent(categoryModel.CategoryName), "CategoryName");
                fromdata.Add(new StringContent(categoryModel.CategoryDescription), "CategoryDescription");
                if (categoryModel.CategoryID == 0)
                {
                    HttpResponseMessage response = await _Client.PostAsync($"{_Client.BaseAddress}/Category/Post", fromdata);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Person Inserted";
                        return RedirectToAction("GET");
                    }
                }
                else
                {
                    HttpResponseMessage response = await _Client.PutAsync($"{_Client.BaseAddress}/Category/Put?CategoryID={categoryModel.CategoryID}", fromdata);
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



        public IActionResult Export()
        {
            try
            {
                HttpResponseMessage response = _Client.GetAsync($"{_Client.BaseAddress}/Category/CategoryExport").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a byte array
                    byte[] pdfBytes = response.Content.ReadAsByteArrayAsync().Result;

                    // Return the PDF file
                    return File(pdfBytes, "application/pdf", "user_data.pdf");
                }
                else
                {
                    // Handle unsuccessful response here
                    if (response.Content != null)
                    {
                        string responseContent = response.Content.ReadAsStringAsync().Result;
                        return Content($"Failed to export PDF. Server returned: {responseContent}");
                    }
                    else
                    {
                        return NotFound("No data available to export.");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request errors
                return StatusCode(500, $"Failed to export PDF. HTTP request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        private byte[] GeneratePdf(List<CategoryModel> persons)
        {
            // Your PDF generation logic here
            // Replace the following placeholder code with your actual PDF generation logic

            using (MemoryStream ms = new MemoryStream())
            {
                // Placeholder: write some PDF content to the memory stream
                // For example:
                var pdfContent = "PDF Content goes here";
                var pdfBytes = System.Text.Encoding.UTF8.GetBytes(pdfContent);
                ms.Write(pdfBytes, 0, pdfBytes.Length);
                return ms.ToArray();
            }
        }

    }
}
