﻿using System;
using MacroTools.Extensions;
using MacroTools.SpellSystem;
using WCSharp.Shared.Data;
using static War3Api.Common;

namespace MacroTools.Spells
{
  /// <summary>
  /// Opens a two-way Waygate for as long as the caster keeps channeling.
  /// </summary>
  public sealed class WaygateOpen : Spell
  {
    /// <summary>
    /// The unit type ID for the entrance Waygate to create.
    /// </summary>
    public int InteriorWaygateUnitTypeId { get; init; }
    
    /// <summary>
    /// The unit type ID for the exit Waygate to create.
    /// </summary>
    public int ExteriorWaygateUnitTypeId { get; init; }
    
    /// <summary>
    /// A function that returns where the exit waygate should be created.
    /// </summary>
    public Func<Point>? GetInteriorWaygatePosition { get; init; }
    
    /// <summary>
    /// A function that returns where the entrance Waygate should be created.
    /// </summary>
    public Func<Point>? GetExteriorWaygatePosition { get; init; }
    
    private unit? _exteriorWaygate;
    private unit? _interiorWaygate;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="WaygateOpen"/> class.
    /// </summary>
    /// <param name="id"></param>
    public WaygateOpen(int id) : base(id)
    {
    }
    
    /// <inheritdoc />
    public override void OnCast(unit caster, unit target, Point targetPoint)
    {
      if (GetExteriorWaygatePosition == null)
      {
        throw new InvalidOperationException(
          $"{nameof(WaygateOpen)} must declare both {nameof(GetExteriorWaygatePosition)}");
      }
      
      if (GetInteriorWaygatePosition == null)
      {
        throw new InvalidOperationException(
          $"{nameof(WaygateOpen)} must declare both {nameof(GetInteriorWaygatePosition)}");
      }
      
      var exteriorWaygatePosition = GetExteriorWaygatePosition();
      var interiorWaygatePosition = GetInteriorWaygatePosition();
      
      _exteriorWaygate = 
        CreateUnit(Player(PLAYER_NEUTRAL_PASSIVE), ExteriorWaygateUnitTypeId, exteriorWaygatePosition.X, exteriorWaygatePosition.Y, 0)
          .SetWaygateDestination(interiorWaygatePosition);
      
      _interiorWaygate = 
        CreateUnit(Player(PLAYER_NEUTRAL_PASSIVE), InteriorWaygateUnitTypeId, interiorWaygatePosition.X, interiorWaygatePosition.Y, 0)
          .SetWaygateDestination(exteriorWaygatePosition);
    }

    /// <inheritdoc />
    public override void OnStop(unit caster)
    {
      RemoveUnit(_exteriorWaygate);
      RemoveUnit(_interiorWaygate);
      _exteriorWaygate = null;
      _interiorWaygate = null;
    }
  }
}