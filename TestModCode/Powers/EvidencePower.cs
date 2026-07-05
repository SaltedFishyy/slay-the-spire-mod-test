using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TestMod.TestModCode.Powers;

// Evidence 使用标准 Power 表示，因此可以直接复用游戏现有的状态图标和层数显示。
public sealed class EvidencePower : CustomPowerModel
{
    // Counter 允许重复获得 Evidence 时把层数累加。
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Evidence",
            "You have {Amount} Evidence.",
            "You have {Amount} Evidence.");

    // 供明确写有“Lose Evidence”的卡牌调用，不会由普通攻击自动触发。
    public static async Task<int> LoseEvidence(
        PlayerChoiceContext choiceContext,
        Creature owner,
        int amount,
        CardModel? source)
    {
        EvidencePower? evidence = owner.GetPower<EvidencePower>();
        if (evidence is null || amount <= 0)
        {
            return 0;
        }

        // 最多只扣除当前拥有的 Evidence，避免资源变成负数。
        int evidenceLost = Math.Min(amount, evidence.Amount);
        await PowerCmd.ModifyAmount(
            choiceContext,
            evidence,
            -evidenceLost,
            owner,
            source);

        return evidenceLost;
    }

    // Spend 会扣除 Evidence，并记录本回合消费量和本场战斗消费次数。
    public static async Task<int> SpendEvidence(
        PlayerChoiceContext choiceContext,
        Creature owner,
        int amount,
        CardModel? source)
    {
        int evidenceSpent = await LoseEvidence(
            choiceContext,
            owner,
            amount,
            source);

        if (evidenceSpent <= 0)
        {
            return 0;
        }

        EvidenceSpendTrackerPower? tracker = owner.GetPower<EvidenceSpendTrackerPower>();
        if (tracker is null)
        {
            await PowerCmd.Apply<EvidenceSpendTrackerPower>(
                choiceContext,
                owner,
                1,
                owner,
                source);
            tracker = owner.GetPower<EvidenceSpendTrackerPower>();
        }
        else
        {
            await PowerCmd.ModifyAmount(
                choiceContext,
                tracker,
                1,
                owner,
                source);
        }

        tracker?.RecordSpendThisTurn(evidenceSpent);
        return evidenceSpent;
    }
}
