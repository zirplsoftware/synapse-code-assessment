using System;

namespace Synapse.Domain.Models.Orders
{
    // NOTE: I only implemented the properties that were used in the original code
    public class MedicalEquipmentOrderLineItem
    {
        private const string DeliveredStatus = "Delivered";

        public string Description { get; set; }

        // NOTE: this is named poorly, 
        // but this is the signature in original code
        // and I will make the assumption
        // that the code was correct in its
        // understanding of the API
        public int DeliveryNotification { get; set; }

        // NOTE: this would be better as an enum, but
        // I will make the assumption that the called API 
        // is already defined
        public string Status { get; set; }

        // NOTE: I went with your OrdinalIgnoreCase, but I reviewed the difference recently
        // between Ordinal and OrdinalIgnoreCase and I think InvariantIgnoreCase is typically
        // more appropriate. I am keeping the original code.
        public bool IsDelivered => DeliveredStatus.Equals(Status, StringComparison.OrdinalIgnoreCase);
    }
}