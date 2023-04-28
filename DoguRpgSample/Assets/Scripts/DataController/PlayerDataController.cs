using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;

public class PlayerDataController : Singleton<PlayerDataController>
{
    private const string PLAYER_DATA = "PlayerData";
    private const string CURRENT_PLAYER_CHARACTER_UUID = "CurrentPlayerCharacterUuid";
    private const string PLAYER_CHARACTERS_DATA = "PlayerCharactersData";
    public PlayerData data { get; private set; }
    private string currentPlayerCharacterUuid;

    public List<PlayerCharacterData> Characters
    {
        get => characters;
        private set => characters = value;
    }

    private List<PlayerCharacterData> characters;

    protected override void InitSingleton()
    {
        base.InitSingleton();

        Reload();
    }

    public void Reload()
    {
        data = JsonPrefs.Load<PlayerData>(PLAYER_DATA);
        if (null == data)
        {
            data = new PlayerData();
        }

        currentPlayerCharacterUuid = JsonPrefs.Load<string>(CURRENT_PLAYER_CHARACTER_UUID);
        if (null == currentPlayerCharacterUuid)
        {
            currentPlayerCharacterUuid = string.Empty;
        }

        characters = JsonPrefs.Load<List<PlayerCharacterData>>(PLAYER_CHARACTERS_DATA);
        if (null == characters)
        {
            characters = new List<PlayerCharacterData>();
        }
    }

    public void SetPlayerData(PlayerData newPlayerData)
    {
        data = newPlayerData;
        JsonPrefs.Save(PLAYER_DATA, data);
    }

    public void SetCurrentPlayerCharacter(string uuid)
    {
        currentPlayerCharacterUuid = uuid;
        JsonPrefs.Save(CURRENT_PLAYER_CHARACTER_UUID, uuid);
    }

    public PlayerCharacterData GetCurrentPlayerCharacter()
    {
        var playerCharacter = characters.FirstOrDefault(x => x.uuid == currentPlayerCharacterUuid);
        return playerCharacter;
    }

    public void AddPlayerCharacter(PlayerCharacterData playerCharacterData)
    {
        characters.Add(playerCharacterData);
        JsonPrefs.Save(PLAYER_CHARACTERS_DATA, characters);
    }

    public bool RemovePlayerCharacter(string uuid)
    {
        var c = characters.Where(c => c.uuid == uuid);
        var playerCharacterDatas = c as PlayerCharacterData[] ?? c.ToArray();
        if (!playerCharacterDatas.Any())
        {
            return false;
        }

        characters.Remove(playerCharacterDatas[0]);
        JsonPrefs.Save(PLAYER_CHARACTERS_DATA, characters);
        return true;
    }

    public void SavePlayerCharacter()
    {
        JsonPrefs.Save(PLAYER_CHARACTERS_DATA, characters);
    }
}