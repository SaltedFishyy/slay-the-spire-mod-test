using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TestMod.TestModCode.Powers;

public sealed class KayleePower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigBetaIconPath => "res://Resources/Images/UI/Evidence.png";
    public override List<(string, string)>? Localization =>
        new PowerLoc("New Client: Kaylee", "Whenever you apply Weak to an enemy, apply the same amount of Expose.", "Whenever you apply Weak to an enemy, apply the same amount of Expose.");
    public override async Task AfterPowerAmountChanged(PlayerChoiceContext context, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power is not WeakPower || amount <= 0 || applier != Owner || power.Owner.Side != CombatSide.Enemy) return;
        int exposeAmount = (int)Math.Floor(amount) * Amount;
        await ExposeHelper.Apply(context, power.Owner, exposeAmount, Owner, cardSource);
    }
}
