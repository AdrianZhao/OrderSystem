﻿namespace OrderSystem.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AvailableQuantity { get; set; }
        public decimal Price { get; set; }
    }
}
