using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class Mistrial : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("Multiplier", 2)];

    public Mistrial() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }

    protected override PileType GetResultPileTypeForCardPlay() => PileType.Exhaust;

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Mistrial",
            "Remove all Weak and Vulnerable from an enemy. Apply Expose equal to {Multiplier} times the total stacks removed. Exhaust.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        WeakPower? weak = cardPlay.Target.GetPower<WeakPower>();
        VulnerablePower? vulnerable = cardPlay.Target.GetPower<VulnerablePower>();
        int weakAmount = Math.Max(0, weak?.Amount ?? 0);
        int vulnerableAmount = Math.Max(0, vulnerable?.Amount ?? 0);
        int exposeAmount = (weakAmount + vulnerableAmount) * DynamicVars["Multiplier"].IntValue;

        if (weak is not null && weakAmount > 0)
        {
            await PowerCmd.ModifyAmount(context, weak, -weakAmount, Owner.Creature, this);
        }

        if (vulnerable is not null && vulnerableAmount > 0)
        {
            await PowerCmd.ModifyAmount(context, vulnerable, -vulnerableAmount, Owner.Creature, this);
        }

        if (exposeAmount > 0)
        {
            await ExposeHelper.Apply(
                context,
                cardPlay.Target,
                exposeAmount,
                Owner.Creature,
                this);
        }
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
