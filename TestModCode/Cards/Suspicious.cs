using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class Suspicious : LawyerCard
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/Suspicious.png";
    public Suspicious() : base(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy) { }
    protected override PileType GetResultPileTypeForCardPlay() => IsUpgraded ? PileType.Discard : PileType.Exhaust;
    public override List<(string, string)>? Localization =>
        new CardLoc("Suspicious o.O", "Apply Expose equal to the target's Weak.{IfUpgraded:show:| Exhaust.}");
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        int weak = Math.Max(0, cardPlay.Target.GetPower<WeakPower>()?.Amount ?? 0);
        await ExposeHelper.Apply(context, cardPlay.Target, weak, Owner.Creature, this);
    }
    protected override void OnUpgrade() { }
}
