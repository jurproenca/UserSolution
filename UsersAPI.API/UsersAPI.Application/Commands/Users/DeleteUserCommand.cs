using MediatR;

public class DeleteUserCommand : IRequest
{
    public int Id { get; set; }
}