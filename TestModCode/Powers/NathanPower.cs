using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace TestMod.TestModCode.Powers;

public sealed class NathanPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "res://Resources/Images/Power/NathanPower.png";
    public override string CustomBigIconPath => "res://Resources/Images/Power/NathanPower.png";
    public override string CustomBigBetaIconPath => "res://Resources/Images/Power/NathanPower.png";
    public override List<(string, string)>? Localization =>
        new PowerLoc("New Client: Nathan", "Whenever you would gain Evidence, gain {Amount} more.", "Whenever you would gain Evidence, gain {Amount} more.");
}
