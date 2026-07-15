using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class TheDuckBucket : LawyerCard
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/TheDuckBucket.png";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("Intangible", 1)];

    public TheDuckBucket() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self) { }

    protected override PileType GetResultPileTypeForCardPlay() => PileType.Exhaust;

    public override List<(string, string)>? Localization =>
        new CardLoc("The Duck Bucket", "Gain {Intangible} Intangible. Exhaust.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay) =>
        await PowerCmd.Apply<IntangiblePower>(
            context,
            Owner.Creature,
            DynamicVars["Intangible"].IntValue,
            Owner.Creature,
            this);

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
