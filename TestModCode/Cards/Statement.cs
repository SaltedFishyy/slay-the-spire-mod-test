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
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/Statement.png";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        MakeCalculatedDamage(6, (card, _) => EvidenceHelper.Get(card.Owner?.Creature));

    public Statement()
        : base(2, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
    }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Statement",
            "Deal damage equal to {CalculationBase} + your current Evidence. Total: {CalculatedDamage:diff()}.");

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
        EnergyCost.UpgradeBy(-1);
        DynamicVars.CalculationBase.UpgradeValueBy(2);
    }
}
