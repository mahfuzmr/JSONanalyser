namespace JSONanalyser.Models
{
    public class Beer
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string Name { get; set; }
        public string descriptionText { get; set; }
        public List<Article> Articles { get; set; }
        public Article Article { get; set; }


    }
}
