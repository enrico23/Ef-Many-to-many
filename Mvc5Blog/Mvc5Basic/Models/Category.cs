using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5Basic.Models
{
    public class Category
    {
        public Category()
        {
            this.posts = new HashSet<Post>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Post> posts { get; set; }
    }
}