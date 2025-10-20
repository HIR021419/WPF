using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManager.Models
{
    class Tag
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int PhotoId { get; set; }
        public Image Photo { get; set; }
    }
}
