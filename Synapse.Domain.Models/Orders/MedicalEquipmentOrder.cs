using System.Collections.Generic;

namespace Synapse.Domain.Models.Orders
{
    public class MedicalEquipmentOrder
    {
        public int Id { get; set; }
        public IList<MedicalEquipmentOrderLineItem> Items { get; set; } = new List<MedicalEquipmentOrderLineItem>();

        // surely many more properties
    }
}
