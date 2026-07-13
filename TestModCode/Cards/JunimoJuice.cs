using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class JunimoJuice : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("MaxEnergy", 1)];

    public JunimoJuice() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self) { }

    protected override PileType GetResultPileTypeForCardPlay() =>
        IsUpgraded ? PileType.Discard : PileType.Exhaust;

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Junimo Juice",
            "Increase your max Energy by {MaxEnergy} this combat.{IfUpgraded:show:| Exhaust.}");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay) =>
        await PowerCmd.Apply<JunimoJuicePower>(
            context,
            Owner.Creature,
            DynamicVars["MaxEnergy"].IntValue,
            Owner.Creature,
            this);

    protected override void OnUpgrade() { }
}
