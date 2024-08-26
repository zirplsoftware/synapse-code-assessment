using System;
using System.Threading.Tasks;
using Synapse.Domain.Models.Orders;
using Synapse.Logging;

namespace Synapse.Domain.Services.Orders
{
    public class OrderProcessingService : IOrderProcessingService
    {
        // NOTE: this would be injected via Autofac or another DI container
        // or can be mocked in a unit test
        public IOrderService OrderService { get; set; }

        public async Task TryProcessDeliveredOrderItemsAsync()
        {
            MedicalEquipmentOrder[] orders = null;

            try
            {
                orders = await OrderService.GetMedicalEquipmentOrdersAsync();
            }
            catch (Exception ex)
            {
                this.GetLog().Log("Failed to get orders", ex);

                // we can eat this one as long as we log it.
                // The API may be down and the next run of the
                // job will try again.
                // there is now nothing to do below.
            }

            if (orders != null)
            {
                foreach (var order in orders)
                {
                    try
                    {
                        var requiresUpdate = false;
                        foreach (var item in order.Items)
                        {
                            // TODO: CHECK WITH PRODUCT OWNER: Duplicate delivery notifications
                            // there is a bug in the original code that I would be unsure of
                            // how to handle without speaking with the product owner.
                            // It appears to send the delivery alert every time
                            // the job runs once it is in a delivered status.
                            // It keeps track of how many were sent, and updates
                            // that to the API, but there is no use of that data.
                            //
                            // I will make this assumption since we want to go live:
                            // - We only want 1 delivery notification sent, ever
                            // 
                            // Any mistakes in my assumptions will need to be addressed
                            // next sprint or in a hotfix if critical
                            // 
                            if (item.IsDelivered
                                && item.DeliveryNotification < 1)
                            {
                                try
                                {
                                    // this try/catch feels unnecessary, but I wanted
                                    // to preserve the original code's behavior of logging 
                                    // a success or a failure to send the alert.
                                    // better approaches include specific exception types
                                    // caught by the main try/catch block.
                                    // this code results in a double log of the exception if it occurs.

                                    await OrderService.SendOrderItemDeliveryAlertAsync(order.OrderId, item);
                                    item.DeliveryNotification = 1;
                                    requiresUpdate = true;

                                    this.GetLog().Log($"Delivery alert sent for Order {order.OrderId}, Item: {item.Description}");
                                }
                                catch (Exception ex)
                                {
                                    this.GetLog().Log($"Send delivery alert failed for Order {order.OrderId}, Item: {item.Description}", ex);

                                    // NOTE: use throw not rethrow
                                    throw;
                                }
                            }
                        }

                        if (requiresUpdate)
                        {
                            // TODO: an exception here requires thought on how to handle
                            // as we have already sent the notifications
                            // and will end up sending duplicate ones next time this runs
                            // if we don't track that we have sent them somehow, despite the API being down.
                            await OrderService.UpdateMedicalEquipmentOrderAsync(order);
                        }
                    }
                    catch (Exception ex)
                    {
                        // NOTE: in a real application, I would likely include the order JSON
                        // in the message for debugging purposes using a JSON serializer
                        // ex: var orderJson = JsonConvert.SerializeObject(order);
                        // But I'll go simple here.

                        this.GetLog().Log($"Unexpected exception sending delivery updates for {order.OrderId}", ex);
                        // NOTE: Log, but move on to the next order so that the whole background job is not stopped
                    }
                }
            }
        }
    }
}
