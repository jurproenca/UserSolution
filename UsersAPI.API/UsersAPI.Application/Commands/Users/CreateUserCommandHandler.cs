using MediatR;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly ApplicationDbContext _context;
    public CreateUserCommandHandler(ApplicationDbContext context) => _context = context;

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new Users{ Name = request.Name, Email = request.Email };
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}