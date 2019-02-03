namespace E_Shop_Engine.Website.Areas.Admin.Models
{
    public class Query
    {
        public int? Page { get; set; }

        public string SortOrder { get; set; }

        public string search;

        public bool descending = false;

        public bool Reversable { get; set; } = false;
    }
}