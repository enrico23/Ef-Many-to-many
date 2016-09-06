using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5Basic.Models
{
    public class Post
    {
        public Post()
        {
            this.categories = new HashSet<Category>();
        }
        public int PostId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Category> categories { get; set; }
    }
}