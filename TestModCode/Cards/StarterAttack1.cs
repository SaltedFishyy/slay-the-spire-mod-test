using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

// Phase 1 攻击测试卡：伤害会读取打出时的当前 Evidence。
public sealed class Statement : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        MakeCalculatedDamage(
            4,
            (card, _) =>
            {
                int evidence = card.Owner.Creature.GetPower<EvidencePower>()?.Amount ?? 0;
                return card.IsUpgraded ? Math.Floor(evidence * 1.5m) : evidence;
            });

    public Statement()
        : base(2, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
    }

    public override List<(string, string)>? Localization =>
        new CardLoc("Statement", "Deal {CalculatedDamage} damage, based on your Evidence.");

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        // 没有 EvidencePower 时按 0 层处理。
        // 这张牌只读取 Evidence 计算伤害，不会消耗或减少 Evidence。
        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
        DynamicVars.CalculationBase.UpgradeValueBy(-4);
    }
}
