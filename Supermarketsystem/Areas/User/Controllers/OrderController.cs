using Microsoft.AspNetCore.Mvc;

using Supermarketsystem.Areas.User.Models;
using Supermarketsystem.BAL;

namespace Supermarketsystem.Areas.User.Controllers
{
    [CheckAccess]
    [Area("User")]
	[Route("User/{Controller}/{Action}")]
	public class OrderController : Controller
    {
		Uri baseuri = new Uri("http://localhost:5071/api");
		private readonly HttpClient _Client;
		public OrderController()
		{
			_Client = new HttpClient();
			_Client.BaseAddress = baseuri;
		}

		public IActionResult OrderGet()
		{
			return View("OrderList");
		}

		[HttpPost]
		[Route("Order/Save")]
		public async Task<IActionResult> Save(OrderModel order)
		{
			try
			{
				
				MultipartFormDataContent formDataContent = new MultipartFormDataContent();
				formDataContent.Add(new StringContent(order.CartID.ToString()), "CartID");
				formDataContent.Add(new StringContent(order.UserID.ToString()), "UserID");
				formDataContent.Add(new StringContent(order.OrderDate.ToString()), "OrderDate");



				HttpResponseMessage response = await _Client.PostAsync($"{_Client.BaseAddress}/Order/Post", formDataContent);

				if (response.IsSuccessStatusCode)
				{
					TempData["Message"] = "Person Inserted";
					return RedirectToAction("OrderGet");
				}
				else
				{
					TempData["Message"] = "error occured";

				}


			}
			catch (Exception ex)
			{
				TempData["Error"] = "An Error Occured" + ex.Message;
			}
			return RedirectToAction("CartList");
		}


        public IActionResult Exports(int UserID)
        {
            try
            {
                HttpResponseMessage response = _Client.GetAsync($"{_Client.BaseAddress}/Order/Bill/{UserID}").Result;

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

        private byte[] GeneratePdf(List<BillModel> persons)
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
