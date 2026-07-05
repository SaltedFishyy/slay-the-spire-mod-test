using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

// 一张普通攻击牌，先按当前 Evidence 计算伤害，再明确失去其中的 10%。
public sealed class EvidenceStrikes : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        MakeCalculatedDamage(
            8,
            (card, _) => Math.Floor(
                (card.Owner.Creature.GetPower<EvidencePower>()?.Amount ?? 0) * 0.2m));

    public EvidenceStrikes()
        : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

    public override List<(string, string)>? Localization =>
        new CardLoc("Evidence Strikes", "Deal {CalculatedDamage} damage + 20% of your Evidence.");

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        // CalculatedDamage 会在全局攻击惩罚扣除 Evidence 之前读取当前层数。
        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        // 升级把基础伤害从 8 提高到 10，Evidence 加成仍然是 20%。
        DynamicVars.CalculationBase.UpgradeValueBy(2);
    }
}
