using System.ComponentModel.DataAnnotations;

namespace MarketingAlliance.Core.Entities.Request
{
    public class FeedbackMessageBody
    {
        /// <summary>
        /// Имя
        /// </summary>
        [RegularExpression(@"^[а-яА-Я]+$", ErrorMessage = "Имя должно содержать только русские буквы")]
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Фамилия
        /// </summary>

        [RegularExpression(@"^[а-яА-Я]+$", ErrorMessage = "Фамилия должна содержать только русские буквы")]
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Отчество
        /// </summary>


        [RegularExpression(@"^[а-яА-Я]+$", ErrorMessage = "Отчество должно содержать только русские буквы")]
        public string? Patronymic { get; set; }

        /// <summary>
        /// Электронная почта
        /// </summary>
        [EmailAddress(ErrorMessage = "Неверный формат электронной почты")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Вопрос
        /// </summary>

        [Required(ErrorMessage = "Вопрос обязателен для заполнения")]
        public string Question { get; set; } = null!;
    }
}