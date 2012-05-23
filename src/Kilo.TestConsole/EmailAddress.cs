using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kilo.TestConsole
{
    public class EmailAddress
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Value { get; set; }
    }
}
