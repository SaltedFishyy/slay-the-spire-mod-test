using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using TestMod.TestModCode.Cards;

namespace TestMod.TestModCode.Powers;

// 在战斗中记录主动 Spend Evidence 的数据，不作为玩家可见状态显示。
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

    // Power 的层数就是本场战斗主动 Spend Evidence 的次数。
    public int SpendCountThisCombat => Amount;

    // 回合变化时旧数据自动视为零，不需要额外的回合补丁。
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

    // Class Action 每次主动 Spend 后降低 3 点费用，最低为 0。
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

        modifiedCost = Math.Max(0, originalCost - SpendCountThisCombat * 3);
        return true;
    }
}
