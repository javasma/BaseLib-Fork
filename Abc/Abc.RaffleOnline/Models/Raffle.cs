using System;

namespace Abc.RaffleOnline.Models
{
    public class Raffle
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset BeginsOn { get; set; }
        public DateTimeOffset EndsOn { get; set; }
        
    }
}