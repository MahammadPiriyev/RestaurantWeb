using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Models
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCartVM> ShoppingCartList { get; set; }
        public double OrderTotal { get; set; }
    }
}
