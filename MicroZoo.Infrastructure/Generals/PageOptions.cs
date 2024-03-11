namespace MicroZoo.Infrastructure.Generals
{
    public class PageOptions
    {
        public int PageNumber {  get; set; }
        public int ItemsOnPage { get; set; }

        public PageOptions(int pageNumber, int itemsOnPage)
        {
            PageNumber = pageNumber;
            ItemsOnPage = itemsOnPage;
        }
    }
}
