using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam2.Data.Entities
{
    public class Dish : BaseEntity
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Desc { get; set; }
        public int RestaurantId { get; set; }
    }
}
