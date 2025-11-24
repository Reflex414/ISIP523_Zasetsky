namespace ISIP523_Zasetsky
{
    public partial class DetailsGarage
    {
        public int ID { get; set; }
        public int DetailsID { get; set; }
        public int GarageID { get; set; }
        public int Count { get; set; }

        // Навигационные свойства
        public virtual Detail Details { get; set; }
        public virtual Garage Garage { get; set; }
    }
}