namespace DiyorMarket.Helper
{
    public class ResourceLink
    {
        public string? Rel { get; set; }
        public string? Href { get; set; }
        public string? Method { get; set; }

        public ResourceLink()
        {
        }

        public ResourceLink(string? rel, string? href)
        {
            Rel = rel;
            Href = href;
        }

        public ResourceLink(string? rel, string? href, string? method) 
            : this(rel, href)
        {
            Method = method;
        }
    }
}
