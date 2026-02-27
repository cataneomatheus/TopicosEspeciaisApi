namespace TopicosEspeciaisApi.Models;

public class Player
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Position { get; set; } = string.Empty;
    public int Number { get; set; }
    public string PhotoUrl { get; set; } = string.Empty;
    public int SlotIndex { get; set; }
}
