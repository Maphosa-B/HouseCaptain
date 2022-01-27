using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseCaptain.Entities
{
    public class ShoppingItemEntity: BaseEntity
    {
        public String Name { get; set; }
        public int Quantity { get; set; }
        public String  QuantityType { get; set; }
        public String Category { get; set; }
        public String Notes { get; set; }
        public int HomeId  { get; set; }
        public String ImgUrl  { get; set; }
    }
}
