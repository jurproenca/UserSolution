using MediatR;
using OpenQA.Selenium;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteUserCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(request.Id);
        if (user == null)
        {
            throw new NotFoundException("Usuário não encontrado");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }
}