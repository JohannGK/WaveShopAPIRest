using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WaveShopAPIRest.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string OpinionResume { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Visible { get; set; } = null!;
        public string? PhotoAddress { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public DateTime Published { get; set; }
        public int? IdProduct { get; set; }
        public int? IdComment { get; set; }

        [JsonIgnore]
        public virtual Product? IdProductNavigation { get; set; }
    }
}
