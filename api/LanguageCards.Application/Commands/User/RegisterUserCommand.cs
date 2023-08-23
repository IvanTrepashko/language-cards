using MediatR;

namespace LanguageCards.Application.Commands.User
{
    internal class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        public Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public record RegisterUserCommand(string Email, string Password, string FirstName, string LastName) : IRequest;
}
