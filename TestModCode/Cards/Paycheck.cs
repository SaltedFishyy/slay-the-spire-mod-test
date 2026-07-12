using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class Paycheck : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Gold", 15)];
    public Paycheck() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self) { }
    public override List<(string, string)>? Localization =>
        new CardLoc("PAYCHECK", "Whenever an enemy dies, gain {Gold} Gold.");
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay) =>
        await PowerCmd.Apply<PaycheckPower>(context, Owner.Creature, DynamicVars["Gold"].IntValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["Gold"].UpgradeValueBy(5);
}
