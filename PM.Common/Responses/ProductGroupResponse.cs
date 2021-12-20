using System;
using System.Collections.Generic;

namespace PM.Common.Responses
{
    public class ProductGroupResponse
    {
        public int ProductGroupId { get; set; }
        public string GroupName { get; set; }
        public List<ProductGroupResponse> Children { get; set; }
    }
}
