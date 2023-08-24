using LanguageCards.Application.Services.Abstractions;
using LanguageCards.Domain.Entities.Auth;
using LanguageCards.Domain.Entities.User;
using LanguageCards.Domain.ValueObject;
using MediatR;
using MongoDb.Repository;

namespace LanguageCards.Application.Commands.Authorization
{
    internal class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserCredentials> _userCredentialsRepository;
        private readonly IPasswordHashService _passwordHashService;

        public RegisterUserCommandHandler(IRepository<User> userRepository, IRepository<UserCredentials> userCredentialsRepository, IPasswordHashService passwordHashService)
        {
            _userRepository = userRepository;
            _userCredentialsRepository = userCredentialsRepository;
            _passwordHashService = passwordHashService;
        }

        public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.FindOneAsync(x => string.Equals(x.Email.Value, request.Email, StringComparison.OrdinalIgnoreCase), cancellationToken);

            if (existingUser is not null)
            {
                // throw exception
                return;
            }

            var userEmail = new Email(request.Email);

            var newUser = new User(Guid.NewGuid(), request.FirstName, request.LastName, userEmail);

            await _userRepository.InsertOneAsync(newUser, cancellationToken);

            var password = _passwordHashService.GetHash(request.Password, out var salt);

            var newUserCredentials = new UserCredentials(newUser.Id, userEmail.Value, password, salt);

            await _userCredentialsRepository.InsertOneAsync(newUserCredentials, cancellationToken);
        }
    }

    public record RegisterUserCommand(string Email, string Password, string FirstName, string LastName) : IRequest;
}
