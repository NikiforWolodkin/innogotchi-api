﻿namespace DataLayer.Models
{
    public class FeedingAndQuenching
    {
        public Guid Id { get; set; }
        public DateTime FeedingTime { get; set; }
        public DateTime QuenchingTime { get; set; }
        public int? UnhappyDays { get; set; }
        public int? FeedingPeriod { get; set; }
        public int? QuenchingPeriod { get; set; }
    }
}