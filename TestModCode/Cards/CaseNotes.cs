using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class CaseNotes : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(3, ValueProp.Move),
        new DynamicVar("Evidence", 4)
    ];

    public CaseNotes() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }

    public override List<(string, string)>? Localization =>
        new CardLoc("Case Notes", "Gain {Block} Block. Gain {Evidence} Evidence. Choose 1 card in your hand to Retain.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);
        await EvidenceHelper.Gain(context, Owner.Creature, DynamicVars["Evidence"].IntValue, this);

        CardModel? selectedCard = (await CardSelectCmd.FromHand(
                context,
                Owner,
                new CardSelectorPrefs(TitleLocString, 1),
                card => card != this,
                this))
            .FirstOrDefault();

        if (selectedCard is not null)
        {
            selectedCard.GiveSingleTurnRetain();
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
        DynamicVars["Evidence"].UpgradeValueBy(2);
    }
}
