using System.Collections.Generic;

namespace PM.Repository.Models
{
    public class Store
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}