using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TestMod.TestModCode.Character;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class RedirectQuestion : LawyerCard
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/RedirectQuestion.png";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Weak", 1),
        new DynamicVar("Expose", 2),
        new DynamicVar("Energy", 1)
    ];

    public RedirectQuestion()
        : base(1, CardType.Skill, CardRarity.Uncommon, LawyerTargetTypes.AnyWeakEnemy)
    {
        LawyerTargetTypes.EnsureRegistered();
    }

    protected override PileType GetResultPileTypeForCardPlay() => PileType.Exhaust;

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Redirect Question",
            "Can only be played on an enemy with Weak. Remove {Weak} Weak. Apply {Expose} Expose. Gain {Energy} Energy. Exhaust.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        WeakPower? weak = cardPlay.Target.GetPower<WeakPower>();
        if (weak is not null && weak.Amount > 0)
        {
            await PowerCmd.ModifyAmount(
                context,
                weak,
                -Math.Min(DynamicVars["Weak"].IntValue, weak.Amount),
                Owner.Creature,
                this);
        }

        await ExposeHelper.Apply(
            context,
            cardPlay.Target,
            DynamicVars["Expose"].IntValue,
            Owner.Creature,
            this);
        await PlayerCmd.GainEnergy(DynamicVars["Energy"].IntValue, Owner);
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
