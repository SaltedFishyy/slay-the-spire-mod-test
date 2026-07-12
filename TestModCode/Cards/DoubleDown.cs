using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

// X-cost attack: spend Evidence based on the actual X energy spent, then apply Expose.
public sealed class DoubleDown : LawyerCard
{
    protected override bool HasEnergyCostX => true;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("EvidencePerEnergy", 2),
        new DynamicVar("UpgradeBonusExpose", 0)
    ];

    protected override bool IsPlayable
    {
        get
        {
            int x = Math.Max(0, EnergyCost.GetAmountToSpend());
            int evidenceCost = DynamicVars["EvidencePerEnergy"].IntValue * x;
            return base.IsPlayable &&
                   (Owner?.Creature is not { } creature ||
                    EvidenceHelper.Get(creature) >= evidenceCost);
        }
    }

    public DoubleDown() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Double Down",
            "Spend {EvidencePerEnergy} Evidence for each Energy spent. Apply X{IfUpgraded:show:+1|} Expose.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        int x = Math.Max(0, cardPlay.Resources.EnergySpent);
        int evidenceCost = DynamicVars["EvidencePerEnergy"].IntValue * x;
        bool paid = await EvidenceHelper.Spend(context, Owner.Creature, evidenceCost, this);
        if (!paid)
        {
            return;
        }

        int exposeAmount = x + DynamicVars["UpgradeBonusExpose"].IntValue;
        if (exposeAmount <= 0)
        {
            return;
        }

        await ExposeHelper.Apply(
            context,
            cardPlay.Target,
            exposeAmount,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["UpgradeBonusExpose"].UpgradeValueBy(1);
    }
}
