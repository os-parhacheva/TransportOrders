using System.Net;
using System.Text;
using System.Text.Json;
using TransportOrders.Presentation.Models.DTO;

namespace TransportOrders.Presentation.Services
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<OrderDTO>> GetOrdersAsync()
        {
            var response = await _httpClient.GetAsync("Order");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                 $"Ошибка HTTP-запроса с кодом состояния: {response.StatusCode}");
            }
            try
            {
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Не чувствителен к регистру букв
                };
                var orders = JsonSerializer.Deserialize<List<OrderDTO>>(json, options);

                // Пустой список вместо null
                return orders ?? new List<OrderDTO>();
            }
            catch (JsonException ex)
            {                
                throw new InvalidOperationException("Не удалось десериализовать ответ на заказы", ex);
            }
        }

        public async Task<OrderDTO> GetOrderByIdAsync(Guid id)
        {
            // Проверка валидности ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Order ID cannot be empty", nameof(id));
            }

            var response = await _httpClient.GetAsync($"Order/{ id }");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                 $"Ошибка HTTP-запроса с кодом состояния: {response.StatusCode}");
            }

            try
            {
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true 
                };
                var orders = JsonSerializer.Deserialize<OrderDTO>(json, options);
                                
                return orders ?? new OrderDTO();
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Не удалось десериализовать ответ на заказ", ex);
            }           
        }

        public async Task<(bool Success, Dictionary<string, string[]>)> CreateOrderAsync(OrderDTO order)
        {            
            var json = JsonSerializer.Serialize(order);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Order", content);
            

            if (response.IsSuccessStatusCode)
                return (true, null);
            
            var responseContent = await response.Content.ReadAsStringAsync();

            var problem = JsonSerializer.Deserialize<ResponseDetailsDto>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return (false, problem?.Errors);            

        }
    }
}
