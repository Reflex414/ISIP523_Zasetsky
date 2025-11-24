using System;
using System.Collections.Generic;

namespace ISIP523_Zasetsky
{
    public partial class Detail
    {
        public int ID { get; set; }
        public string NameDetail { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<DetailsGarage> DetailsGarages { get; set; } = new List<DetailsGarage>();
    }

}