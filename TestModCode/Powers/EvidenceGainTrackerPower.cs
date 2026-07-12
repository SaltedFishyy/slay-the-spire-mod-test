using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using TestMod.TestModCode.Cards;

namespace TestMod.TestModCode.Powers;

// Hidden combat tracker for "gain Evidence" events.
public sealed class EvidenceGainTrackerPower : CustomPowerModel
{
    private const int ClassActionDiscountPerGain = 2;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override bool IsVisibleInternal => false;
    public override bool ShouldPlayVfx => false;

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Evidence Gain Tracker",
            "Tracks Evidence gained during this combat.",
            "Tracks Evidence gained during this combat.");

    public int GainCountThisCombat => Amount;

    public override bool TryModifyEnergyCostInCombat(
        CardModel card,
        decimal originalCost,
        out decimal modifiedCost)
    {
        if (card is not ClassAction || card.Owner.Creature != Owner)
        {
            modifiedCost = originalCost;
            return false;
        }

        modifiedCost = Math.Max(0, originalCost - GainCountThisCombat * ClassActionDiscountPerGain);
        return true;
    }
}
