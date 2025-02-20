public class Document
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Content { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
}

public class DocumentVector
{
    public Document Document { get; set; }
    public float[] Embedding { get; set; }
} 