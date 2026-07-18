using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

// Spend Evidence as an additional cost, then deal fixed damage and apply Weak.
public sealed class ClosingArgument : LawyerCard
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/ClosingArgument.png";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(10, ValueProp.Move),
        new DynamicVar("EvidenceCost", 3),
        new DynamicVar("Weak", 1)
    ];

    protected override bool IsPlayable =>
        base.IsPlayable &&
        (Owner?.Creature is not { } creature ||
         EvidenceHelper.Get(creature) >= DynamicVars["EvidenceCost"].IntValue);

    public ClosingArgument()
        : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    public override List<(string, string)>? Localization =>
        new CardLoc("Closing Argument", "Spend {EvidenceCost} Evidence. Deal {Damage:diff()} damage. Apply {Weak} Weak.");

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        bool paid = await EvidenceHelper.Spend(
            choiceContext,
            Owner.Creature,
            DynamicVars["EvidenceCost"].IntValue,
            this);
        if (!paid)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        await PowerCmd.Apply<WeakPower>(
            choiceContext,
            cardPlay.Target,
            DynamicVars["Weak"].IntValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars["Weak"].UpgradeValueBy(1);
    }
}
