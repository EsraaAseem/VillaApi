﻿namespace VillaApi.Model.modelDto
{
    public class VillaUpdateDto
    {
        public Guid villaId { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
    }
}