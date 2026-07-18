using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class CourtroomSpotlight : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("BonusDamage", 3)];

    public CourtroomSpotlight() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self) { }

    public override List<(string, string)>? Localization =>
        new CardLoc("Courtroom Spotlight", "Expose deals {BonusDamage} additional damage.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay) =>
        await PowerCmd.Apply<CourtroomSpotlightPower>(
            context,
            Owner.Creature,
            DynamicVars["BonusDamage"].IntValue,
            Owner.Creature,
            this);

    protected override void OnUpgrade() => DynamicVars["BonusDamage"].UpgradeValueBy(2);
}
