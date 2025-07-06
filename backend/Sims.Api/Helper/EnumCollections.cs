namespace Sims.Api.Helper
{
    public enum RoleEnums
    {
        SuperAdmin = 0,
        Admin = 1,
        Staff = 2,
        Others = 3
    }
    public enum PurchaseOrderStatus
    {
        Pending = 0, //Default status after order is created but not yet processed
        Received = 1, //All items in the order have been received and added to inventory
        Cancelled = 2, //Order was cancelled before completion
        Returned = 3 //Order was received but then returned (defects, mismatch, etc.)
    }

    public enum SalesStatus
    {
        Completed = 0,         // Payment received, inventory adjusted
        Refunded = 1,          // Full refund, inventory restored
        PartialRefund = 2,     // Some items refunded, partial inventory adjustment
        Cancelled = 3,         // Sale cancelled before fulfillment, no items shipped
        Pending = 4,           // Awaiting payment confirmation or manual approval
        InProgress = 5,        // Being prepared or shipped (e.g., warehouse picking)
        Delivered = 6,         // Goods delivered to customer
        Failed = 7             // Payment failed or system error
    }
}
