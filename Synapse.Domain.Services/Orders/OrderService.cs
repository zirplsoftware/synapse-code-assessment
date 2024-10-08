﻿using System.Threading.Tasks;
using Synapse.Domain.Models.Orders;
using Synapse.Logging;
using Synapse.Net.Http;

namespace Synapse.Domain.Services.Orders
{
    public class OrderService : IOrderService
    {
        // NOTE: this would be retrieved from configuration
        // of some sort in a real application rather than hardcoded
        private const string OrdersUrl = "https://orders-api.com/orders";
        private const string AlertsUrl = "https://alert-api.com/alerts";
        private const string UpdatesUrl = "https://update-api.com/update";

        // NOTE: this class should probably use an instance RestClient property
        // allowing the RestClient to use an instance HttpClient property
        // with this class implementing IDisposable to dispose of the RestClient (and thus the HttpClient through RestClient.Dispose()).
        // Either the calling code would need to wrap this in a using block or we could rely on the IoC container to manage the lifetime of this class.

        public async Task<MedicalEquipmentOrder[]> GetMedicalEquipmentOrdersAsync()
        {
            using (var restClient = new RestClient())
            {
                // NOTE: the original code provided
                // logging of exceptions but did not propagate them.
                // instead it just returned an empty array.
                // I didn't feel this was the best approach here.
                // Sometimes it is desired, and in that case,
                // a better method would be:
                // async Task<bool> TryGetMedicalEquipmentOrdersOrdersAsync(out MedicalEquipmentOrder[] orders)
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
                        $"Alert for delivered item: Order {orderId}, Item: {item.Description}, Delivery Notifications: {item.DeliveryNotification}"
                    // NOTE: default this as the first notification in the message
                };

                // NOTE: same note about propagating exceptions as above
                // TECH NOTE: No response required, so just parse to object
                await restClient.PostAsJsonAndParseResponseAsync<object>(AlertsUrl, content);

                // NOTE: log inside the using block in case Dispose throws an exception
                this.GetLog().Log($"Delivery alert sent for Order {orderId}, Item: {item.Description}");
            }
        }

        public async Task UpdateMedicalEquipmentOrderAsync(MedicalEquipmentOrder order)
        {
            using (var restClient = new RestClient())
            {
                // NOTE: same note about propagating exceptions as above
                // TECH NOTE: No response required, so just parse to object
                await restClient.PostAsJsonAndParseResponseAsync<object>(UpdatesUrl, order);

                // NOTE: log inside the using block in case Dispose throws an exception
                this.GetLog().Log($"Updated Order {order.OrderId}");
            }
        }
    }
}