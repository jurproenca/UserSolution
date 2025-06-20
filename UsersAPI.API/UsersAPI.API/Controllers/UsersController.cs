using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        => Ok(await _mediator.Send(new GetAllUsersQuery()));

    [HttpPost]
    public async Task<ActionResult<int>> CreateUser([FromBody] CreateUserCommand command)
        => Ok(await _mediator.Send(command));

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await _mediator.Send(command);
        return NoContent(); // HTTP 204 (sucesso sem conteúdo)
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _mediator.Send(new DeleteUserCommand { Id = id });
        return NoContent(); // Retorna HTTP 204 (sucesso sem conteúdo)
    }

}