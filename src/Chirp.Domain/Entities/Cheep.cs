namespace Chirp.Domain.Entities;

public class Cheep
{
    public int MessageId { get; set; }
    public int AuthorId { get; set; }
    public string Text { get; set; }
    public int PubDate { get; set; }
    
    public string Author { get; set; }

    public string FormattedDate => DateTimeOffset.FromUnixTimeSeconds(PubDate).ToString("MM/dd/yy H:mm:ss");
    public Cheep(int messageId, int authorId, string text, int pubDate, string username)
    {
        MessageId = messageId;
        AuthorId = authorId;
        Text = text;
        PubDate = pubDate;
        Author = username;
    }
}