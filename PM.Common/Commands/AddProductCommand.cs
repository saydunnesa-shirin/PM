using System;
using System.Collections.Generic;

namespace PM.Common.Commands
{
    public class AddProductCommand
    {
        public string Name { get; set; }
        public int ProductGroupId { get; set; }
        public DateTime EntryTime { get; set; }
        public decimal Price { get; set; }
        public int VatRate { get; set; }
        public decimal PriceWithVat { get; set; }
        public List<int> StoreIds { get; set; }
    }
}
