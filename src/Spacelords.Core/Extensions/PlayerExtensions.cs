﻿using System.Collections.Generic;
using WCSharp.Shared.Data;
using static War3Api.Common;

namespace MacroTools.Extensions
{
  /// <summary>
  /// A set of helpful extension methods for dealing with Warcraft 3's native player class.
  /// </summary>
  public static class PlayerExtensions
  {
    /// <summary>
    /// Set the number of extra <see cref="ControlPoint.ControlLevel"/>s the player gets each turn.
    /// </summary>
    /// <returns></returns>
    public static player SetControlLevelPerTurnBonus(this player whichPlayer, float value)
    {
      PlayerData.ByHandle(whichPlayer).ControlLevelPerTurnBonus = value;
      return whichPlayer;
    }

    /// <summary>
    /// Returns the number of extra <see cref="ControlPoint.ControlLevel"/>s the player gets each turn.
    /// </summary>
    public static float GetControlLevelPerTurnBonus(this player whichPlayer) =>
      PlayerData.ByHandle(whichPlayer).ControlLevelPerTurnBonus;

    /// <summary>
    /// Pings the minimap for the player.
    /// </summary>
    /// <param name="whichPlayer">Who to display the ping to.</param>
    /// <param name="position">Where to ping.</param>
    /// <param name="duration">How long the ping should last.</param>
    /// <returns>The same player that was passed in.</returns>
    public static player PingLocation(this player whichPlayer, Point position, float duration)
    {
      if (GetLocalPlayer() == whichPlayer)
        PingMinimap(position.X, position.Y, duration);
      return whichPlayer;
    }

    /// <summary>
    /// Applies a particular camera field to the player's view.
    /// </summary>
    public static void ApplyCameraField(this player whichPlayer, camerafield whichField, float value, float duration)
    {
      if (GetLocalPlayer() != whichPlayer)
        return;
      SetCameraField(whichField, value, duration);
    }

    /// <summary>
    /// Increases or decreases a specific player state for the player.
    /// </summary>
    public static void AdjustPlayerState(this player player, playerstate playerState, int value) =>
      SetPlayerState(player, playerState, GetPlayerState(player, playerState) + value);

    /// <summary>Adds an amount of gold to a player.</summary>
    public static void AddGold(this player player, float gold) => PlayerData.ByHandle(player).AddGold(gold);

    /// <summary>Adds an amount of lumber to a player.</summary>
    public static void AddLumber(this player player, float lumber) => PlayerData.ByHandle(player).AddLumber(lumber);

    /// <summary>Sets the player's gold to a specific value.</summary>
    public static void SetGold(this player player, float gold) => PlayerData.ByHandle(player).SetGold(gold);

    /// <summary>Sets the player's lumber to a specific value.</summary>
    public static void SetLumber(this player player, float lumber) => PlayerData.ByHandle(player).SetLumber(lumber);

    /// <summary>Returns the player's gold, including any partial gold.</summary>
    public static float GetGold(this player player) => PlayerData.ByHandle(player).GetGold();

    /// <summary>Returns the player's lumber, including any partial lumber.</summary>
    public static float GetLumber(this player player) => PlayerData.ByHandle(player).GetLumber();

    /// <summary>Returns the player's gold income, including any bonuses.</summary>
    public static float GetTotalIncome(this player player) => PlayerData.ByHandle(player).TotalIncome;

    /// <summary>Returns the player's bonus gold income.</summary>
    public static float GetBonusIncome(this player player) => PlayerData.ByHandle(player).BonusIncome;

    /// <summary>Returns the player's gold income, without any bonuses.</summary>
    public static float GetBaseIncome(this player player) => PlayerData.ByHandle(player).BaseIncome;

    /// <summary>Returns the player's lumber income.</summary>
    public static float GetLumberIncome(this player player) => PlayerData.ByHandle(player).LumberIncome;

    /// <summary>Modifies the player's bonus income.</summary>
    public static void AddBonusIncome(this player player, float value) =>
      PlayerData.ByHandle(player).BonusIncome += value;

    /// <summary>Modifies the player's lumber income.</summary>
    public static void AddLumberIncome(this player player, float value) =>
      PlayerData.ByHandle(player).LumberIncome += value;

    /// <summary>
    /// Returns the amount of food the player is using.
    /// </summary>
    public static int GetFoodUsed(this player whichPlayer) =>
      GetPlayerState(whichPlayer, PLAYER_STATE_RESOURCE_FOOD_USED);

    /// <summary>
    /// Returns the player's food cap.
    /// </summary>
    public static int GetFoodCap(this player whichPlayer) =>
      GetPlayerState(whichPlayer, PLAYER_STATE_RESOURCE_FOOD_CAP);

    /// <summary>
    /// Returns player's food cap ceiling.
    /// </summary>
    public static int GetFoodCapCeiling(this player whichPlayer) =>
      GetPlayerState(whichPlayer, PLAYER_STATE_FOOD_CAP_CEILING);

    /// <summary>Determines whether or not the player can see or use the specified ability.</summary>
    internal static void SetAbilityAvailability(this player player, int ability, bool value) =>
      SetPlayerAbilityAvailable(player, ability, value);

