﻿using MacroTools.DummyCasters;
using MacroTools.SpellSystem;
using WCSharp.Shared.Data;
using static War3Api.Common;

namespace MacroTools.Spells
{
  public sealed class ChannelAnySpellCaster : Spell
  {
    public int DummyAbilityId { get; init; }
    public string DummyAbilityOrderString { get; init; }
    public int Duration { get; init; }

    public ChannelAnySpellCaster(int id) : base(id)
    {
    }

    public override void OnCast(unit caster, unit target, Point targetPoint)
    {
      DummyCasterManager.GetLongLivedDummyCaster().ChannelAtCaster(caster, DummyAbilityId, DummyAbilityOrderString, GetAbilityLevel(caster), Duration);
    }
  }
}