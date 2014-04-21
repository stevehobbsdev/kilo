using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace Kilo.TestConsole
{
    public class User : TableEntity
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        public EmailAddress EmailAddress { get; set; }
    }
}
