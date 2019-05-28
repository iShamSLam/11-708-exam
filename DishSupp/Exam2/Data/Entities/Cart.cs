using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam2.Data.Entities
{
    public class Cart : BaseEntity
    {
        public int UserId { get; set; }
        public IEnumerable<Dish> Dishes { get; set; }
        public int TotalPrice { get; set; }
    }
}
