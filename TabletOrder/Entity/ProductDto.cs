using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TabletOrder.Entity
{
    [Serializable]
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } 
        public int ProductGroupId { get; set; }
        public decimal ProductPrice { get; set; }   
        public int Stockref { get; set; }
        public int Qty { get; set; }

        public decimal NetAmount
        {
            get
            {
                if (Qty > 0)
                {
                    return Qty * ProductPrice;
                }

                return 0;
            }
        }

        private ProductGroupDto _productGroup;
        public ProductGroupDto ProductGroup
        {
            get
            {
                if (_productGroup == null)
                {
                    _productGroup = StaticValues.ProductGroups.
                      FirstOrDefault(x => x.GroupID == ProductGroupId.ToString());
                }

                return _productGroup;
            }
        }


    }
}