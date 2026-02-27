using TopicosEspeciaisApi.Models;

namespace TopicosEspeciaisApi.Services;

public class PlayerService
{
    private readonly List<Player> _players = new();

    public PlayerService()
    {
        SeedData();
    }

    private void SeedData()
    {
        _players.AddRange(new List<Player>
        {
            new() { Id = Guid.NewGuid(), Name = "Marcos", Age = 28, Position = "GK", PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e0/Marcos_Roberto.jpg/220px-Marcos_Roberto.jpg", SlotIndex = 0 },
            new() { Id = Guid.NewGuid(), Name = "Cafu", Age = 32, Position = "DEF", PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9e/Cafu_2006.jpg/220px-Cafu_2006.jpg", SlotIndex = 1 },
            new() { Id = Guid.NewGuid(), Name = "Lúcio", Age = 24, Position = "DEF", PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/5a/L%C3%BAcio_in_Brazil_Kit_2006.jpg/220px-L%C3%BAcio_in_Brazil_Kit_2006.jpg", SlotIndex = 2 },
            new() { Id = Guid.NewGuid(), Name = "Roque Júnior", Age = 26, Position = "DEF", PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4e/Roque_J%C3%BAnior.jpg/220px-Roque_J%C3%BAnior.jpg", SlotIndex = 3 },
            new() { Id = Guid.NewGuid(), Name = "Roberto Carlos", Age = 29, Position = "DEF", PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c4/Roberto_Carlos_2006.jpg/220px-Roberto_Carlos_2006.jpg", SlotIndex = 4 },
            new() { Id = Guid.NewGuid(), Name = "Gilberto Silva", Age = 25, Position = "MID", PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a0/Gilberto_Silva_2014.jpg/220px-Gilberto_Silva_2014.jpg", SlotIndex = 5 },
            new() { Id = Guid.NewGuid(), Name = "Kléberson", Age = 23, Position = "MID", PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f6/Kl%C3%A9berson.jpg/220px-Kl%C3%A9berson.jpg", SlotIndex = 6 },
            new() { Id = Guid.NewGuid(), Name = "Ronaldinho", Age = 22, Position = "MID", PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3f/Ronaldinho_Ga%C3%BAcho.jpg/220px-Ronaldinho_Ga%C3%BAcho.jpg", SlotIndex = 7 },
            new() { Id = Guid.NewGuid(), Name = "Rivaldo", Age = 30, Position = "MID", PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e5/Rivaldo_2012.jpg/220px-Rivaldo_2012.jpg", SlotIndex = 8 },
            new() { Id = Guid.NewGuid(), Name = "Ronaldo", Age = 25, Position = "FWD", PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d6/Ronaldo_2002.jpg/220px-Ronaldo_2002.jpg", SlotIndex = 9 },
            new() { Id = Guid.NewGuid(), Name = "Ronaldinho Gaúcho", Age = 22, Position = "FWD", PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3f/Ronaldinho_2012.jpg/220px-Ronaldinho_2012.jpg", SlotIndex = 10 },
            new() { Id = Guid.NewGuid(), Name = "Felipão", Age = 53, Position = "COACH", PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b0/Luiz_Felipe_Scolari_2014.jpg/220px-Luiz_Felipe_Scolari_2014.jpg", SlotIndex = 11 },
        });
    }

    public List<Player> GetAll() => _players;

    public Player? GetById(Guid id) => _players.FirstOrDefault(p => p.Id == id);

    public Player Add(Player player)
    {
        player.Id = Guid.NewGuid();
        _players.Add(player);
        return player;
    }

    public Player? Update(Guid id, Player updated)
    {
        var player = _players.FirstOrDefault(p => p.Id == id);
        if (player is null) return null;

        player.Name = updated.Name;
        player.Age = updated.Age;
        player.Position = updated.Position;
        player.PhotoUrl = updated.PhotoUrl;
        player.SlotIndex = updated.SlotIndex;

        return player;
    }

    public bool Delete(Guid id)
    {
        var player = _players.FirstOrDefault(p => p.Id == id);
        if (player is null) return false;
        _players.Remove(player);
        return true;
    }
}
