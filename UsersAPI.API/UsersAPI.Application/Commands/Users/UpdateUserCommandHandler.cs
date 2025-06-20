using MediatR;
using OpenQA.Selenium;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly ApplicationDbContext _context;

    public UpdateUserCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(request.Id);
        if (user == null)
        {
            throw new NotFoundException("Usuário não encontrado");
        }

        user.Name = request.Name;
        user.Email = request.Email;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}