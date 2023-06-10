using System;
using System.Collections.Generic;

namespace ShopHai.Models;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public int? CartId { get; set; }

    public int? UserId { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public int? Status { get; set; }

    public virtual Cart? Cart { get; set; }

    public virtual User? User { get; set; }
}
