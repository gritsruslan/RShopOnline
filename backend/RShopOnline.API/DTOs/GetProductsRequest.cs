namespace RShopAPI_Test.DTOs;

public record GetProductsRequest(Guid CategoryId, int Page, int PageSize, string OrderByField, string OrderDirection);