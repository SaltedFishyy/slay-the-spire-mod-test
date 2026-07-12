using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace TestMod.TestModCode.Powers;

public sealed class NathanPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigBetaIconPath => "res://Resources/Images/UI/Evidence.png";
    public override List<(string, string)>? Localization =>
        new PowerLoc("New Client: Nathan", "Evidence gain is increased by 50% per stack.", "Evidence gain is increased by 50% per stack.");
}
