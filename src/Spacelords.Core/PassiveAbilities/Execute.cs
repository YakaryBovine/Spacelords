﻿using MacroTools.ControlPointSystem;
using MacroTools.Extensions;
using MacroTools.PassiveAbilitySystem;
using static War3Api.Common;

namespace MacroTools.PassiveAbilities
{
  /// <summary>
  /// Causes the unit to instantly kill enemies who drop below a certain threshold.
  /// </summary>
  public sealed class Execute : PassiveAbility, IAppliesEffectOnDamage
  {
    private const string Effect = "Objects\\Spawnmodels\\Human\\HumanLargeDeathExplode\\HumanLargeDeathExplode.mdl";

    /// <summary>
    /// Non-resistant enemies are instantly killed when their hit points drop below the caster's attack damage multiplied by this value.
    /// </summary>
    public float DamageMultNonResistant { get; init; }

    /// <summary>
    /// Resistant enemies are instantly killed when their hit points drop below the caster's attack damage multiplied by this value.
    /// </summary>
    public float DamageMultResistant { get; init; }

    /// <summary>
    /// Structures are instantly killed when their hit points drop below the caster's attack damage multiplied by this value.
    /// </summary>
    public float DamageMultStructure { get; init; }

  
    /// <summary>
    /// Initializes a new instance of the <see cref="Execute"/> class.
    /// </summary>
    /// <param name="unitTypeId"><inheritdoc /></param>
    public Execute(int unitTypeId) : base(unitTypeId)
    {
    }

    /// <inheritdoc />
    public void OnDealsDamage()
    {
      var triggerUnit = GetTriggerUnit();

      var damageMult = 1f;
      if (IsUnitType(triggerUnit, UNIT_TYPE_STRUCTURE) || ControlPointManager.Instance.UnitIsControlPoint(triggerUnit))
      {
        damageMult = DamageMultStructure;
      }
      else if (triggerUnit.IsResistant())
      {
        damageMult = DamageMultResistant;
      }
      else if (!triggerUnit.IsResistant())
      {
        damageMult = DamageMultNonResistant;
      }

      if (damageMult == 1f)
        return;

      if (!BlzGetEventIsAttack() || !(GetUnitState(triggerUnit, UNIT_STATE_LIFE) < GetEventDamage() + GetEventDamageSource().GetAverageDamage(0) * damageMult))
        return;

      BlzSetEventDamage(GetUnitState(triggerUnit, UNIT_STATE_LIFE) + 1);
      BlzSetEventDamageType(DAMAGE_TYPE_UNIVERSAL);
      DestroyEffect(AddSpecialEffectTarget(Effect, triggerUnit, "origin"));
    }
  }
}