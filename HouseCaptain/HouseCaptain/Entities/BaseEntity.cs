using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseCaptain.Entities
{
    public class BaseEntity
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public int Status { get; set; } = 1;
        public DateTime AddDate { get; set; } = DateTime.Now;
        public DateTime LastModificationDate { get; set; } = DateTime.Now;
    }
}
