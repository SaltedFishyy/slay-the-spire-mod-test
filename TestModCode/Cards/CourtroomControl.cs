using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class CourtroomControl : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("BonusEvidence", 3)];
    public CourtroomControl() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self) { }
    public override List<(string, string)>? Localization =>
        new CardLoc("Courtroom Control", "Whenever you gain Evidence, gain {BonusEvidence} additional Evidence.");
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay) =>
        await PowerCmd.Apply<CourtroomControlPower>(context, Owner.Creature, DynamicVars["BonusEvidence"].IntValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["BonusEvidence"].UpgradeValueBy(2);
}
