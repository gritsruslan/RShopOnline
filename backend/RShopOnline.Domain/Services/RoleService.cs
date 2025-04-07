using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.Services;

public class RoleService(IUsersRepository repository) : IRoleService
{
    public async Task<Role?> GetUserRoleById(Guid id, CancellationToken ct)
    {
        var user = await repository.GetUserById(id, ct);

        return user?.Role ?? null;
    }
}