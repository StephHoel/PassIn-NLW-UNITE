namespace Communication.Responses;
public class ResponseEventJson
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public int Maximum_Attendees { get; set; }
    public int Attendees_Amount { get; set; }
}
