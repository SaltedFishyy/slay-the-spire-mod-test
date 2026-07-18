using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class SoftPressure : LawyerCard
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/SoftPressure.png";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Weak", 1),
        new DynamicVar("Evidence", 3)
    ];

    public SoftPressure() : base(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy) { }

    protected override PileType GetResultPileTypeForCardPlay() =>
        IsUpgraded ? PileType.Discard : PileType.Exhaust;

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Soft Pressure",
            "Apply {Weak} Weak. If the enemy already had Weak, gain {Evidence} Evidence.{IfUpgraded:show:| Exhaust.}");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        bool hadWeak = (cardPlay.Target.GetPower<WeakPower>()?.Amount ?? 0) > 0;

        await PowerCmd.Apply<WeakPower>(
            context,
            cardPlay.Target,
            DynamicVars["Weak"].IntValue,
            Owner.Creature,
            this);

        if (hadWeak)
        {
            await EvidenceHelper.Gain(
                context,
                Owner.Creature,
                DynamicVars["Evidence"].IntValue,
                this);
        }
    }

    protected override void OnUpgrade() { }
}
