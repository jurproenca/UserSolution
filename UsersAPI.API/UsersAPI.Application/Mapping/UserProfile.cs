using AutoMapper;

public class UserProfile : Profile
{
    public UserProfile() => CreateMap<Users, UserDto>();
}