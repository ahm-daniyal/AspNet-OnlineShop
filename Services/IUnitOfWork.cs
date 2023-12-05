using System;
namespace OnlineShop.Services
{
	public interface IUnitOfWork
	{
		IPaypalServices PaypalServices { get; }
	}
}

