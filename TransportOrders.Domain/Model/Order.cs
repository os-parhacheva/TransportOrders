using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TransportOrders.Domain.Model
{
    public class Order : IValidatableObject
    {
        //Идентификатор
        public Guid Id { get; set; }
        //Город отправителя
        [Required]
        [MaxLength(100)]
        public string CitySender { get; set; }
        //Адрес отправителя
        [Required]
        [MaxLength(200)]
        public string AddressSender { get; set; }
        //Город получателя
        [Required]
        [MaxLength(100)]
        public string CityRecipient { get; set; }
        
        //Адрес получателя
        [Required]
        [MaxLength(200)]
        public string AddressRecipient { get; set; }
        
        //Вес груза
        [Required]
        [Precision(18, 2)]
        public decimal CargoWeight { get; set; }
        //Дата отправки
        [Required]
        public DateTime PickupDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //
            if(CargoWeight <= 0)
            {
                yield return new ValidationResult(
                    "Вес должен быть больше 0",
                    new[] { nameof(CargoWeight) });
            }
            // Проверка на 2 знака после запятой
            if (decimal.Round(CargoWeight, 2) != CargoWeight)
            {
                yield return new ValidationResult(
                    "Вес должен содержать не более 2 знаков после запятой",
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
