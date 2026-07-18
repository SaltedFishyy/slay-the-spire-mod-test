using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class IRememberThis : LawyerCard
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/IRememberThis.png";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(13, ValueProp.Move),
        new DynamicVar("Evidence", 8)
    ];

    public IRememberThis() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

    public override List<(string, string)>? Localization =>
        new CardLoc("I Remember This", "Gain {Block:diff()} Block. Gain {Evidence} Evidence.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);
        await EvidenceHelper.Gain(context, Owner.Creature, DynamicVars["Evidence"].IntValue, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);
        DynamicVars["Evidence"].UpgradeValueBy(2);
    }
}
