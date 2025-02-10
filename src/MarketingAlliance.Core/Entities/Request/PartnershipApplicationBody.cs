using System.ComponentModel.DataAnnotations;

namespace MarketingAlliance.Core.Entities.Request
{
    public class PartnershipApplicationBody
    {
        [RegularExpression(@"^[а-яА-Я]+$", ErrorMessage = "Имя должно содержать только русские буквы")]
        public string FirstName { get; set; } = null!;

        [RegularExpression(@"^[а-яА-Я]+$", ErrorMessage = "Фамилия должна содержать только русские буквы")]
        public string LastName { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Неверный формат электронной почты")]
        public string Email { get; set; } = null!;

        [RegularExpression(@"^[а-яА-Я]+$", ErrorMessage = "Отчество должно содержать только русские буквы")]
        public string? Patronymic { get; set; }

        public string Pharmacy { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Количество розничных точек должно быть больше 0")]
        public int NumberOfRetailPoints { get; set; }

        [RegularExpression(@"^\d{12}$", ErrorMessage = "ИНН должен содержать 12 цифр")]
        public string INN { get; set; } = null!;

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Неверный формат номера телефона (10 цифр)")]
        public string ContactPhoneNumber { get; set; } = null!;

        public string? Comment { get; set; }
    }
}