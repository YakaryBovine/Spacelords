using MacroTools.PassiveAbilitySystem;
using MacroTools.Save;

namespace TestMap.Source.Setup
{
  public static class GameSetup
  {
    public static void Setup()
    {
      SaveManager.Initialize();
      PassiveAbilityManager.InitializePreplacedUnits();
    }
  }
}