using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TestMod.TestModCode.Cards;

public sealed class IQuestionYou : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(2, ValueProp.Move),
        new DynamicVar("Weak", 1)
    ];

    protected override PileType GetResultPileTypeForCardPlay() => PileType.Exhaust;

    public IQuestionYou() : base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

    public override List<(string, string)>? Localization =>
        new CardLoc("I QUESTION YOU", "Deal {Damage:diff()} damage. Apply {Weak} Weak. Exhaust.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(context);
        await PowerCmd.Apply<WeakPower>(
            context,
            cardPlay.Target,
            DynamicVars["Weak"].IntValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2);
}
