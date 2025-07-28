namespace InvenePHI.Server.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string Description { get; set; } // Additional details about the order
    }
}
