using System;
using System.Collections.Generic;
using System.Text;

namespace HouseCaptain.Models.Shopping
{
    public class ShoppingItemModel
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Notes { get; set; }
        public int Quantity { get; set; }
        public String ImgUrl { get; set; } = "bb.jpg";
        public String BarCode { get; set; }
        public int CategoryId { get; set; }
    }

}
