using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using Supermarketsystem.Areas.Admin.Models;
using Supermarketsystem.BAL;
using System.ComponentModel;

namespace Supermarketsystem.Areas.Admin.Controllers
{
    [CheckAccess]
    [Area("Admin")]
    [Route("Admin/{Controller}/{Action}")]
    public class SubCategoryController : Controller
    {
        Uri baseuri = new Uri("http://localhost:5071/api");
        private readonly HttpClient _Client;
      
        public SubCategoryController()
        {
            _Client = new HttpClient();
            _Client.BaseAddress = baseuri;
           
        }
        public IActionResult GET()
        {
            List<SubCategoryModel> subCategories = new List<SubCategoryModel>();
            HttpResponseMessage response = _Client.GetAsync($"{_Client.BaseAddress}/SubCategory/Get").Result;
            if (response.IsSuccessStatusCode)
            {

                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonobject = JsonConvert.DeserializeObject(data);
                var dataofobject = jsonobject.data;
                var extractedDtaJson = JsonConvert.SerializeObject(dataofobject, Formatting.Indented);
                subCategories = JsonConvert.DeserializeObject<List<SubCategoryModel>>(extractedDtaJson);
            }
            return View("SubCategoryList", subCategories);
        }
    


        public IActionResult Delete(int SubCategoryID)
        {
            HttpResponseMessage response = _Client.DeleteAsync($"{_Client.BaseAddress}/SubCategory/Delete?SubCategoryID={SubCategoryID}").Result;
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
        public IActionResult Edit(int SubCategoryID)
        {

            /*Dropdown start*/
            List<Categorydropdown> Dropdown = new List<Categorydropdown>();
            HttpResponseMessage response1 = _Client.GetAsync($"{_Client.BaseAddress}/SubCategory/GetDropdown").Result;
            if (response1.IsSuccessStatusCode)
            {

                string data = response1.Content.ReadAsStringAsync().Result;
                dynamic jsonobject = JsonConvert.DeserializeObject(data);
                var dataofobject = jsonobject.data;
                var extractedDtaJson = JsonConvert.SerializeObject(dataofobject, Formatting.Indented);
                Dropdown = JsonConvert.DeserializeObject<List<Categorydropdown>>(extractedDtaJson);
            }
            ViewBag.Dropdown = Dropdown;

            /*Dropdown end*/

           
            
                SubCategoryModel subCategory = new SubCategoryModel();
                HttpResponseMessage response = _Client.GetAsync($"{_Client.BaseAddress}/SubCategory/GetById/{SubCategoryID}").Result;
                if (response.IsSuccessStatusCode)
                {

                    string data = response.Content.ReadAsStringAsync().Result;
                    dynamic jsonobject = JsonConvert.DeserializeObject(data);
                    var dataofobject = jsonobject.data;
                    var extractedDtaJson = JsonConvert.SerializeObject(dataofobject, Formatting.Indented);
                    subCategory = JsonConvert.DeserializeObject<SubCategoryModel>(extractedDtaJson);
                }


                return View("SubCategoryAdd", subCategory);
            
        }








        [HttpPost]
        public async Task<IActionResult> Save(SubCategoryModel subCategoryModel)
        {
            try
            {
                MultipartFormDataContent fromdata = new MultipartFormDataContent();
                fromdata.Add(new StringContent(Convert.ToString(subCategoryModel.CategoryID)), "CategoryID");
                fromdata.Add(new StringContent(subCategoryModel.SubCategoryName), "SubCategoryName");
                fromdata.Add(new StringContent(subCategoryModel.SubCategoryImage), "SubCategoryImage");
                if (subCategoryModel.SubCategoryID == 0)
                {
                    HttpResponseMessage response = await _Client.PostAsync($"{_Client.BaseAddress}/SubCategory/Post", fromdata);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Person Inserted";
                        return RedirectToAction("GET");
                    }
                }
                else
                {
                    HttpResponseMessage response = await _Client.PutAsync($"{_Client.BaseAddress}/SubCategory/Put?SubCategoryID={subCategoryModel.SubCategoryID}", fromdata);
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

        /*
                [HttpPost]
                public async Task<IActionResult> Save(SubCategoryModel subCategoryModel)
                {
                    try
                    {
                        MultipartFormDataContent formData = new MultipartFormDataContent();
                        formData.Add(new StringContent(subCategoryModel.CategoryID.ToString()), "CategoryID");
                        formData.Add(new StringContent(subCategoryModel.SubCategoryName), "SubCategoryName");

                        // Add the image to the form data
                        if (subCategoryModel.SubCategoryImageFile != null)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await subCategoryModel.SubCategoryImageFile.CopyToAsync(memoryStream);
                                formData.Add(new StreamContent(memoryStream), "SubCategoryImage", subCategoryModel.SubCategoryImageFile.FileName);
                            }
                        }

                        if (subCategoryModel.SubCategoryID == 0)
                        {
                            HttpResponseMessage response = await _Client.PostAsync("api/SubCategory/Post", formData);
                            if (response.IsSuccessStatusCode)
                            {
                                TempData["Message"] = "Person Inserted";
                                return RedirectToAction("GET");
                            }
                        }
                        else
                        {
                            HttpResponseMessage response = await _Client.PutAsync($"api/SubCategory/Put?SubCategoryID={subCategoryModel.SubCategoryID}", formData);
                            if (response.IsSuccessStatusCode)
                            {
                                TempData["Message"] = "Person updated";
                                return RedirectToAction("GET");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = "An Error Occurred" + ex.Message;
                    }
                    return RedirectToAction("GET");
                }
        */


        public IActionResult SubCategoryExport()
        {
            try
            {
                HttpResponseMessage response = _Client.GetAsync($"{_Client.BaseAddress}/SubCategory/SubCategoryExport").Result;

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

        private byte[] GeneratePdf(List<SubCategoryModel> persons)
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

