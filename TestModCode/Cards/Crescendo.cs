using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TestMod.TestModCode.Cards;

public sealed class Crescendo : LawyerCard
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/Crescendo.png";
    public Crescendo() : base(3, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy) { }

    protected override PileType GetResultPileTypeForCardPlay() => PileType.Exhaust;

    public override List<(string, string)>? Localization =>
        new CardLoc("Crescendo", "Stun an enemy. Exhaust.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await CreatureCmd.Stun(cardPlay.Target, null);
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
