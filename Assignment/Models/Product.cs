﻿using System.ComponentModel.DataAnnotations;

namespace Assignment.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }

        
        public string? ImagePath { get; set; }
    }
}
