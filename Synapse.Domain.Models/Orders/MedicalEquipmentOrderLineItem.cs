using System;

namespace Synapse.Domain.Models.Orders
{
    public class MedicalEquipmentOrderLineItem
    {
        private const string DeliveredStatus = "Delivered";

        public int ItemId { get; set; }
        public string Description { get; set; }

        // NOTE this should not be an int and is named poorly, 
        // but this is the name and data type in original code
        // and I will make the assumption
        // that the code was correct in its
        // understanding of the API
        public int? DeliveryNotification { get; set; }

        // NOTE: this would be better as an enum, but
        // I will make the assumption that the called API 
        // is already defined
        public string Status { get; set; }

        // NOTE: although not set up in this solution
        // I treated this like I would an entity framework model relationship
        // with a reference back to the principal entity
        public int OrderId { get; set; }
        public virtual MedicalEquipmentOrder Order { get; set; }




        // NOTE: I went with your OrdinalIgnoreCase, but I reviewed the difference recently
        // between Ordinal and OrdinalIgnoreCase and I think InvariantIgnoreCase is typically more appropriate
        // but I am keeping the original code
        public bool IsDelivered => DeliveredStatus.Equals(Status, StringComparison.OrdinalIgnoreCase);
    }
}