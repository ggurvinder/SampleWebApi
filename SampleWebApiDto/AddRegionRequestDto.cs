﻿using System.ComponentModel.DataAnnotations;

namespace SampleWebApiDto.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3,ErrorMessage = "Code has to be a minimum of 3 characters")]
        [MaxLength(3, ErrorMessage = "Code has to be a maximum of 3 characters")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100,ErrorMessage = "Code has to be a maximum of 100 characters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }

        //public double Area { get; set; }
        //public double Lat { get; set; }
        //public double Long { get; set; }
        //public long Population { get; set; }
    }
}
