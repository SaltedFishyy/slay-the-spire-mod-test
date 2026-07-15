using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TestMod.TestModCode.Powers;

public abstract class KennyProgressPower : CustomPowerModel
{
    protected abstract int Threshold { get; }

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    public override string CustomPackedIconPath => "res://Resources/Images/Power/KennyPower.png";
    public override string CustomBigIconPath => "res://Resources/Images/Power/KennyPower.png";
    public override string CustomBigBetaIconPath => "res://Resources/Images/Power/KennyPower.png";
    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "New Client: Kenny",
            $"Whenever you gain a total of {Threshold} Evidence, draw 1 card. The counter shows current progress.",
            $"Whenever you gain a total of {Threshold} Evidence, draw 1 card. The counter shows current progress.");

    public void InitializeProgress() => SetAmount(0);

    public int RecordEvidenceGain(int amount)
    {
        if (amount <= 0)
            return 0;

        int accumulated = Amount + amount;
        int drawCount = accumulated / Threshold;
        SetAmount(accumulated % Threshold);

        if (drawCount > 0)
            Flash();

        return drawCount;
    }
}

public sealed class KennyPower : KennyProgressPower
{
    protected override int Threshold => 7;
}

public sealed class KennyUpgradedPower : KennyProgressPower
{
    protected override int Threshold => 5;
}
