using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TestMod.TestModCode.Powers;

public sealed class BigBrainPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigBetaIconPath => "res://Resources/Images/UI/Evidence.png";

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Big Brain",
            "Whenever Expose triggers, gain {Amount} Evidence.",
            "Whenever Expose triggers, gain {Amount} Evidence.");

    public async Task OnExposeTriggered(PlayerChoiceContext context)
    {
        if (Amount <= 0)
            return;

        Flash();
        await EvidenceHelper.Gain(context, Owner, Amount);
    }
}
