using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class BurdenOfProof : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("EvidenceThreshold", 8)];

    public BurdenOfProof() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self) { }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Burden of Proof",
            "Your Attack card hits deal 1 additional damage for every {EvidenceThreshold} Evidence you have.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (IsUpgraded)
            await PowerCmd.Apply<UpgradedBurdenOfProofPower>(context, Owner.Creature, 1, Owner.Creature, this);
        else
            await PowerCmd.Apply<BurdenOfProofPower>(context, Owner.Creature, 1, Owner.Creature, this);
    }

    protected override void OnUpgrade() =>
        DynamicVars["EvidenceThreshold"].UpgradeValueBy(-3);
}
