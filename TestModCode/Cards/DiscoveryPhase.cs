using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class DiscoveryPhase : LawyerCard
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/DiscoveryPhase.png";
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Evidence", 20)];
    public DiscoveryPhase() : base(3, CardType.Skill, CardRarity.Rare, TargetType.Self) { }
    public override List<(string, string)>? Localization => new CardLoc("Discovery Phase", "Gain {Evidence} Evidence.");
    protected override Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay) =>
        EvidenceHelper.Gain(context, Owner.Creature, DynamicVars["Evidence"].IntValue, this);
    protected override void OnUpgrade() => DynamicVars["Evidence"].UpgradeValueBy(10);
}
