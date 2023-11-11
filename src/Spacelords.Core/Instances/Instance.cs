using System;
using System.Collections.Generic;
using System.Linq;
using MacroTools.Extensions;
using MacroTools.Libraries;
using WCSharp.Shared.Data;
using static War3Api.Common;

namespace MacroTools.Instances
{
  /// <summary>
  /// An instance is a region that is physically seperate from all other instances irrespective of its actual geographical
  /// location on the Warcraft map.
  /// </summary>
  public sealed class Instance
  {
    /// <summary>The entire region that constitutes the physical area of the <see cref="Instance"/>.</summary>
    public region Region { get; }

    /// <summary>The number of <see cref="Gate"/>s that can be used to enter or exit the <see cref="Instance"/>.</summary>
    public int GateCount => _gates.Count;
    
    private readonly trigger _dependencyDiesTrigger;
    private readonly List<Gate> _gates = new();
    private readonly string _name;
    private readonly Rectangle[] _rectangles;
    private readonly List<unit> _dependencies = new();
    
    /// <summary>Initializes a new instance of the <see cref="Instance"/> class.</summary>
    public Instance(string name, IEnumerable<Rectangle> areas)
    {
      Region = CreateRegion();
      _rectangles = areas.ToArray();
      foreach (var rectangle in _rectangles)
        RegionAddRect(Region, rectangle.Rect);
      _name = name;
      _dependencyDiesTrigger = CreateTrigger()
        .AddAction(Destroy);
    }

    /// <summary>Initializes a new instance of the <see cref="Instance"/> class.</summary>
    public Instance(string name, Rectangle area) : this(name, new[] {area})
    {
    }
    
    /// <summary>
    /// Adds a <see cref="Gate"/> to the <see cref="Instance"/>, indicating where it can be entered and exited.
    /// </summary>
    public void AddGate(Gate gate) => _gates.Add(gate);

    /// <summary>
    ///   Gets the <see cref="Gate" /> nearest the given position, if any.
    /// </summary>
    public Gate GetNearestGate(Point position)
    {
      float distanceToNearestGate = 0;
      Gate? nearestGate = null;
      
      foreach (var gate in _gates)
      {
        var distance = MathEx.GetDistanceBetweenPoints(position, gate.InteriorPosition);
        if (distance <= distanceToNearestGate)
        {
          nearestGate = gate;
          distanceToNearestGate = distance;
        }
      }

      if (nearestGate == null)
        throw new InvalidOperationException(
          $"Could not find the nearest {nameof(Gate)} for {nameof(Instance)} {_name} at position {position.X}, {position.Y}.");
      
      return nearestGate;
    }

    /// <summary>
    ///   Makes the <see cref="Instance" /> dependent on the given <see cref="unit" /> being alive.
    ///   When any of the dependent <see cref="unit" />s die, every unit in the <see cref="Instance" /> is killed.
    /// </summary>
    public void AddDependency(unit dependency)
    {
      _dependencies.Add(dependency);
      _dependencyDiesTrigger.RegisterUnitEvent(dependency, EVENT_UNIT_DEATH);
    }
    
    /// <summary>
    /// Kills all units in the instance, then evacuates all units and items to the nearest exterior <see cref="Gate"/>.
    /// </summary>
    private void Destroy()
    {
      try
      {
        _dependencyDiesTrigger.Destroy();
        foreach (var rect in _rectangles)
        {
          var unitsInRect = CreateGroup()
            .EnumUnitsInRect(rect)
            .EmptyToList();

          var evacuationPosition = _gates.First().ExteriorPosition;
          
          KillUnits(unitsInRect);
          EvacuateUnits(unitsInRect, evacuationPosition);
          EvacuateItemsInRect(rect, evacuationPosition);
        }

        foreach (var unit in _dependencies)
          unit.Kill();
      }
      catch (Exception ex)
      {
        Logger.LogError(ex.ToString());
      }
    }

    private static void KillUnits(List<unit> unitsToKill)
    {
      foreach (var unit in unitsToKill)
        if (!BlzIsUnitInvulnerable(unit))
          KillUnit(unit);
    }
    
    private static void EvacuateUnits(List<unit> unitsInRect, Point evacuationPosition)
    {
      foreach (var unit in unitsInRect) 
        unit.SetPosition(evacuationPosition);
    }

    private static void EvacuateItemsInRect(Rectangle rect, Point evacuationPosition)
    {
      EnumItemsInRect(rect.Rect, null, () =>
      {
        try
        {
          var enumItem = GetEnumItem();
          enumItem.SetPosition(evacuationPosition);
        }
        catch (Exception ex)
        {
          Logger.LogError(ex.ToString());
        }
      });
    }
  }
}