    internal static void SetColor(this player whichPlayer, playercolor color, bool changeExisting)
    {
      SetPlayerColor(whichPlayer, color);
      if (!changeExisting) return;
      foreach (var unit in CreateGroup().EnumUnitsOfPlayer(whichPlayer).EmptyToList())
        SetUnitColor(unit, color);
    }

    internal static PlayerData GetPlayerData(this player player) =>
      PlayerData.ByHandle(player);

    private static void SetPlayerAllianceStateAlly(this player sourcePlayer, player otherPlayer, bool flag)
    {
      SetPlayerAlliance(sourcePlayer, otherPlayer, ALLIANCE_PASSIVE, flag);
      SetPlayerAlliance(sourcePlayer, otherPlayer, ALLIANCE_HELP_REQUEST, flag);
      SetPlayerAlliance(sourcePlayer, otherPlayer, ALLIANCE_HELP_RESPONSE, flag);
      SetPlayerAlliance(sourcePlayer, otherPlayer, ALLIANCE_SHARED_XP, flag);
      SetPlayerAlliance(sourcePlayer, otherPlayer, ALLIANCE_SHARED_SPELLS, flag);
    }

    private static void SetPlayerAllianceStateVision(this player sourcePlayer, player otherPlayer, bool flag) =>
      SetPlayerAlliance(sourcePlayer, otherPlayer, ALLIANCE_SHARED_VISION, flag);

    private static void SetPlayerAllianceStateControl(this player sourcePlayer, player otherPlayer, bool flag) =>
      SetPlayerAlliance(sourcePlayer, otherPlayer, ALLIANCE_SHARED_CONTROL, flag);

    private static void SetPlayerAllianceStateFullControl(this player sourcePlayer, player otherPlayer, bool flag) =>
      SetPlayerAlliance(sourcePlayer, otherPlayer, ALLIANCE_SHARED_ADVANCED_CONTROL, flag);

    /// <summary>
    /// Displays a nicely formatted hint to the player.
    /// </summary>
    public static void DisplayHint(this player whichPlayer, string msg)
    {
      DisplayTextToPlayer(whichPlayer, 0, 0, $"\n|cff00ff00HINT|r - {msg}");
      if (GetLocalPlayer() == whichPlayer)
        StartSound(SoundLibrary.Hint);
    }

    /// <summary>
    /// Displays a nicely formatted hint to the player that one of their heroes has gained some stats.
    /// </summary>
    public static void DisplayHeroReward(this unit whichUnit, int strength, int agility, int intelligence,
      int experience)
    {
      var display = $"\n|cff00ff00HERO REWARD EARNED -{GetHeroProperName(whichUnit)}|r";
      if (strength > 0)
        display = $"{display}\n+{strength} Strength";
      if (agility > 0)
        display = $"{display}\n+{agility} Agility";
      if (intelligence > 0)
        display = $"{display}\n+{intelligence} Intelligence";
      if (experience > 0)
        display = $"{display}\n+{experience} Experience";
      DisplayTextToPlayer(GetOwningPlayer(whichUnit), 0, 0, display);
      if (GetLocalPlayer() == GetOwningPlayer(whichUnit)) 
        StartSound(SoundLibrary.Hint);
    }

    /// <summary>
    /// Displays a nicely formatted hint to the player that they have acquired a research.
    /// </summary>
    public static void DisplayResearchAcquired(this player whichPlayer, int researchId, int researchLevel)
    {
      DisplayTextToPlayer(whichPlayer, 0, 0,
        $"\n|cff00ff00RESEARCH ACQUIRED - {GetObjectName(researchId)}|r\n{BlzGetAbilityExtendedTooltip(researchId, researchLevel)}");
      if (GetLocalPlayer() == whichPlayer)
        StartSound(SoundLibrary.Hint);
    }

    /// <summary>
    /// Displays a nicely formatted hint to the player that they have unlocked a new unit type.
    /// </summary>
    public static void DisplayUnitTypeAcquired(this player whichPlayer, int unitId, string flavor)
    {
      DisplayTextToPlayer(whichPlayer, 0, 0,
        $"\n|cff00ff00NEW UNIT ACQUIRED - {GetObjectName(unitId)}\n|r{flavor}");
      if (GetLocalPlayer() == whichPlayer)
        StartSound(SoundLibrary.Hint);
    }

    /// <summary>
    /// Alerts the player that one of their researches has been refunded.
    /// </summary>
    public static void DisplayRefundedResearch(this player whichPlayer, int researchTypeId)
    {
      DisplayTextToPlayer(whichPlayer, 0, 0,
        $"\n|cff008000REFUND|r - You cannot research {GetObjectName(researchTypeId)}. All resources spent on it have been refunded.");
    }
    
    /// <summary>Removes all of the player's resources.</summary>
    public static player RemoveAllResources(this player whichPlayer)
    {
      whichPlayer.SetGold(0);
      whichPlayer.SetLumber(0);
      return whichPlayer;
    }
  }
}