using Synapse.Domain.Models.Orders;
using Synapse.Domain.Services.Orders;

namespace Synapse.Domain.Services.UnitTests.Orders
{
    // NOTE: I only included one method, but many test cases are needed, such as:
    // - no orders returned
    // - 1 order with 1 item
    // - multiple orders (some with items requiring alerts, some not)
    // - multiple items in an order (some with items requiring alerts, some not)
    // - exception retrieving orders
    // - exception sending delivery alert
    // - exception updating order after an alert
    // - an exception sending a delivery alert for 1 item should not prevent the next item from sending an alert
    // - an exception updating an order should not prevent the next order from being processed
    // - no exceptions make it through to the Program ever
    // - duplicate alerts are not sent
    // and plenty more edge cases like
    // - "Delivered" but with wrong casing
    [TestClass]
    public class OrderProcessingServiceTests
    {
        private OrderProcessingService _orderProcessingService;
        private OrderServiceMock _orderServiceMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _orderServiceMock = new OrderServiceMock();
            _orderProcessingService = new OrderProcessingService
            {
                // NOTE: using a framework like Moq is a better approach
                // but I've only used Moq and understand there was a 
                // recent security issue with the developer, so I am rolling my own
                OrderService = _orderServiceMock
            };
        }

        [TestMethod]
        public async Task TryProcessDeliveredOrderItemsAsync_GetMedicalEquipmentOrdersAsyncIsCalled()
        {
            // This test case only tests that the GetMedicalEquipmentOrdersAsync is called

            // 1) Set up the _orderServiceMock...

            // 2) call the method being tested
            await _orderProcessingService.TryProcessDeliveredOrderItemsAsync();

            // 3) evaluate the results on the mock
            Assert.AreEqual(_orderServiceMock.GetMedicalEquipmentOrdersAsyncWasCalled, true);
        }

        // NOTE: this is not a good enough mock implementation to test the service
        // a mock framework is necessary for thorough testing
        private class OrderServiceMock : IOrderService
        {
            internal bool GetMedicalEquipmentOrdersAsyncWasCalled;
            internal bool SendOrderItemDeliveryAlertAsyncWasCalled;
            internal bool UpdateMedicalEquipmentOrderAsyncWasCalled;

            public async Task<MedicalEquipmentOrder[]> GetMedicalEquipmentOrdersAsync()
            {
                GetMedicalEquipmentOrdersAsyncWasCalled = true;

                return new MedicalEquipmentOrder[]
                {
                    new MedicalEquipmentOrder()
                    {
                        Items = new List<MedicalEquipmentOrderLineItem>()
                        {
                            new MedicalEquipmentOrderLineItem()
                            {
                                DeliveryNotification = 0,
                                Description = "Item 1",
                                Status = "Delivered"
                            }
                        }
                    }
                };
            }

            public async Task SendOrderItemDeliveryAlertAsync(int orderId, MedicalEquipmentOrderLineItem item)
            {
                SendOrderItemDeliveryAlertAsyncWasCalled = true;
            }

            public async Task UpdateMedicalEquipmentOrderAsync(MedicalEquipmentOrder order)
            {
                UpdateMedicalEquipmentOrderAsyncWasCalled = true;
            }
        }
    }
}
