using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA_mid_project
{
    public class Post
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public int CommentsCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
