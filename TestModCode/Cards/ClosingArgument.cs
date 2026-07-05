using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

// 根据本回合主动消费的 Evidence 总量造成三倍伤害。
public sealed class ClosingArgument : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        MakeCalculatedDamage(
            0,
            (card, _) =>
                (card.Owner.Creature.GetPower<EvidenceSpendTrackerPower>()?.EvidenceSpentThisTurn ?? 0) * 3);

    public ClosingArgument()
        : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    public override List<(string, string)>? Localization =>
        new CardLoc("Closing Argument", "Deal {CalculatedDamage} damage, equal to 3 times the Evidence spent this turn.");

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        // 升级只把费用从 1 降为 0，伤害公式不变。
        EnergyCost.UpgradeBy(-1);
    }
}
