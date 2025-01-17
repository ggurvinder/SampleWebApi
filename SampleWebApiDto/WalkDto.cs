﻿using SampleWebApiDto.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleWebApiDto
{
    public class WalkDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        //public Guid DifficultyId { get; set; }
        //public Guid RegionId { get; set; }

        public RegionDto Region { get; set; }
        public DifficultyDto Difficulty { get; set; }


    }
}
