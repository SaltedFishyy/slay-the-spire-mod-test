using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

// 一张罕见攻击牌，额外伤害等于当前 Evidence 的一半。
public sealed class CrossExamine : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        MakeCalculatedDamage(
            8,
            (card, _) =>
            {
                int evidence = card.Owner.Creature.GetPower<EvidencePower>()?.Amount ?? 0;
                decimal multiplier = card.IsUpgraded ? 0.7m : 0.5m;
                return Math.Floor(evidence * multiplier);
            });

    public CrossExamine()
        : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    public override List<(string, string)>? Localization =>
        new CardLoc("Cross-Examine", "Deal {CalculatedDamage} damage (base damage plus 50% of your Evidence, rounded down).");

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        // 伤害会读取当前 Evidence，但打出这张攻击牌不会减少 Evidence。
        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        // 升级把基础伤害从 8 提高到 10，Evidence 加成由计算函数提高到 70%。
        DynamicVars.CalculationBase.UpgradeValueBy(2);
    }
}
