using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TestMod.TestModCode.Powers;

public sealed class PaycheckPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigBetaIconPath => "res://Resources/Images/UI/Evidence.png";
    public override List<(string, string)>? Localization =>
        new PowerLoc("PAYCHECK", "Whenever an enemy dies, gain {Amount} Gold.", "Whenever an enemy dies, gain {Amount} Gold.");
    public override async Task AfterDeath(PlayerChoiceContext context, Creature creature, bool wasAlreadyDead, float deathDelay)
    {
        if (wasAlreadyDead || creature.Side != CombatSide.Enemy || Owner.Player is not { } player) return;
        await PlayerCmd.GainGold(Amount, player, false);
    }
}
