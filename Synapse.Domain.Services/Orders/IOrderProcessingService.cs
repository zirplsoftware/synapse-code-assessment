using System.Threading.Tasks;

namespace Synapse.Domain.Services.Orders
{
    public interface IOrderProcessingService
    {
        Task ProcessDeliveredOrderItemsAsync();
    }
}