using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace TestMod.TestModCode.Cards;

public sealed class ThatsLegal : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(5, ValueProp.Move),
        new DynamicVar("Draw", 2)
    ];

    public ThatsLegal() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "That's Legal",
            "Deal {Damage:diff()} damage. If you have not played an Attack this turn, draw {Draw} cards.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        bool isFirstAttack = CombatState is { } combatState &&
            !CombatManager.Instance.History.CardPlaysFinished.Any(entry =>
                entry.HappenedThisTurn(combatState) &&
                entry.CardPlay.Card.Owner == Owner &&
                entry.CardPlay.Card.Type == CardType.Attack);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(context);

        if (isFirstAttack)
            await CardPileCmd.Draw(context, DynamicVars["Draw"].IntValue, Owner);
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3);
}
