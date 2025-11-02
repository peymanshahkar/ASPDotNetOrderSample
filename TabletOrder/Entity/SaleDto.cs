using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace TabletOrder.Entity
{
    [Serializable]
    public class SaleHeaderDto
    {
        public long SaleHID { get; set; }
        public string SaleMali
        {
            get
            {
                return StaticValues.SaleMali;
            }
            
        }
        public long FactNumber { get; set; } = 0;
        /// <summary>
        /// تاریخ شمسی
        /// </summary>
        public string Dt { get; set; }
        public int PersonID { get; set; } = 0;

        /// <summary>
        /// Amount
        /// </summary>
        public decimal SumF { get; set; }
        public decimal Tax { get; set; }
        public decimal Ezafat { get; set; } = 0;
        public decimal Takhfif { get; set; } = 0;
        public Int16 RcvType { get; set; } = 1;
        public decimal Cashrcv { get; set; } = 0;
        public decimal unCashrcv { get; set; } = 0;
        public decimal Mandeh { get; set; }
        public string Miz { get; set; }
        public bool Saloon { get; set; } = true;
        public int Shift { get; set; }
        public bool IsPrintFactor { get; set; } = false;
        public bool IsKitchenPrint { get; set; } = false;


        public void Calc()
        {
            decimal _tax = 0;
            Tax = 0;
            SumF = 0;
            foreach (var item in SaleDetaileS)
            {
                SumF += item.SumPrice;
                if (item.ProductGroup.IsHaveTax)
                {
                    _tax = (StaticValues.TaxPercent / 100) * item.SumPrice;
                    Tax += _tax;
                }
            }

            Mandeh = SumF+Tax;

        }

        //public void SetFactNumber()
        //{

        //}

        public  List<SaleDetailDto> SaleDetaileS { get; set; }
        public List<Data.Parameter> GetParameter()
        {
            List<Data.Parameter> ParamS = new List<Data.Parameter>();

            PropertyInfo[] props = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in props)
            {
                if (p.CanRead && p.Name!= "SaleDetaileS")
                {
                    ParamS.Add(new Data.Parameter
                    {
                        ParameterName = p.Name,
                        ParameterValue = p.GetValue(this).ToString()
                    });
                   
                }
            }

            return ParamS;
        }

    }
    [Serializable]
    public class SaleDetailDto
    {
        public long RowID { get; set; }
        public long HeaderID { get; set; }
        public int PartID { get; set; }
        public int Qty { get; set; }
        public decimal FeePrice { get; set; }
        public decimal SumPrice { get; set; }
        public bool Prnt { get; set; }
        public bool IsDel { get; set; }
        public int NewValue { get; set; }

        public ProductDto Product { get; set; }
        private ProductGroupDto _productGroup;
        public ProductGroupDto ProductGroup
        {
            get
            {
                if (_productGroup == null)
                {
                    _productGroup = StaticValues.ProductGroups.
                        FirstOrDefault(x => x.GroupID == Product.ProductGroupId.ToString());
                }

                return _productGroup;
            }
        }

        public List<Data.Parameter> GetParameter()
        {
            List<Data.Parameter> ParamS = new List<Data.Parameter>();

            PropertyInfo[] props = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in props)
            {
                if (p.CanRead)
                {
                    ParamS.Add(new Data.Parameter
                    {
                        ParameterName = p.Name,
                        ParameterValue = p.GetValue(this).ToString()
                    });

                }
            }

            return ParamS;
        }



        public List<Data.Parameter> GetParameterForUpdate()
        {
            List<Data.Parameter> ParamS = new List<Data.Parameter>();

            PropertyInfo[] props = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in props)
            {
                if (p.CanRead && p.Name!= "HeaderID")
                {
                    ParamS.Add(new Data.Parameter
                    {
                        ParameterName = p.Name,
                        ParameterValue = p.GetValue(this).ToString()
                    });

                }
            }

            return ParamS;
        }


    }
}