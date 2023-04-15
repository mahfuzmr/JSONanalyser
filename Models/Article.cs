namespace JSONanalyser.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string ShortDescription { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public string PricePerUnitText { get; set; }
        public string Image { get; set; }
    }
}
