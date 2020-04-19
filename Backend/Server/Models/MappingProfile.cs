using AutoMapper;
using Grpc.Core;
using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<(PostMessageRequest Request, string ConnectionId, IServerStreamWriter<MessageResponse> ResponseStream), Participant>()
            .ForMember(dest => dest.ConnectionId, opt => opt.MapFrom(src => src.ConnectionId))
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Request.Login))
            .ForMember(dest => dest.ResponseStream, opt => opt.MapFrom(src => src.ResponseStream));

            CreateMap<PostMessageRequest, ChatEntry>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<ChatEntry, MessageResponse>()
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.SpecifyKind(src.DateCreated, DateTimeKind.Utc))));

            CreateMap<(Participant Participant, ChatRoomActivity Activity), MessageResponse>()
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => $"{src.Participant.Login} - {src.Activity}"))
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => "System"))
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc))));

            CreateMap<IEnumerable<ChatEntry>, GetMessagesResponse>()
            .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src));
        }
    }
}
