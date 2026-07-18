using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class TurnTheTable : LawyerCard
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/TurnTheTable.png";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Weak", 1),
        new DynamicVar("Expose", 1),
        new DynamicVar("Evidence", 5)
    ];

    public TurnTheTable() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Turn the Table",
            "If the enemy intends to attack, apply {Weak} Weak and {Expose} Expose. Otherwise, gain {Evidence} Evidence.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        bool intendsToAttack = cardPlay.Target.Monster?.IntendsToAttack == true;

        if (intendsToAttack)
        {
            await PowerCmd.Apply<WeakPower>(
                context,
                cardPlay.Target,
                DynamicVars["Weak"].IntValue,
                Owner.Creature,
                this);
            await ExposeHelper.Apply(
                context,
                cardPlay.Target,
                DynamicVars["Expose"].IntValue,
                Owner.Creature,
                this);
            return;
        }

        await EvidenceHelper.Gain(
            context,
            Owner.Creature,
            DynamicVars["Evidence"].IntValue,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Expose"].UpgradeValueBy(1);
        DynamicVars["Evidence"].UpgradeValueBy(2);
    }
}
