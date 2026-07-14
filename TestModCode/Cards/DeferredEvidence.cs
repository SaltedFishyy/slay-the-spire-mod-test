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
        new CardLoc("Deferred Evidence", "Lose half your Evidence. At the start of your next turn, gain twice the amount lost.");
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        int amountToLose = EvidenceHelper.Get(Owner.Creature) / 2;
        int lost = await EvidenceHelper.Lose(context, Owner.Creature, amountToLose, this);
        if (lost <= 0)
            return;

        int returnAmount = lost * 2;
        DeferredEvidencePower? power = await PowerCmd.Apply<DeferredEvidencePower>(
            context,
            Owner.Creature,
            returnAmount,
            Owner.Creature,
            this);
        power?.AddPendingReturn(returnAmount);
    }
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
