namespace Synapse.Domain.Models.Orders
{
    public class MedicalEquipmentOrderLineItem
    {
        public int Id { get; set; }
        public MedicalEquipmentOrderStatusEnum Status { get; set; }

        // NOTE: although not set up in this solution
        // I treated this like I would an entity framework model relationship
        // with a reference back to the principal entity
        public int OrderId { get; set; }
        public virtual MedicalEquipmentOrder Order { get; set; }
        
        // surely many more properties
    }
}