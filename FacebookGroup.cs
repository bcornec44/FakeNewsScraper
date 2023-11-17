using System.ComponentModel.DataAnnotations.Schema;

namespace FakeNewsScraper
{
    [Table("facebook_group")]
    public class FacebookGroup
    {
        public string Content { get; set; }
        public string Summary { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
    }

}
