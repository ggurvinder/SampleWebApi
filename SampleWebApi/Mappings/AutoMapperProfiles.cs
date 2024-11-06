using AutoMapper;
using SampleWebApi.Models.Domain;
using SampleWebApiDto;
using SampleWebApiDto.Models.DTO;

namespace SampleWebApi.Mappings
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            //Incase properties are different
            //CreateMap<UserDomain, UserDto>()
            //    .ForMember(x=>x.Name,opt=>opt.MapFrom(x=>x.FullName))
            //    .ReverseMap();

            CreateMap<Models.Domain.Region, RegionDto>().ReverseMap();
            CreateMap<Models.Domain.Region, AddRegionRequestDto>().ReverseMap();
            CreateMap<Models.Domain.Region, UpdateRegionRequestDto>().ReverseMap();

            


            CreateMap<Models.Domain.Walk, AddWalkRequestDto>().ReverseMap();
            CreateMap<Models.Domain.Walk, UpdateWalkRequestDto>().ReverseMap();
            

            CreateMap<Models.Domain.Walk, WalkDto>().ReverseMap();
            CreateMap<Models.Domain.Difficulty, DifficultyDto>().ReverseMap();



        }




    }


    public class UserDomain
    {
        public string FullName { get; set; }
    }

    public class UserDto
    {
        public string Name { get; set; }
    }

}
