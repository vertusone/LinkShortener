namespace Links.Models.View
{
    public class LinkModel : Link
    {
        //private HttpRequest _request;
        private string _hostPrefix;

        public string ShortUrl =>
            string.Concat(_hostPrefix, "/", UrlKey);

        public static LinkModel FromEntity(Link model, string hostPrefix) =>
            new LinkModel
            {
                _hostPrefix = hostPrefix,
                Id = model.Id,
                LongUrl = model.LongUrl,
                UrlKey = model.UrlKey,
                InsertionDate = model.InsertionDate,
                RedirectsCount = model.RedirectsCount
            };
    }
}