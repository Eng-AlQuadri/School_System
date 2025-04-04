namespace API.DTOs;

public class MessageDto
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public string? SenderUsername { get; set; }
    public string? SenderPhotoUrl { get; set; }
    public int RecipientId { get; set; }
    public string RecipientUsername { get; set; } = null!;
    public string? RecipientPhotoUrl { get; set; }
    public string Content { get; set; } = null!;
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; }
}
