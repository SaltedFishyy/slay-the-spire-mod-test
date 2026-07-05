using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

// Phase 1 攻击测试卡：伤害会读取打出时的当前 Evidence。
public sealed class StarterAttack1 : LawyerCard
{
    public StarterAttack1()
        : base(2, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
    }

    public override List<(string, string)>? Localization =>
        new CardLoc("Starter Attack 1", "Deal 8 damage plus your current Evidence.");

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        // 没有 EvidencePower 时按 0 层处理。
        int evidence = Owner.Creature.GetPower<EvidencePower>()?.Amount ?? 0;
        int damage = 8 + evidence;

        // 这张牌只读取 Evidence 计算伤害，不会消耗或减少 Evidence。
        await DamageCmd.Attack(damage)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }
}
