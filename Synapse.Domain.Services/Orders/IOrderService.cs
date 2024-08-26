using System.Threading.Tasks;
using Synapse.Domain.Models.Orders;
using Synapse.Net.Http;

namespace Synapse.Domain.Services.Orders
{
    public interface IOrderService
    {
        Task<MedicalEquipmentOrder[]> GetMedicalEquipmentOrdersAsync();
        Task SendOrderItemDeliveryAlertAsync(int orderId, MedicalEquipmentOrderLineItem item);
        Task UpdateMedicalEquipmentOrderAsync(MedicalEquipmentOrder order);
    }

    public class OrderService : IOrderService
    {
        // NOTE: this would be retrieved from configuration
        // of some sort in a real application rather than hardcoded
        private const string OrdersUrl = "https://orders-api.com/orders";
        private const string AlertsUrl = "https://alert-api.com/alerts";
        private const string UpdatesUrl = "https://update-api.com/update";

        public async Task<MedicalEquipmentOrder[]> GetMedicalEquipmentOrdersAsync()
        {
            using (var restClient = new RestClient())
            {
                // NOTE: the original code provided
                // logged exceptions but did not propagate them.
                // this is a bad idea.
                // though sometimes it is desired, and in that case,
                // a better method would be is async Task<bool> TryGetMedicalEquipmentOrdersOrdersAsync(out MedicalEquipmentOrder[] orders)
                // but for my purposes, I am choosing to propagate the exception
                // and let the caller handle it.
                var orders = await restClient.GetAndParseResponseAsJsonAsync<MedicalEquipmentOrder[]>(OrdersUrl);
                return orders;
            }
        }

        public async Task SendOrderItemDeliveryAlertAsync(int orderId, MedicalEquipmentOrderLineItem item)
        {
            using (var restClient = new RestClient())
            {
                var content = new
                {
                    Message =
                        $"Alert for delivered item: Order {orderId}, Item: {item.Description}, Delivery Notifications: {item.DeliveryNotification ?? 1}"
                        // NOTE: default this as the first notification in the message
                };

                // NOTE: same note about propagating exceptions as above
                // TECH NOTE: No response required, so just parse to object
                await restClient.PostAsJsonAndParseResponseAsync<object>(AlertsUrl, content);
            }
        }

        public async Task UpdateMedicalEquipmentOrderAsync(MedicalEquipmentOrder order)
        {
            using (var restClient = new RestClient())
            {
                // NOTE: same note about propagating exceptions as above
                // TECH NOTE: No response required, so just parse to object
                await restClient.PostAsJsonAndParseResponseAsync<object>(UpdatesUrl, order);
            }
        }
    }
}
