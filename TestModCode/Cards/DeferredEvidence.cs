using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class DeferredEvidence : LawyerCard
{
    public DeferredEvidence() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }
    public override List<(string, string)>? Localization =>
        new CardLoc("Deferred Evidence", "Lose half your Evidence. Next turn, gain triple the Evidence lost.");
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        int lost = EvidenceHelper.Get(Owner.Creature) / 2;
        if (lost <= 0) return;
        await EvidenceHelper.Lose(context, Owner.Creature, lost, this);
        await PowerCmd.Apply<DeferredEvidencePower>(context, Owner.Creature, lost * 3, Owner.Creature, this);
    }
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
