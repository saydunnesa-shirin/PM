using System;
using System.Collections.Generic;

namespace PM.Repository.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int ProductGroupId { get; set; }
        public virtual ProductGroup ProductGroup { get; set; }
        public DateTime EntryTime { get; set; }
        public decimal Price { get; set; }
        public int VatRate { get; set; }
        public decimal PriceWithVat { get; set; }
        public ICollection<Store> Stores { get; set; }
    }
}
