namespace Synapse.Domain.Models.Orders
{
    public enum MedicalEquipmentOrderStatusEnum
    {
        // TODO: check with product owner if these statuses are correct
        // we do know that 4 = Delivered, which is all we need
        // to launch this feature.
        Pending = 1, // TODO: needed?
        Ordered = 2, // TODO: needed?
        Paid = 3, // TODO: needed?

        // This one is valid per the requirements
        Delivered = 4
    }
}