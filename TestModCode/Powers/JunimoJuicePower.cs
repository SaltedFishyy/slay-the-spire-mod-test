using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace TestMod.TestModCode.Powers;

public sealed class JunimoJuicePower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigBetaIconPath => "res://Resources/Images/UI/Evidence.png";

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Junimo Juice",
            "You have {Amount} additional max Energy this combat.",
            "You have {Amount} additional max Energy this combat.");

    public override decimal ModifyMaxEnergy(Player player, decimal amount) =>
        Owner.Player == player ? amount + Amount : amount;
}
