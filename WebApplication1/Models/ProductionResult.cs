namespace WebApplication1.Models
{
    public class ProductionResult
    {
        public int Id { get; set; }
        public int ProductionPlanId { get; set; }
        public int Monday { get; set; }
        public int Tuesday { get; set; }
        public int Wednesday { get; set; }
        public int Thursday { get; set; }
        public int Friday { get; set; }
        public int Saturday { get; set; }
        public int Sunday { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
