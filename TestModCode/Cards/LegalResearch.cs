using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class LegalResearch : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [new DynamicVar("Draw", 1), new DynamicVar("Evidence", 2)];
    public LegalResearch() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self) { }
    public override List<(string, string)>? Localization =>
        new CardLoc("Legal Research", "Draw {Draw} card. Gain {Evidence} Evidence.");
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(context, DynamicVars["Draw"].IntValue, Owner);
        await EvidenceHelper.Gain(context, Owner.Creature, DynamicVars["Evidence"].IntValue, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["Draw"].UpgradeValueBy(1);
        DynamicVars["Evidence"].UpgradeValueBy(1);
    }
}
