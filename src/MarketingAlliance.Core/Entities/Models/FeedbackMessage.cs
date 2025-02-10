namespace MarketingAlliance.Core.Entities.Models
{
    public class FeedbackMessage
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string Email { get; set; } = null!;
        public string Question { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string ToMessage()
        {
            return "Здравствуйте!\n\n" +
                   $"Меня зовут {FirstName} {Patronymic} {LastName}, и я хотел(а) бы задать вам вопрос.\n\n\n" +
                   "Моя контактная информация:\n" +
                   $"- Email: {Email}\n\n" +
                   "Вопрос:\n" +
                   $"{Question}\n";
        }
    }
}