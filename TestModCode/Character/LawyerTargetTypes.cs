using BaseLib.Patches.Content;
using BaseLib.Patches.Features;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TestMod.TestModCode.Character;

public static class LawyerTargetTypes
{
    private static bool _registered;

    [CustomEnum]
    public static TargetType AnyWeakEnemy;

    public static void EnsureRegistered()
    {
        if (_registered)
        {
            return;
        }

        CustomTargetType.RegisterSingleTargetType(AnyWeakEnemy, CanTargetWeakEnemy);
        _registered = true;
    }

    private static bool CanTargetWeakEnemy(Creature creature) =>
        creature.IsAlive &&
        creature.Side == CombatSide.Enemy &&
        (creature.GetPower<WeakPower>()?.Amount ?? 0) > 0;
}
