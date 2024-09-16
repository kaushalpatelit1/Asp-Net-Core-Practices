using AutoMapper;
using USA_REST_Api.Controllers;
using USA_REST_Api.Models;
using USAApi.Controllers;
using USAApi.Models;

namespace USAApi.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<RoomEntity, Room>()
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate / 100m))
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src => Link.To(
                    nameof(RoomsController.GetRoomById), new { roomId = src.Id })))
                .ForMember(dest => dest.Book, opt => opt.MapFrom(src => 
                    FormMetadata.FromModel(new BookingForm(),
                    Link.ToForm(
                            nameof(Controllers.RoomsController.CreateBookingForRoom),
                            Link.PostMethod,
                            new {roomId = src.Id},
                            Form.CreateRalation))));

            CreateMap<OpeningEntity, Opening>()
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate / 100m))
                .ForMember(dest => dest.StartAt, opt => opt.MapFrom(src => src.StartAt))
                .ForMember(dest => dest.EndAt, opt => opt.MapFrom(src => src.EndAt))
                .ForMember(dest => dest.Room, opt => opt.MapFrom(src => Link.To(
                    nameof(RoomsController.GetRoomById), new { roomId = src.RoomId })));

            CreateMap<BookingEntity, Booking>()
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total / 100m))
                .ForMember(dest => dest.Room, opt => opt.MapFrom(src => Link.To(
                    nameof(RoomsController.GetRoomById), new { roomId = src.Id })))
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src => Link.To(
                    nameof(BookingsController.GetBookingById), new { bookingId = src.Id })));

            CreateMap<UserEntity, User>()
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
                    Link.To(nameof(UsersController.GetUserById),
                    new { userId = src.Id })));
        }
    }
}
