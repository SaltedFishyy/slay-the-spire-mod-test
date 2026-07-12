using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TestMod.TestModCode.Powers;

// # Expose 共用操作：目前只转换 Weak 与 Vulnerable，避免误删特殊 debuff。
public static class ExposeHelper
{
    public static int Get(Creature? target) =>
        Math.Max(0, target?.GetPower<ExposePower>()?.Amount ?? 0);

    public static async Task<ExposePower?> Apply(
        PlayerChoiceContext choiceContext,
        Creature? target,
        int amount,
        Creature? applier = null,
        CardModel? cardSource = null)
    {
        if (target is null || amount <= 0)
        {
            return null;
        }

        return await PowerCmd.Apply<ExposePower>(
            choiceContext,
            target,
            amount,
            applier,
            cardSource);
    }

    public static async Task<bool> Decrement(Creature? target)
    {
        ExposePower? expose = target?.GetPower<ExposePower>();
        if (expose is null || expose.Amount <= 0)
        {
            return false;
        }

        await PowerCmd.Decrement(expose);
        return true;
    }

    public static async Task<bool> Remove(Creature? target)
    {
        ExposePower? expose = target?.GetPower<ExposePower>();
        if (expose is null)
        {
            return false;
        }

        await PowerCmd.Remove(expose);
        return true;
    }

    public static async Task<int> ConvertWeakVulnerableToExpose(
        PlayerChoiceContext choiceContext,
        Creature target,
        Creature? applier,
        CardModel? cardSource)
    {
        WeakPower? weak = target.GetPower<WeakPower>();
        VulnerablePower? vulnerable = target.GetPower<VulnerablePower>();
        int exposeAmount = Math.Max(0, weak?.Amount ?? 0) +
                           Math.Max(0, vulnerable?.Amount ?? 0);

        if (exposeAmount == 0)
        {
            return 0;
        }

        if (weak is not null)
        {
            await PowerCmd.Remove(weak);
        }

        if (vulnerable is not null)
        {
            await PowerCmd.Remove(vulnerable);
        }

        await Apply(
            choiceContext,
            target,
            exposeAmount,
            applier,
            cardSource);

        return exposeAmount;
    }
}
