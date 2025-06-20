using MediatR;

public class CreateUserCommand : IRequest<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Id { get; internal set; }
}