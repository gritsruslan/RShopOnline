using RShopAPI_Test.Core.Enums;

namespace RShopAPI_Test.Services.Interfaces;

public interface IRoleService
{
    public Task<Role?> GetUserRoleById(Guid id, CancellationToken ct);
}