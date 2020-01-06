using AutoMapper;
using DC = MartiCase.API.DataContracts;
using S = MartiCase.Converters.Model;

namespace MartiCase.IoC.Configuration.AutoMapper.Profiles
{
    public class APIMappingProfile : Profile
    {
        public APIMappingProfile()
        {
            CreateMap<DC.Requests.FileRequest, S.ExecutionModel>()
                .ForMember(x => x.File, opt => opt.Ignore())
                .ForMember(x => x.Extension, opt => opt.Ignore())
                .ForMember(x => x.Name, opt => opt.Ignore())
              .ReverseMap()
                .ForMember(x => x.FileToUpload, opt => opt.Ignore());
        }
    }

}
