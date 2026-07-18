using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class BigBrain : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("Evidence", 3)];

    public BigBrain() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self) { }

    public override List<(string, string)>? Localization =>
        new CardLoc("Big Brain", "Whenever Expose triggers, gain {Evidence} Evidence.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay) =>
        await PowerCmd.Apply<BigBrainPower>(
            context,
            Owner.Creature,
            DynamicVars["Evidence"].IntValue,
            Owner.Creature,
            this);

    protected override void OnUpgrade() => DynamicVars["Evidence"].UpgradeValueBy(1);
}
