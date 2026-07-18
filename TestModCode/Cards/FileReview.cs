using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class FileReview : LawyerCard
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/FileReview.png";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Cards", 3),
        new DynamicVar("Evidence", 2)
    ];

    public FileReview() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self) { }

    protected override PileType GetResultPileTypeForCardPlay() => PileType.Exhaust;

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "File Review",
            "Look at the top {Cards} cards of your draw pile. Choose 1 to discard. Put the rest into your hand. Gain {Evidence} Evidence. Exhaust.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        CardPile? drawPile = Owner.PlayerCombatState?.DrawPile;
        List<CardModel> topCards = drawPile?.Cards
            .Take(DynamicVars["Cards"].IntValue)
            .ToList() ?? [];

        if (topCards.Count > 0)
        {
            CardModel? selected = await CardSelectCmd.FromChooseACardScreen(
                context,
                topCards,
                Owner,
                false);

            if (selected is not null && drawPile?.Cards.Contains(selected) == true)
            {
                await CardCmd.Discard(context, selected);

                List<CardModel> remainingCards = topCards
                    .Where(card => !ReferenceEquals(card, selected) && drawPile.Cards.Contains(card))
                    .ToList();

                if (remainingCards.Count > 0)
                {
                    await CardPileCmd.Add(
                        remainingCards,
                        PileType.Hand,
                        CardPilePosition.Bottom,
                        this);
                }
            }
        }

        await EvidenceHelper.Gain(
            context,
            Owner.Creature,
            DynamicVars["Evidence"].IntValue,
            this);
    }

    protected override void OnUpgrade() => DynamicVars["Evidence"].UpgradeValueBy(2);
}
