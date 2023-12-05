using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
	public class Order
	{
        public Order()
        {
            OrderDetails = new List<OrderDetails>();
        }

        public int Id { get; set; }

        [Display(Name="Order No")]
        public string OrderNo { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public DateTime OrderDate { get; set; }

        public virtual List<OrderDetails> OrderDetails { get; set; }
    }
}

