using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace TestMod.TestModCode.Powers;

public sealed class CourtroomControlPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigBetaIconPath => "res://Resources/Images/UI/Evidence.png";
    public override List<(string, string)>? Localization =>
        new PowerLoc("Courtroom Control", "Whenever you gain Evidence, gain {Amount} additional Evidence.", "Whenever you gain Evidence, gain {Amount} additional Evidence.");
}
