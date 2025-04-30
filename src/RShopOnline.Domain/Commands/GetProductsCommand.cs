namespace RShopAPI_Test.Services.Commands;

public record GetProductsCommand(Guid CategoryId, int Page, int PageSize, string OrderByField, bool Ascending);