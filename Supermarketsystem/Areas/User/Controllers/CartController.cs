using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Supermarketsystem.Areas.Admin.Models;
using Supermarketsystem.Areas.User.Models;
using Supermarketsystem.BAL;

namespace Supermarketsystem.Areas.User.Controllers
{
	[CheckAccess]
	[Area("User")]
	[Route("User/{Controller}/{Action}")]
	public class CartController : Controller
	{
		Uri baseuri = new Uri("http://localhost:5071/api");
		private readonly HttpClient _Client;
		public CartController()
		{
			_Client = new HttpClient();
			_Client.BaseAddress = baseuri;
		}
		public IActionResult CartList(int UserID)
		{

			List<CartModel> Dropdown = new List<CartModel>();
			HttpResponseMessage response1 = _Client.GetAsync($"{_Client.BaseAddress}/Cart/GetByID/{UserID}").Result;
			if (response1.IsSuccessStatusCode)
			{

				string data = response1.Content.ReadAsStringAsync().Result;
				dynamic jsonobject = JsonConvert.DeserializeObject(data);
				var dataofobject = jsonobject.data;
				var extractedDtaJson = JsonConvert.SerializeObject(dataofobject, Formatting.Indented);
				Dropdown = JsonConvert.DeserializeObject<List<CartModel>>(extractedDtaJson);
			}
			return View(Dropdown);
		}


		[HttpPost]
		[Route("Cart/Save")]
		public async Task<IActionResult> Save(CartModel cartModel, int UserID)
		{
			try
			{
				MultipartFormDataContent fromdata = new MultipartFormDataContent();
				fromdata.Add(new StringContent(Convert.ToString(cartModel.ProductID)), "ProductID");
				fromdata.Add(new StringContent(Convert.ToString(cartModel.UserID)), "UserID");

				if (cartModel.CartID == 0)
				{
					HttpResponseMessage response = await _Client.PostAsync($"{_Client.BaseAddress}/Cart/Post", fromdata);
					if (response.IsSuccessStatusCode)
					{
						TempData["Message"] = "Person Inserted";
						return RedirectToAction("CartList", "Cart", new { UserID = UserID });
					}
				}

			}
			catch (Exception ex)
			{
				TempData["Error"] = "An Error Occured" + ex.Message;
			}
			return RedirectToAction("CartList");
		}

		/*increment */
		public async Task<IActionResult> Edit(int ProductID, int UserID)
		{
			try
			{
				MultipartFormDataContent fromdata = new MultipartFormDataContent();
				fromdata.Add(new StringContent(Convert.ToString(ProductID)), "ProductID");
                
                HttpResponseMessage response = await _Client.PutAsync($"{_Client.BaseAddress}/Cart/Increment?ProductID={ProductID}", fromdata);
				if (response.IsSuccessStatusCode)
				{
					TempData["Message"] = "Person updated";
					return RedirectToAction("CartList");
				}

			}
			catch (Exception ex)
			{
				TempData["Error"] = "An Error Occured" + ex.Message;
			}
			return RedirectToAction("CartList");
		}

		/*decrement*/
        public async Task<IActionResult> Decrement(int ProductID, int UserID)
        {
            try
            {
                MultipartFormDataContent fromdata = new MultipartFormDataContent();
                fromdata.Add(new StringContent(Convert.ToString(ProductID)), "ProductID");

                HttpResponseMessage response = await _Client.PutAsync($"{_Client.BaseAddress}/Cart/Decrement?ProductID={ProductID}", fromdata);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Person updated";
                    return RedirectToAction("CartList");
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "An Error Occured" + ex.Message;
            }
            return RedirectToAction("CartList");
        }


        [Route("Cart/Delete")]
		public IActionResult Delete(int CartID, int UserID)
		{
			HttpResponseMessage response = _Client.DeleteAsync($"{_Client.BaseAddress}/Cart/Delete?CartID={CartID}").Result;
			if (response.IsSuccessStatusCode)
			{
				TempData["Message"] = "Person Delete";
			}
			else
			{
				TempData["Message"] = "Error deleting person. Please try again.";
			}

			return RedirectToAction("CartList", "Cart", new { UserID = UserID });

		}

		[HttpGet]
		
		public IActionResult CartDetail(int CartID)
		{
			/*IEnumerable<CartModel> cartItems = new List<CartModel>();*/
			var cartmodel = new CartModel();
			HttpResponseMessage response = _Client.GetAsync($"{_Client.BaseAddress}/Cart/GetByCartID/{CartID}").Result;
			if (response.IsSuccessStatusCode)
			{

				string data = response.Content.ReadAsStringAsync().Result;
				dynamic jsonobject = JsonConvert.DeserializeObject(data);
				var dataofobject = jsonobject.data;
				var extractedDtaJson = JsonConvert.SerializeObject(dataofobject, Formatting.Indented);
				 cartmodel= JsonConvert.DeserializeObject<CartModel>(extractedDtaJson);
			}


			return View("CartDetail", cartmodel);

		}



	}
}
