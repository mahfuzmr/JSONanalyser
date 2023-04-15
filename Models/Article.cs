using System.Globalization;

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
        public virtual double PricePerUnit
        {
            get
            {
                string pricePerUnitText = this.PricePerUnitText;
                string[] priceParts = pricePerUnitText.Split(' ');
                string priceString = priceParts[0].Trim('(', ')');
                double price = double.Parse(priceString.Replace(',', '.'), CultureInfo.InvariantCulture);
                return price;
            }

        }
    }
}
