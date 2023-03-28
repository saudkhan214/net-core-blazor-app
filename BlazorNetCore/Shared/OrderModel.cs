using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorNetCore.Shared
{
    public class OrderModel
    {
        public decimal OrderID { get; set; }
        public int? OrderStatus { get; set; }
    }
}
