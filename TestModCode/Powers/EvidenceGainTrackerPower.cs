using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using TestMod.TestModCode.Cards;

namespace TestMod.TestModCode.Powers;

// Hidden combat tracker for "gain Evidence" events.
public sealed class EvidenceGainTrackerPower : CustomPowerModel
{
    private const int ClassActionDiscountPerGain = 2;
    private int _gainedThisTurn;
    private int _trackedTurnNumber = -1;

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

    public int EvidenceGainedThisTurn
    {
        get
        {
            int currentTurn = Owner.Player?.PlayerCombatState?.TurnNumber ?? -1;
            return currentTurn == _trackedTurnNumber ? _gainedThisTurn : 0;
        }
    }

    public void RecordGainThisTurn(int amount)
    {
        if (amount <= 0)
            return;

        int currentTurn = Owner.Player?.PlayerCombatState?.TurnNumber ?? -1;
        if (currentTurn != _trackedTurnNumber)
        {
            _trackedTurnNumber = currentTurn;
            _gainedThisTurn = 0;
        }

        _gainedThisTurn += amount;
    }

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
