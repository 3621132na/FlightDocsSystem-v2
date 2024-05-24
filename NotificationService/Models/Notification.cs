namespace NotificationService.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string UserEmail { get; set; }
        public string Message { get; set; }
        public DateTime SentDate { get; set; }
    }
}
