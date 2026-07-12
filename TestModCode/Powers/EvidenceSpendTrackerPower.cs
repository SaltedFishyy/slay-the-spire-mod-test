using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace TestMod.TestModCode.Powers;

// Hidden combat tracker for Evidence spent as an active cost.
public sealed class EvidenceSpendTrackerPower : CustomPowerModel
{
    private int _spentThisTurn;
    private int _trackedTurnNumber = -1;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override bool IsVisibleInternal => false;
    public override bool ShouldPlayVfx => false;

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Evidence Spend Tracker",
            "Tracks Evidence spent during this combat.",
            "Tracks Evidence spent during this combat.");

    public int SpendCountThisCombat => Amount;

    public int EvidenceSpentThisTurn
    {
        get
        {
            int currentTurn = Owner.Player?.PlayerCombatState?.TurnNumber ?? -1;
            return currentTurn == _trackedTurnNumber ? _spentThisTurn : 0;
        }
    }

    public void RecordSpendThisTurn(int amount)
    {
        int currentTurn = Owner.Player?.PlayerCombatState?.TurnNumber ?? -1;
        if (currentTurn != _trackedTurnNumber)
        {
            _trackedTurnNumber = currentTurn;
            _spentThisTurn = 0;
        }

        _spentThisTurn += amount;
    }
}
