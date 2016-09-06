using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5Basic.ViewModels
{
    public class AssignedCategoryViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }
}