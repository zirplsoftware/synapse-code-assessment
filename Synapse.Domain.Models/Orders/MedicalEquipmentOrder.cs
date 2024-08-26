using System.Collections.Generic;

namespace Synapse.Domain.Models.Orders
{
    public class MedicalEquipmentOrder
    {
        public int OrderId { get; set; }
        public IList<MedicalEquipmentOrderLineItem> Items { get; set; } = new List<MedicalEquipmentOrderLineItem>();
    }
}
