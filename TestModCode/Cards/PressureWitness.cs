using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class PressureWitness : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Weak", 2)];
    public PressureWitness() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy) { }
    protected override PileType GetResultPileTypeForCardPlay() => IsUpgraded ? PileType.Discard : PileType.Exhaust;
    public override List<(string, string)>? Localization =>
        new CardLoc("Pressure Witness", "Apply {Weak} Weak.{IfUpgraded:show:| Exhaust.}");
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await PowerCmd.Apply<WeakPower>(context, cardPlay.Target, DynamicVars["Weak"].IntValue, Owner.Creature, this);
    }
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
