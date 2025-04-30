namespace RShopAPI_Test.Core.Enums;

public enum OrderStatus
{
    Pending = 0,
    Sent = 1,
    Delivered = 2,
    Completed = 3,
    CanceledByUser = 4,
    CanceledByShop = 5,
}