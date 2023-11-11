﻿using System.Collections.Generic;
using System.Linq;
using MacroTools.ControlPointSystem;
using MacroTools.Extensions;
using MacroTools.FactionSystem;
using MacroTools.UserInterface;
using static War3Api.Common;

namespace MacroTools.FactionChoices
{
  /// <summary>Allows a player to choose between one of two factions at the start of the game.</summary>
  public sealed class FactionChoiceDialogPresenter : ChoiceDialogPresenter<Faction>
  {
    /// <summary>Initializes a new instance of the <see cref="FactionChoiceDialogPresenter"/> class.</summary>
    public FactionChoiceDialogPresenter(params Faction[] factions) : base(ConvertToFactionChoices(factions),
      "Pick your Faction")
    {
    }

    protected override void OnChoicePicked(player pickingPlayer, Choice<Faction> choice)
    {
      var pickedFaction = choice.Data;
      HasChoiceBeenPicked = true;
      if (GetLocalPlayer() == pickingPlayer && pickedFaction.StartingCameraPosition != null)
      {
        var startingCameraPosition = pickedFaction.StartingCameraPosition;
        if (startingCameraPosition != null)
          SetCameraPosition(startingCameraPosition.X,
            startingCameraPosition.Y);
      }

      //Todo: once all Factions are moved to the new Faction model, remove the if statement and always register
      //Todo: the faction, as a player should never be picking an already registered faction.
      if (FactionManager.GetFactionByName(pickedFaction.Name) == null)
        FactionManager.Register(pickedFaction);
      
      pickingPlayer.SetFaction(pickedFaction);
      var startingUnits = pickedFaction.StartingUnits;
      pickingPlayer.RescueGroup(startingUnits);

      foreach (var unpickedFaction in Choices.Where(x => x.Data != choice.Data))
        RemoveFaction(unpickedFaction.Data);
    }

    /// <inheritdoc />
    protected override Choice<Faction> GetDefaultChoice(player whichPlayer) => Choices.First();

    private static void RemoveFaction(Faction faction)
    {
      var startingUnits = faction.StartingUnits;
      foreach (var unit in startingUnits)
        if (ControlPointManager.Instance.UnitIsControlPoint(unit))
          unit.Rescue(Player(PLAYER_NEUTRAL_AGGRESSIVE));
        else
          unit.Remove();
      
      faction.OnNotPicked();
    }
    
    private static Choice<Faction>[] ConvertToFactionChoices(IEnumerable<Faction> factions)
    {
      return factions
        .Select(x => new Choice<Faction>(x, $"{x.Name} {x.LearningDifficulty.ToColoredText()}"))
        .ToArray();
    }
  }
}