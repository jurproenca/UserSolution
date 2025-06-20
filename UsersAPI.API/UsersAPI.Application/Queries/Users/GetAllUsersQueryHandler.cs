using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetAllUsersQueryHandler(ApplicationDbContext context, IMapper mapper)
        => (_context, _mapper) = (context, mapper);

    public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        => _mapper.Map<List<UserDto>>(await _context.Users.ToListAsync(cancellationToken));
}