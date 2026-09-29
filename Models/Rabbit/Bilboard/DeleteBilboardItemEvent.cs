namespace Models.Rabbit.Bilboard
{
    public class DeleteBilboardItemEvent
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
    }
}
