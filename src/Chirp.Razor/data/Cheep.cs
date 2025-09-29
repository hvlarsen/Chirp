namespace Chirp.Razor.Data;

public class Cheep
{
    public int MessageId { get; set; }
    public int AuthorId { get; set; }
    public string Text { get; set; }
    public int PubDate { get; set; }

    public Cheep(int messageId, int authorId, string text, int pubDate)
    {
        MessageId = messageId;
        AuthorId = authorId;
        Text = text;
        PubDate = pubDate;
   }
}