﻿using System.Linq;
using MacroTools.CommandSystem;
using MacroTools.Extensions;
using MacroTools.LegendSystem;
using static War3Api.Common;

namespace MacroTools.Cheats
{
  public sealed class CheatPermaKill : Command
  {
    /// <inheritdoc />
    public override string CommandText => "permakill";

    /// <inheritdoc />
    public override bool Exact => true;

    /// <inheritdoc />
    public override int MinimumParameterCount => 0;

    /// <inheritdoc />
    public override CommandType Type => CommandType.Cheat;

    /// <inheritdoc />
    public override string Description => "Permanently kills the selected Legends.";

    /// <inheritdoc />
    public override string Execute(player cheater, params string[] parameters)
    {
      var selectedUnits = CreateGroup().EnumSelectedUnits(cheater).EmptyToList();
      if (!selectedUnits.Any())
        return "You're not selecting any Legends.";
      
      foreach (var unit in selectedUnits)
      {
        var asLegend = LegendaryHeroManager.GetFromUnit(unit);
        asLegend?.PermanentlyKill();
      }

      return "Permanently killed all selected Legends.";
    }
  }
}