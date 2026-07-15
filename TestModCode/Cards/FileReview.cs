using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class FileReview : LawyerCard
{
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
            "Look at the top {Cards} cards of your draw pile. Choose 1 to discard. Gain {Evidence} Evidence. Exhaust.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        List<CardModel> topCards = Owner.PlayerCombatState?.DrawPile?.Cards
            .Take(DynamicVars["Cards"].IntValue)
            .ToList() ?? [];

        if (topCards.Count > 0)
        {
            CardModel? selected = (await CardSelectCmd.FromSimpleGrid(
                    context,
                    topCards,
                    Owner,
                    new CardSelectorPrefs(TitleLocString, 1)))
                .FirstOrDefault();

            if (selected is not null &&
                Owner.PlayerCombatState?.DrawPile?.Cards.Contains(selected) == true)
            {
                await CardCmd.Discard(context, selected);
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
