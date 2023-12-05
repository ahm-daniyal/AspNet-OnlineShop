using System;
using PayPal.Api;

namespace OnlineShop.Services
{
	public class PaypalServices : IPaypalServices
	{
        private readonly APIContext apiContext;
        private readonly Payment payment;
        private readonly IConfiguration configuration;

		public PaypalServices(IConfiguration configuration)
		{
			this.configuration = configuration;
			var clientId = "AUy44hyjBD3wiK7oZPuZPvHuNXKQuZJaODf4eICGVYxRAa5NKkv1OYryAgWu9jQUahmy5Kuscgry-bM-";
            var clientSecret = "EN8y8w7uN2iobtoyxDbIrcR1491TeSnDT5wgCsCvgFTNjMm1NW2bQxXiJPuyWasROMZJ62tooI_bdAp5";

			var config = new Dictionary<string, string>
			{
				{ "mode", "sandbox" },
				{ "clientId", clientId },
				{ "clientSecret", clientSecret }
			};

			var accessToken = new OAuthTokenCredential(clientId, clientSecret, config).GetAccessToken();
			apiContext = new APIContext(accessToken);

			payment = new Payment
			{
				intent = "sale",
				payer = new Payer { payment_method = "paypal" }
			};
        }

        public async Task<Payment> CreateOrderAsync(decimal amount, string returnUrl, string cancelUrl)
        {
			var apiContext = new APIContext(new OAuthTokenCredential(
				configuration["PayPal:ClientId"],
				configuration["PayPal:ClientSecret"]).GetAccessToken());

			var itemList = new ItemList()
			{
				items = new List<Item>()
				{
					new Item()
					{
						name = "Membership Fee",
						currency = "USD",
						price = amount.ToString("0.00"),
						quantity = "1",
						sku = "membership"
					}
				}
			};

			var transaction = new Transaction()
			{
				amount = new Amount()
				{
					currency = "USD",
					total = amount.ToString("0.00"),
					details = new Details()
					{
						subtotal = amount.ToString("0.00")
					}
				},
				item_list = itemList,
				description = "Membership Fee"
			};

			var payment = new Payment()
			{
				intent = "sale",
				payer = new Payer() { payment_method = "paypal" },
				redirect_urls = new RedirectUrls()
				{
					return_url = returnUrl,
					cancel_url = cancelUrl
				},
				transactions = new List<Transaction>() { transaction }
			};

			var createdPayment = payment.Create(apiContext);

            return createdPayment;
        }
    }
}

