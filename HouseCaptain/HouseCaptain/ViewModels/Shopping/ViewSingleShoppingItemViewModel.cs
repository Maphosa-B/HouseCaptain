using System;
using System.Collections.Generic;
using System.Text;

namespace HouseCaptain.ViewModels.Shopping
{
    public class ViewSingleShoppingItemViewModel:MyBaseViewModel
    {
        public String Name { get; set; } = "Shoes";
        public String Notes { get; set; } = "Please buy right brand";
        public String ImgUrl { get; set; } = "Shoe.jpg";
        public int Quantity { get; set; } = 3;

        public ViewSingleShoppingItemViewModel()
        {

        }

    }
}
