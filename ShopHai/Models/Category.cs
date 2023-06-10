using System;
using System.Collections.Generic;

namespace ShopHai.Models;

public partial class Category
{
    public string? CateName { get; set; }

    public int CateId { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
