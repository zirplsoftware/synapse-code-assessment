using System.Threading.Tasks;
using Synapse.Domain.Models.Orders;

namespace Synapse.Domain.Services.Orders
{
    public interface IOrderService
    {
        Task<MedicalEquipmentOrder[]> GetMedicalEquipmentOrdersAsync();
        Task SendOrderItemDeliveryAlertAsync(int orderId, MedicalEquipmentOrderLineItem item);
        Task UpdateMedicalEquipmentOrderAsync(MedicalEquipmentOrder order);
    }
}
