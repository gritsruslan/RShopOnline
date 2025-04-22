using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.Authentication;

public class AuthenticationService(IUsersRepository repository) : IAuthenticationService
{
    public async Task<IIdentity> Authenticate(Guid id)
    {
        var user = await repository.GetUserById(id, CancellationToken.None);

        if (user is null)
        {
            return RecognizedUser.Guest;
        }

        return new RecognizedUser(user.Id, user.Role);
    }
}