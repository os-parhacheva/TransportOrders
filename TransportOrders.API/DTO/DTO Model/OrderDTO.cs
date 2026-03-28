
namespace TransportOrders.API.DTO_Model
{
    public class OrderDTO
    {
        public Guid Id { get; set; }    
        public string CitySender { get; set; }    
        public string AddressSender { get; set; }  
        public string CityRecipient { get; set; }
      
        public string AddressRecipient { get; set; }

        public decimal CargoWeight { get; set; }

        public DateTime PickupDate { get; set; }
    }
}
