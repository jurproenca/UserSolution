using MediatR;

public class GetAllUsersQuery : IRequest<List<UserDto>> { }