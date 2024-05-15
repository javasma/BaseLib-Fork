namespace BaseLib.Core.Mail;
public class FileAttachment
{
    public Stream? Stream { get; set; }
    public string? FileName { get; set; }
    public string? MediaType { get; set; }
}
