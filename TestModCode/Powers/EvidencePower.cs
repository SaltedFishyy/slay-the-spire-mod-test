using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace TestMod.TestModCode.Powers;

// Evidence 使用标准 Power 表示，因此可以直接复用游戏现有的状态图标和层数显示。
public sealed class EvidencePower : CustomPowerModel
{
    // Counter 允许重复获得 Evidence 时把层数累加。
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigBetaIconPath => "res://Resources/Images/UI/Evidence.png";

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Evidence",
            "You have {Amount} Evidence.",
            "You have {Amount} Evidence.");
}
