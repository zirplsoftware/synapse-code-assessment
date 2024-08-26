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
                                    // NOTE: a better approach to this involves specific exception types
                                    // such as SendOrderItemDeliveryAlertException thrown by the
                                    // OrderService.SendOrderItemDeliveryAlertAsync method and
                                    // caught by the main try/catch block instead of adding this small try/catch block.
                                    // Also, this less ideal approach results in a double log of the exception if it occurs.

                                    item.DeliveryNotification = 1; // update it first so the message will be accurate
                                    await OrderService.SendOrderItemDeliveryAlertAsync(order.OrderId, item);
                                    requiresUpdate = true;
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
                            try
                            {
                                // NOTE: a better approach to this involves specific exception types
                                // such as UpdateMedicalEquipmentOrderException thrown by the
                                // OrderService.UpdateMedicalEquipmentOrderAsync method and
                                // caught by the main try/catch block instead of adding this small try/catch block.
                                // Also, this less ideal approach results in a double log of the exception if it occurs.

                                await OrderService.UpdateMedicalEquipmentOrderAsync(order);
                            }
                            catch (Exception ex)
                            {
                                // TODO: an exception here requires thought on how to handle
                                // as we have already sent the notifications
                                // and will end up sending duplicate ones next time this runs
                                // if we don't track that we have sent them somehow, despite the API being down.
                                // maybe the product owner is okay with a rare duplicate occurring.

                                this.GetLog().Log($"Unexpected exception updating order {order.OrderId}");

                                // NOTE: use throw not rethrow
                                throw;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // NOTE: in a real application, I would likely include the order JSON
                        // in the message for debugging purposes. I'd write a simple JSON serialization extension method
                        // and use it this way: var orderJson = order.ToJson();
                        // But I'll go simple here.

                        this.GetLog().Log($"Unexpected exception processing delivered order items for {order.OrderId}", ex);
                        // NOTE: Log, but move on to the next order so that the whole background job is not stopped
                    }
                }
            }
        }
    }
}
