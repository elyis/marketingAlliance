namespace MarketingAlliance.Core.Entities.Models
{
    public class PartnershipApplication
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string Email { get; set; } = null!;
        public string Pharmacy { get; set; } = null!;
        public int NumberOfRetailPoints { get; set; }
        public string INN { get; set; } = null!;
        public string ContactPhoneNumber { get; set; } = null!;
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public string ToMessage()
        {
            return "Здравствуйте!\n\n" +
                   $"Меня зовут {FirstName} {Patronymic} {LastName}, и я хотел(а) бы обсудить возможность сотрудничества.\n\n" +
                   "Вот информация о нашей компании:\n\n" +
                   $"- Email: {Email}\n" +
                   $"- Контактный телефон: {ContactPhoneNumber}\n" +
                   $"- Аптека: {Pharmacy}\n" +
                   $"- Количество торговых точек: {NumberOfRetailPoints}\n" +
                   $"- ИНН: {INN}\n\n" +
                   $"{(string.IsNullOrWhiteSpace(Comment) ? "" : $"Дополнительный комментарий: {Comment}\n\n")}" +
                   "Буду рад(а) обсудить детали сотрудничества в удобное для вас время. Ожидаю вашего ответа.\n";
        }
    }
}