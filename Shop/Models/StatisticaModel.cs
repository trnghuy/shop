﻿namespace Shop.Models
{
    public class StatisticaModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int Sold { get; set; }
        public int Revenue { get; set; }
        public int Profit { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
