using System.Collections.Generic;

namespace PM.Repository.Models
{
    public class ProductGroup
    {
        public int ProductGroupId { get; set; }
        public string Name { get; set; }
        public int? ParentProductGroupId { get; set; }
        public virtual ProductGroup Parent { get; set; }
        public virtual List<ProductGroup> Children { get; set; } //TODO: ICollection vs HashSet
    }
}
