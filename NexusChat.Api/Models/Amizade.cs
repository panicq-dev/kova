
using NexusChat.Enums;
namespace NexusChat.Models;

public class Amizade
{
    public string Id {  get; set; } = string.Empty;
    public string IdEnviador { get; set; } = string.Empty;
    public string IdRecebedor { get; set; } = string.Empty;
    public AmizadeStatus Status { get; set; }
    public DateTime DataCriacao { get; set; } =  DateTime.Now;
    
}