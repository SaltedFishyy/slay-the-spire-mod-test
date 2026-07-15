using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class YouAreLying : LawyerCard
{
    private bool _isResolving;
    private bool _targetHadExpose;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        MakeCalculatedDamage(
            12,
            (card, target) => card is YouAreLying lying && lying.ShouldAddBonus(target)
                ? card.DynamicVars.CalculationBase.BaseValue
                : 0);

    public YouAreLying() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "YOU ARE LYING",
            "Deal {CalculationBase} damage. If the enemy has Expose, deal {CalculationBase} additional damage. Total: {CalculatedDamage:diff()}.");

    private bool ShouldAddBonus(Creature? target) =>
        _isResolving ? _targetHadExpose : ExposeHelper.Get(target) > 0;

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        _targetHadExpose = ExposeHelper.Get(cardPlay.Target) > 0;
        _isResolving = true;
        try
        {
            await DamageCmd.Attack(DynamicVars.CalculatedDamage)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(context);
        }
        finally
        {
            _isResolving = false;
        }
    }

    protected override void OnUpgrade() => DynamicVars.CalculationBase.UpgradeValueBy(3);
}
