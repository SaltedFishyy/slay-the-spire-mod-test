using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class Done : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("ExposePerCard", 1)];

    protected override PileType GetResultPileTypeForCardPlay() => PileType.Exhaust;

    public Done() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies) { }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "DONE",
            "Exhaust your hand. Apply {ExposePerCard} Expose to ALL enemies for each card Exhausted. Exhaust.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        List<CardModel> handSnapshot = Owner.PlayerCombatState?.Hand?.Cards
            .Where(card => card != this)
            .ToList() ?? [];

        int exhaustedCount = 0;
        foreach (CardModel card in handSnapshot)
        {
            await CardCmd.Exhaust(context, card);
            if (card.Pile?.Type == PileType.Exhaust)
                exhaustedCount++;
        }

        int exposeAmount = exhaustedCount * DynamicVars["ExposePerCard"].IntValue;
        if (exposeAmount <= 0)
            return;

        foreach (Creature enemy in this.GetTargets().Where(creature => creature.IsAlive))
            await ExposeHelper.Apply(context, enemy, exposeAmount, Owner.Creature, this);
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
