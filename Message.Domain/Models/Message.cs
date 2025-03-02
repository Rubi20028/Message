using System.ComponentModel.DataAnnotations;

namespace Message.Domain.Models;

public class Message
{
    [Key]
    public int MessageId { get; set; }
    
    [Required]
    public string Text { get; set; }
    
    [Required]
    public DateTime TimeStamp { get; set; }
    
    [Required]
    public int SequenceNumber { get; set; }
}