namespace Communication.Responses;
public class ResponseAttendeeJson
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime Created_At { get; set; }
    public DateTime? CheckedIn_At { get; set; }
}
