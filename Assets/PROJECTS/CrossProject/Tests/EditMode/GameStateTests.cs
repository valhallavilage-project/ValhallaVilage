using CrossProject.Core.Characters;
using CrossProject.Core.SaveLoad;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using JsonSerializer = CrossProject.Core.SaveLoad.JsonSerializer;

public class GameStateTests
{
    [Test]
    public void SerializeAndDeserializePart()
    {
        var serializer = new JsonSerializer();
        var part = new ObtainedCharactersPart();
        part.ObtainedCharacters.Add(new CharacterId("Test1"));
        part.ObtainedCharacters.Add(new CharacterId("Test2"));
        part.ObtainedCharacters.Add(new CharacterId("Test3"));
        part.CurrentCharacterId = part.ObtainedCharacters[1];
        var json = serializer.Serialize(part);
        part = serializer.Deserialize<ObtainedCharactersPart>(json);
        Assert.NotZero(part.ObtainedCharacters.Count);
    }

    [Test]
    public void SaveAndLoadTest()
    {
        var gameState = new GameState();
        var serializer = new JsonSerializer();
        gameState.Set(new ObtainedCharactersPart());
        gameState.TryGet<ObtainedCharactersPart>(out var part);
        part.ObtainedCharacters.Add(new CharacterId("Test"));
        gameState.Set(part);
        gameState.TryGet(out part);
        var json = serializer.Serialize(gameState, Formatting.Indented);
        gameState = serializer.Deserialize<GameState>(json);
        gameState.TryGet(out part);
        Assert.NotZero(part.ObtainedCharacters.Count);
    }
}
