using System;
using System.Collections.Generic;
using System.Text;

namespace Abc.Financial.Models
{
    public class Instrument
    {
        public DateTimeOffset IssueDate { get; set; }
        public string Id { get; set; }
        public string UUID { get; set; }
        public decimal Amount { get; set; }
    }
}
