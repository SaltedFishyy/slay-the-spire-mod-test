using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class JointDefense : LawyerCard
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/JointDefense.png";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(7, ValueProp.Move),
        new DynamicVar("Evidence", 3)
    ];

    public JointDefense() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies) { }

    public override List<(string, string)>? Localization =>
        new CardLoc("Joint Defense", "All allies gain {Block:diff()} Block. Gain {Evidence} Evidence.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);
        await EvidenceHelper.Gain(context, Owner.Creature, DynamicVars["Evidence"].IntValue, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
        DynamicVars.Block.UpgradeValueBy(3);
        DynamicVars["Evidence"].UpgradeValueBy(2);
    }
}
