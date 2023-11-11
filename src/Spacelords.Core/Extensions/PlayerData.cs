using System;
using System.Collections.Generic;
using MacroTools.Save;
using static War3Api.Common;

namespace MacroTools.Extensions
{
  /// <summary>
  /// Provides extra information about players that is not already tracked by the Warcraft 3 engine.
  /// </summary>
  internal sealed class PlayerData
  {
    private static readonly Dictionary<int, PlayerData> ById = new();

    private readonly player _player;

    private float _partialGold;
    private float _partialLumber;
    
    public void UpdatePlayerSetting(string setting, int value)
    {
      switch (setting)
      {
        case "CamDistance":
          PlayerSettings.CamDistance = value;
          _player.ApplyCameraField(CAMERA_FIELD_TARGET_DISTANCE, PlayerSettings.CamDistance, 1);
          break;
      }
      SaveManager.Save(PlayerSettings);
    }

    public void UpdatePlayerSetting(string setting, bool value)
    {
      switch (setting)
      {
        case "PlayDialogue":
          PlayerSettings.PlayDialogue = value;
          break;
        case "ShowQuestText":
          PlayerSettings.ShowQuestText = value;
          break;
        case "ShowCaptions":
          PlayerSettings.ShowCaptions = value;
          break;
      }
      SaveManager.Save(PlayerSettings);
    }

    public PlayerSettings PlayerSettings => SaveManager.SavesByPlayer.ContainsKey(_player)? SaveManager.SavesByPlayer[_player]: CreateNewPlayerSettings();

    private PlayerSettings CreateNewPlayerSettings()
    {
      var newPlayerSettings = new PlayerSettings();
      newPlayerSettings.SetPlayer(_player);
      SaveManager.SavesByPlayer[_player] = newPlayerSettings;
      return newPlayerSettings;
    }

    private PlayerData(player player)
    {
      _player = player;
    }
    
    public void AddGold(float x)
    {
      var fullGold = (float) Math.Floor(x);
      var remainderGold = x - fullGold;

      SetPlayerState(_player, PLAYER_STATE_RESOURCE_GOLD,
        GetPlayerState(_player, PLAYER_STATE_RESOURCE_GOLD) + R2I(fullGold));
      _partialGold += remainderGold;

      while (true)
      {
        if (_partialGold < 1) break;

        _partialGold -= 1;
        SetPlayerState(_player, PLAYER_STATE_RESOURCE_GOLD, GetPlayerState(_player, PLAYER_STATE_RESOURCE_GOLD) + 1);
      }
    }

    public void AddLumber(float x)
    {
      var fullLumber = (float) Math.Floor(x);
      var remainderLumber = x - fullLumber;

      SetPlayerState(_player, PLAYER_STATE_RESOURCE_LUMBER,
        GetPlayerState(_player, PLAYER_STATE_RESOURCE_LUMBER) + R2I(fullLumber));
      _partialLumber += remainderLumber;

      while (true)
      {
        if (_partialLumber < 1) break;

        _partialLumber -= 1;
        SetPlayerState(_player, PLAYER_STATE_RESOURCE_LUMBER, GetPlayerState(_player, PLAYER_STATE_RESOURCE_LUMBER) + 1);
      }
    }

    public void SetGold(float gold)
    {
      var fullGold = (int)gold / 1;
      SetPlayerState(_player, PLAYER_STATE_RESOURCE_GOLD, fullGold);
      
      var remainderGold = gold % 1;
      _partialGold = remainderGold;
    }

    public void SetLumber(float lumber)
    {
      var fullLumber = (int)lumber / 1;
      SetPlayerState(_player, PLAYER_STATE_RESOURCE_LUMBER, fullLumber);
      
      var remainderLumber = lumber % 1;
      _partialLumber = remainderLumber;
    }

    public float GetGold() => GetPlayerState(_player, PLAYER_STATE_RESOURCE_GOLD) + _partialGold;

    public float GetLumber() => GetPlayerState(_player, PLAYER_STATE_RESOURCE_LUMBER) + _partialLumber;

    /// <summary>
    ///   Retrieves the <see cref="PlayerData" /> object which contains information about the given <see cref="player" />.
    /// </summary>
    public static PlayerData ByHandle(player whichPlayer)
    {
      if (ById.TryGetValue(GetPlayerId(whichPlayer), out var person)) return person;

      var newPerson = new PlayerData(whichPlayer);
      Register(newPerson);
      return newPerson;
    }

    /// <summary>
    ///   Register a <see cref="PlayerData" /> to the Person system.
    /// </summary>
    private static void Register(PlayerData playerData) => ById.Add(GetPlayerId(playerData._player), playerData);
  }
}