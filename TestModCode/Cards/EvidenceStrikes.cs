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
            (card, _) =>
            {
                int evidence = card.Owner.Creature.GetPower<EvidencePower>()?.Amount ?? 0;
                decimal multiplier = card.IsUpgraded ? 0.3m : 0.2m;
                return Math.Floor(evidence * multiplier);
            });

    public EvidenceStrikes()
        : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

    public override List<(string, string)>? Localization =>
        new CardLoc("Evidence Strikes", "Deal {CalculatedDamage} damage. Then lose 10% of your Evidence, rounded down.");

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        // 先保存打牌前的 Evidence，伤害和损失都以这个数值为准。
        int evidenceBeforeEffect = Owner.Creature.GetPower<EvidencePower>()?.Amount ?? 0;
        int evidenceLost = (int)Math.Floor(evidenceBeforeEffect * 0.1m);

        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        // Lose Evidence 不属于主动 Spend，因此不会增加任何 Spend 统计。
        await EvidencePower.LoseEvidence(
            choiceContext,
            Owner.Creature,
            evidenceLost,
            this);
    }

    protected override void OnUpgrade()
    {
        // 升级后费用变为 0，基础伤害变为 10，Evidence 加成由计算函数提高到 30%。
        EnergyCost.UpgradeBy(-1);
        DynamicVars.CalculationBase.UpgradeValueBy(2);
    }
}
