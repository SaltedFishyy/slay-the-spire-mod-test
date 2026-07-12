using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

// 非普通攻击：基础伤害加当前 Evidence 的指定比例，最终结果向下取整。
public sealed class CrossExamine : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        MakeCalculatedDamage(10, (card, _) => Math.Floor(
            EvidenceHelper.Get(card.Owner?.Creature) * (card.IsUpgraded ? 0.7m : 0.5m)));

    public CrossExamine() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Cross Examine",
            "{IfUpgraded:show:Deal damage equal to 12 + 70% of your current Evidence.|Deal damage equal to 10 + 50% of your current Evidence.} Total: {CalculatedDamage}.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this).Targeting(cardPlay.Target).Execute(context);
    }

    protected override void OnUpgrade() => DynamicVars.CalculationBase.UpgradeValueBy(2);
}
