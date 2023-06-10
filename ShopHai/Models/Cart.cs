using System;
using System.Collections.Generic;

namespace ShopHai.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public decimal? Total { get; set; }

    public bool IsPaid { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
