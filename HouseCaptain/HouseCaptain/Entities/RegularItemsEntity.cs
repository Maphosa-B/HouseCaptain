using System;
using System.Collections.Generic;
using System.Text;

namespace HouseCaptain.Entities
{
    public class RegularItemsEntity:BaseEntity
    {
        public String Name { get; set; }

        public String ImgUrl { get; set; }

        public int  AddCount { get; set; }

        public String QuantityType { get; set; }

        public int HomeId { get; set; }
    }
}
