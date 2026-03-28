using TransportOrders.API.DTO_Model;
using TransportOrders.Domain.Model;

namespace TransportOrders.API.DTO
{
    public static class ConversionDTO
    {

        public static List<OrderDTO> ConvertOrdersToOrderDTOs(List<Order> orders)
        {
            List<OrderDTO> result = new List<OrderDTO>(); 
            foreach (var order in orders)
            {
                result.Add(GeOrderDTOFromOrder(order));
            }
            return result;
        }

        public static OrderDTO GeOrderDTOFromOrder(Order order) 
        {
            return new OrderDTO() { 
                Id = order.Id,
                CitySender       = order.CitySender,
                CityRecipient    = order.CityRecipient,
                AddressSender    = order.AddressSender,
                AddressRecipient = order.AddressRecipient,
                CargoWeight      = order.CargoWeight,
                PickupDate       = order.PickupDate,

            };
        }

    }
}
