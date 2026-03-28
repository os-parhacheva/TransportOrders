using System.ComponentModel.DataAnnotations;

namespace TransportOrders.Presentation.Models.DTO
{
    public class OrderDTO : IValidatableObject
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Заполните обязательное поле")]
        [MaxLength(100)]
        public string CitySender { get; set; }

        [Required(ErrorMessage = "Заполните обязательное поле")]
        [MaxLength(200)]
        public string AddressSender { get; set; }

        [Required(ErrorMessage = "Заполните обязательное поле")]
        [MaxLength(100)]
        public string CityRecipient { get; set; }

        [Required(ErrorMessage = "Заполните обязательное поле")]
        [MaxLength(200)]
        public string AddressRecipient { get; set; }

        
        public decimal CargoWeight { get; set; }
        public DateTime PickupDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            
            if (CargoWeight <= 0)
            {
                yield return new ValidationResult(
                    "Вес груза должен быть больше 0 кг",
                    new[] { nameof(CargoWeight) });
            }

            // Проверка даты (не раньше сегодняшнего дня)
            if (PickupDate.Date < DateTime.Now.Date)
            {
                yield return new ValidationResult(
                    "Дата забора не может быть раньше текущей даты",
                    new[] { nameof(PickupDate) });
            }
        }
    }
}
