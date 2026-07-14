using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TestMod.TestModCode.Character;

namespace TestMod.TestModCode.Cards;

public sealed class PocketCart : LawyerCard
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/PocketCart.png";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("Cards", 1)];

    public PocketCart() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

    protected override PileType GetResultPileTypeForCardPlay() => PileType.Exhaust;

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Pocket C.A.R.T.",
            "Add {Cards} random Lawyer cards to your hand. They cost 0 this turn. Exhaust.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        int count = DynamicVars["Cards"].IntValue;
        if (count <= 0)
        {
            return;
        }

        IEnumerable<CardModel> candidates = ModelDb.CardPool<LawyerCardPool>()
            .AllCards
            .Where(card => card is not PocketCart && card.CanBeGeneratedInCombat);

        List<CardModel> generated = CardFactory.GetDistinctForCombat(
                Owner,
                candidates,
                count,
                Owner.RunState.Rng.CombatCardGeneration)
            .ToList();

        if (generated.Count == 0)
        {
            return;
        }

        foreach (CardModel card in generated)
        {
            card.EnergyCost.SetThisTurnOrUntilPlayed(0, false);
        }

        await CardPileCmd.AddGeneratedCardsToCombat(generated, PileType.Hand, Owner);
    }

    protected override void OnUpgrade() => DynamicVars["Cards"].UpgradeValueBy(1);
}
