namespace RShopAPI_Test.Services.UseCases.GetProducts;

public record GetProductsCommand(Guid CategoryId, int Page, int PageSize, string OrderByField, string OrderDirection);