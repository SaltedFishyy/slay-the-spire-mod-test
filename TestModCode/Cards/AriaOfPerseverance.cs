using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class AriaOfPerseverance : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Weak", 1),
        new DynamicVar("Vulnerable", 1)
    ];

    public AriaOfPerseverance() : base(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy) { }

    public override List<(string, string)>? Localization =>
        new CardLoc("Aria of Perseverance", "Apply {Weak} Weak and {Vulnerable} Vulnerable.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await PowerCmd.Apply<WeakPower>(
            context,
            cardPlay.Target,
            DynamicVars["Weak"].IntValue,
            Owner.Creature,
            this);
        await PowerCmd.Apply<VulnerablePower>(
            context,
            cardPlay.Target,
            DynamicVars["Vulnerable"].IntValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Weak"].UpgradeValueBy(1);
        DynamicVars["Vulnerable"].UpgradeValueBy(1);
    }
}
