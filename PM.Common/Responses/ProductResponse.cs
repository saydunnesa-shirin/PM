using System;
using System.Collections.Generic;

namespace PM.Common.Responses
{
    public class ProductResponse
    {
        public string Name { get; set; }
        public string ProductGroupName { get; set; }
        public DateTime EntryTime { get; set; }
        public decimal Price { get; set; }
        public int VatRate { get; set; }
        public decimal PriceWithVat { get; set; }
        public List<StoreResponse> Stores { get; set; }
    }
}
