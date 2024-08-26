using System.Collections.Generic;

namespace Synapse.Domain.Models.Orders
{
    // NOTE: I only implemented the properties that were used in the original code
    public class MedicalEquipmentOrder
    {
        // NOTE: due the original code implementation using JObject, 
        // I don't know if this is actually an int or a string.
        // going with assumption that it is an int.
        // Unit tests will confirm.
        public int OrderId { get; set; }
        public IList<MedicalEquipmentOrderLineItem> Items { get; set; } = new List<MedicalEquipmentOrderLineItem>();
    }
}
