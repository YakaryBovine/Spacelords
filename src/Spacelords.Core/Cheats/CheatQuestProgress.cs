﻿using MacroTools.CommandSystem;
using MacroTools.Extensions;
using MacroTools.FactionSystem;
using MacroTools.QuestSystem;
using static War3Api.Common;

namespace MacroTools.Cheats
{
  /// <summary>
  /// A command that sets a specified <see cref="QuestData"/>'s progress to the specified level.
  /// </summary>
  public sealed class CheatQuestProgress : Command
  {
    private readonly QuestProgress _progress;
    private readonly string _commandText;

    /// <inheritdoc />
    public override string CommandText => _commandText;
    
    /// <inheritdoc />
    public override bool Exact => false;

    /// <inheritdoc />
    public override int MinimumParameterCount => 1;

    /// <inheritdoc />
    public override CommandType Type => CommandType.Cheat;

    /// <inheritdoc />
    public override string Description => $"Sets the specified quest's progress to {_progress.ToString()} for the specified faction.";

    /// <summary>
    /// Initializes a new instance of the <see cref="CheatQuestProgress"/> class.
    /// </summary>
    public CheatQuestProgress(string commandText, QuestProgress progress)
    {
      _commandText = commandText;
      _progress = progress;
    }
    
    /// <inheritdoc />
    public override string Execute(player cheater, params string[] parameters)
    {
      Faction? faction;
      if (parameters.Length < 2)
      {
        faction = cheater.GetFaction();
        if (faction == null)
          return $"You are not playing as a {nameof(Faction)}, so you don't have any quests.";
      }
      else
      {
        faction = FactionManager.GetFactionByName(parameters[1]);
        if (faction == null) 
          return $"{parameters[1]} is not a valid {nameof(Faction)}.";
      }
      
      var quest = faction.GetQuestByTitle(parameters[0]);
      if (quest == null)
        return $"{parameters[0]} is not a valid quest for {nameof(Faction)} {faction.Name}.";
      
      quest.Progress = _progress;
      return $"Set quest progress of {quest.Title} to {_progress.ToString()} for {nameof(Faction)} {faction.Name}.";
    }
  }
}