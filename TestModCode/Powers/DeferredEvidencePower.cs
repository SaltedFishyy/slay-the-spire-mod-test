using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TestMod.TestModCode.Powers;

public sealed class DeferredEvidencePower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigBetaIconPath => "res://Resources/Images/UI/Evidence.png";
    public override List<(string, string)>? Localization =>
        new PowerLoc("Deferred Evidence", "At the start of your next turn, gain {Amount} Evidence.", "At the start of your next turn, gain {Amount} Evidence.");
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext context, Player player)
    {
        if (Owner.Player != player || Amount <= 0) return;
        int amount = Amount;
        Flash();
        await EvidenceHelper.Gain(context, Owner, amount);
        await PowerCmd.Remove(this);
    }
}
