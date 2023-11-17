using System.ComponentModel.DataAnnotations.Schema;

namespace FakeNewsScraper
{
    [Table("facebook_group_post")]
    public class FacebookGroupPost
    {
        public string GroupLink { get; set; }
        public string Summary { get; set; }
        public string Text { get; set; }
        public string Link { get; set; }
    }

}
