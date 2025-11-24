using System;
using System.Collections.Generic;

namespace ISIP523_Zasetsky
{
    public partial class Garage
    {
        public int ID { get; set; }
        public string NameGarage { get; set; }
        public decimal Balance { get; set; }

        public virtual ICollection<DetailsGarage> DetailsGarages { get; set; } = new List<DetailsGarage>();
    }
}