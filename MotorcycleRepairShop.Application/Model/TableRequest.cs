using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRepairShop.Application.Model
{
    public class TableRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? Keyword { get; set; }
    }
}
