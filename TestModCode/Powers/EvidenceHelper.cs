using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TestMod.TestModCode.Powers;

// 统一处理 Evidence 的获得、主动支付和效果损失，避免各张卡直接修改 Power 数量。
public static class EvidenceHelper
{
    public static int Get(Creature? creature) =>
        Math.Max(0, creature?.GetPower<EvidencePower>()?.Amount ?? 0);

    public static async Task Gain(PlayerChoiceContext context, Creature creature, int amount, CardModel? source = null)
    {
        if (amount <= 0)
            return;

        int previousAmount = Get(creature);
        int nathanBonus = Math.Max(0, creature.GetPower<NathanPower>()?.Amount ?? 0);
        int totalAmount = amount + nathanBonus;

        await PowerCmd.Apply<EvidencePower>(context, creature, totalAmount, creature, source);
        int actualGain = Math.Max(0, Get(creature) - previousAmount);
        if (actualGain <= 0)
            return;

        EvidenceGainTrackerPower? gainTracker =
            await PowerCmd.Apply<EvidenceGainTrackerPower>(context, creature, 1, creature, source);
        gainTracker?.RecordGainThisTurn(actualGain);

        CourtroomControlPower? courtroom = creature.GetPower<CourtroomControlPower>();
        if (courtroom is not null)
            await courtroom.AfterEvidenceGained(context);

        int kennyDrawCount = creature.Powers
            .OfType<KennyProgressPower>()
            .Sum(kenny => kenny.RecordEvidenceGain(actualGain));
        if (kennyDrawCount > 0 && creature.Player is { } player)
            await CardPileCmd.Draw(context, kennyDrawCount, player);
    }

    // Lose 不属于主动支付，因此不会写入本回合 spend tracker。
    public static async Task<int> Lose(PlayerChoiceContext context, Creature creature, int amount, CardModel? source = null)
    {
        EvidencePower? evidence = creature.GetPower<EvidencePower>();
        int lost = Math.Min(Math.Max(0, amount), evidence?.Amount ?? 0);
        if (evidence is null || lost <= 0)
            return 0;

        await PowerCmd.ModifyAmount(context, evidence, -lost, creature, source);
        return lost;
    }

    public static Task<int> LoseAll(PlayerChoiceContext context, Creature creature, CardModel? source = null) =>
        Lose(context, creature, Get(creature), source);

    // Spend 是主动费用：减少 Evidence，并同时记录支付量与支付事件次数。
    public static async Task<bool> Spend(PlayerChoiceContext context, Creature creature, int amount, CardModel? source = null)
    {
        amount = Math.Max(0, amount);
        if (Get(creature) < amount)
            return false;
        if (amount == 0)
            return true;

        int lost = await Lose(context, creature, amount, source);
        if (lost != amount)
            return false;

        EvidenceSpendTrackerPower? tracker =
            await PowerCmd.Apply<EvidenceSpendTrackerPower>(context, creature, 1, creature, source);
        tracker?.RecordSpendThisTurn(amount);

        return true;
    }
}
