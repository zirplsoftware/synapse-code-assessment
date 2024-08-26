using System;
using System.Threading.Tasks;
using Synapse.Domain.Models.Orders;

namespace Synapse.Domain.Services.Orders
{
    public interface IOrderProcessingService
    {
        Task ProcessDeliveredOrderItemsAsync();
    }
    public class OrderProcessingService : IOrderProcessingService
    {
        public IOrderService OrderService { get; set; }

        public async Task ProcessDeliveredOrderItemsAsync()
        {
            MedicalEquipmentOrder[] orders = null;

            try
            {
                orders = await OrderService.GetMedicalEquipmentOrdersAsync();
            }
            catch (Exception ex)
            {
                // NATHAN: log
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
                            // there is a bug in the original code - 
                            // code that I would be unsure how to handle without
                            // speaking with the product owner.
                            // It appears to send the delivery alert every time
                            // the job runs once it is in a delivered status.
                            // it keeps track of how many were sent, and updates
                            // that to the API, but there is no use of that data.
                            //
                            // I will make this assumption since we want to go live
                            // - We only want 1 delivery notification sent
                            // 
                            // Any bugs in my logic will need to be addressed
                            // next sprint or in a hotfix if critical
                            // 
                            if (item.IsDelivered
                                && (item.DeliveryNotification == null
                                    || item.DeliveryNotification.Value < 1))
                            {
                                await OrderService.SendOrderItemDeliveryAlertAsync(order.OrderId, item);
                                item.DeliveryNotification = 1;
                                requiresUpdate = true;
                            }
                        }

                        if (requiresUpdate)
                        {
                            // TODO: an exception here requires thought on how to handle
                            // as we have already sent the notifications
                            // and will end up sending duplicate ones next time this runs
                            await OrderService.UpdateMedicalEquipmentOrderAsync(order);
                        }
                    }
                    catch (Exception ex)
                    {
                        // NOTE: Log, but move on to the next order so that the whole background job is not stopped

                        // NATHAN: implement logging
                    }
                }
            }
        }
    }
}
