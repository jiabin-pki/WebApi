using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Birthday { get; set; }
        public int Age { get; set; }
    }
}
