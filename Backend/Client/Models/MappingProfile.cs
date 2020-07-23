using AutoMapper;
using Server;
using System.Collections.Generic;

namespace Client.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<int, GetMessagesRequest>()
                    .ForMember(dest => dest.LastMessages, opt => opt.MapFrom(src => src));

            CreateMap<GetMessagesResponse, IEnumerable<ChatEntry>>()
                .ConvertUsing(new MessagesCustomConverter());

            CreateMap<MessageResponse, ChatEntry>()
                    .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated.ToDateTime()));
        }

        public class MessagesCustomConverter : ITypeConverter<GetMessagesResponse, IEnumerable<ChatEntry>>
        {
            private IEnumerable<ChatEntry> GetMessages(GetMessagesResponse source, ResolutionContext context)
            {
                foreach (var product in source.Messages)
                {
                    yield return context.Mapper.Map<ChatEntry>(product);
                }
            }

            public IEnumerable<ChatEntry> Convert(GetMessagesResponse source, IEnumerable<ChatEntry> destination, ResolutionContext context)
            {
                return GetMessages(source, context);
            }
        }
    }
}
