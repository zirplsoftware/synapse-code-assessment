using Synapse.Domain.Services.Orders;

var orderProcessingService = new OrderProcessingService
{
    OrderService = new OrderService()
};

// NOTE: I personally prefer logging of exceptions to occur
// at the highest level possible, which is usually before
// they are eaten. Often, but not always, the entry point.
// In this case, logging is appearing throughout the code
// since that approach requires more thoughtful design and more time
// than an evaluation allows.
//
// This method logs and eats all exceptions.
// This is why it is prefaced with Try.
// Though it returns no bool indicating success
// simply because there are no requirements for it.
await orderProcessingService.TryProcessDeliveredOrderItemsAsync();