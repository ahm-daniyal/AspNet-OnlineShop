using System;
namespace OnlineShop.Services
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly IConfiguration configuration;

		public UnitOfWork(IConfiguration configuration)
		{
			this.configuration = configuration;
            PaypalServices = new PaypalServices(configuration);
		}

        public IPaypalServices PaypalServices { get; private set; }
    }
}

