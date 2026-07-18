using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TestMod.TestModCode.Powers;

public abstract class BurdenOfProofPowerBase : CustomPowerModel
{
    protected abstract int EvidenceThreshold { get; }

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigBetaIconPath => "res://Resources/Images/UI/Evidence.png";

    public override decimal ModifyDamageAdditive(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (target is null ||
            dealer != Owner ||
            cardSource?.Type != CardType.Attack ||
            !props.IsPoweredAttack() ||
            Amount <= 0)
        {
            return 0m;
        }

        return (EvidenceHelper.Get(Owner) / EvidenceThreshold) * Amount;
    }
}

public sealed class BurdenOfProofPower : BurdenOfProofPowerBase
{
    protected override int EvidenceThreshold => 8;

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Burden of Proof",
            "Your Attack card hits deal {Amount} additional damage for every 8 Evidence you have.",
            "Your Attack card hits deal {Amount} additional damage for every 8 Evidence you have.");
}

public sealed class UpgradedBurdenOfProofPower : BurdenOfProofPowerBase
{
    protected override int EvidenceThreshold => 5;

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Burden of Proof+",
            "Your Attack card hits deal {Amount} additional damage for every 5 Evidence you have.",
            "Your Attack card hits deal {Amount} additional damage for every 5 Evidence you have.");
}
