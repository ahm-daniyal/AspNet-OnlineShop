using System;
using PayPal.Api;

namespace OnlineShop.Services
{
	public interface IPaypalServices
	{
		Task<Payment> CreateOrderAsync(decimal amount, string returnUrl, string cancelUrl);
	}
}

