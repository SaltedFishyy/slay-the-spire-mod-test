using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TestMod.TestModCode.Powers;

public sealed class KennyPower : CustomPowerModel
{
    private sealed class ProgressTracker(int threshold)
    {
        public int Threshold { get; } = threshold;
        public int Progress { get; set; }
    }

    // Power models are cloned from prototypes, so allocate mutable state lazily per combat instance.
    private List<ProgressTracker>? _trackers;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigBetaIconPath => "res://Resources/Images/UI/Evidence.png";
    public override List<(string, string)>? Localization =>
        new PowerLoc("New Client: Kenny", "Tracks Evidence gained for each Kenny. Yea Kenny is the best.", "Tracks Evidence gained for each Kenny. Actually, Kenny is the best best");

    public void AddTracker(int threshold)
    {
        if (threshold > 0)
            (_trackers ??= []).Add(new ProgressTracker(threshold));
    }

    public async Task RecordEvidenceGain(PlayerChoiceContext context, int amount)
    {
        if (amount <= 0 || _trackers is not { Count: > 0 } trackers || Owner.Player is not { } player)
            return;

        int drawCount = 0;
        foreach (ProgressTracker tracker in trackers)
        {
            int accumulated = tracker.Progress + amount;
            drawCount += accumulated / tracker.Threshold;
            tracker.Progress = accumulated % tracker.Threshold;
        }

        if (drawCount <= 0)
            return;

        Flash();
        await CardPileCmd.Draw(context, drawCount, player);
    }
}
