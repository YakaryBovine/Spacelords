using System.Collections.Generic;
using MacroTools.CommandSystem;
using WCSharp.Events;
using static War3Api.Common;

namespace MacroTools.Cheats
{
  public sealed class CheatMana : Command
  {
    /// <inheritdoc />
    public override string CommandText => "mana";
    
    /// <inheritdoc />
    public override bool Exact => false;
    
    /// <inheritdoc />
    public override int MinimumParameterCount => 1;
    
    /// <inheritdoc />
    public override CommandType Type => CommandType.Cheat;
    
    /// <inheritdoc />
    public override string Description => "When activated, causes your units to fully restore mana whenever they cast a spell.";
    
    private static readonly List<player> PlayersWithCheat = new();

    /// <inheritdoc />
    public override string Execute(player cheater, params string[] parameters)
    {
      var toggle = parameters[0];

      switch (toggle)
      {
        case "on":
          SetCheatActive(cheater, true);
          return "Infinite mana activated.";
        case "off":
          SetCheatActive(cheater, false);
          return "Infinite mana deactivated.";
        default:
          return "You must specify \"on\" or \"off\" as the first parameter.";
      }
    }

    /// <inheritdoc />
    public override void OnRegister() => 
      PlayerUnitEvents.Register(UnitTypeEvent.SpellEndCast, Spell);
    
    private static void SetCheatActive(player whichPlayer, bool isActive)
    {
      switch (isActive)
      {
        case true when !PlayersWithCheat.Contains(whichPlayer):
          PlayersWithCheat.Add(whichPlayer);
          return;
        case false when PlayersWithCheat.Contains(whichPlayer):
          PlayersWithCheat.Remove(whichPlayer);
          break;
      }
    }

    private static void Spell()
    {
      if (PlayersWithCheat.Contains(GetTriggerPlayer()))
        SetUnitState(GetTriggerUnit(), UNIT_STATE_MANA, GetUnitState(GetTriggerUnit(), UNIT_STATE_MAX_MANA));
    }
  }
}