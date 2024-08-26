using System.Threading.Tasks;
using Synapse.Domain.Models.Orders;
using Synapse.Net.Http;

namespace Synapse.Domain.Services.Orders
{
    public interface IOrderService
    {
        Task<MedicalEquipmentOrder[]> GetMedicalEquipmentOrdersAsync();
    }

    public class OrderService : IOrderService
    {
        // NOTE: this would be retrieved from configuration
        // of some sort in a real application rather than hardcoded
        private const string OrdersUrl = "https://orders-api.com/orders";

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
    }
}
