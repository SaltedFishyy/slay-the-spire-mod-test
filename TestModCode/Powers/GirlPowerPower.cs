using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TestMod.TestModCode.Powers;

public sealed class GirlPowerPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigBetaIconPath => "res://Resources/Images/UI/Evidence.png";

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Girl Power",
            "Whenever a friendly card applies Expose to an enemy, apply {Amount} Weak to it.",
            "Whenever a friendly card applies Expose to an enemy, apply {Amount} Weak to it.");

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext context,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (power is not ExposePower ||
            amount <= 0 ||
            Amount <= 0 ||
            power.Owner.Side != CombatSide.Enemy ||
            applier?.Side != CombatSide.Player ||
            cardSource is null)
        {
            return;
        }

        Flash();
        await PowerCmd.Apply<WeakPower>(context, power.Owner, Amount, Owner, null);
    }
}
