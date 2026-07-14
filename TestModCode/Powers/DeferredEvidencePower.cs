using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TestMod.TestModCode.Powers;

public sealed class DeferredEvidencePower : CustomPowerModel
{
    // Power models are cloned from prototypes, so allocate mutable state lazily per combat instance.
    private List<int>? _pendingReturns;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "res://Resources/Images/Power/DeferredEvidencePower.png";
    public override string CustomBigIconPath => "res://Resources/Images/Power/DeferredEvidencePower.png";
    public override string CustomBigBetaIconPath => "res://Resources/Images/Power/DeferredEvidencePower.png";
    public override List<(string, string)>? Localization =>
        new PowerLoc("Deferred Evidence", "At the start of your next turn, gain {Amount} Evidence.", "At the start of your next turn, gain {Amount} Evidence.");

    public void AddPendingReturn(int amount)
    {
        if (amount > 0)
            (_pendingReturns ??= []).Add(amount);
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext context, Player player)
    {
        if (Owner.Player != player || Amount <= 0) return;

        List<int> pending = _pendingReturns is { Count: > 0 } returns ? [.. returns] : [Amount];
        _pendingReturns?.Clear();
        Flash();
        foreach (int amount in pending)
            await EvidenceHelper.Gain(context, Owner, amount);

        await PowerCmd.Remove(this);
    }
}
