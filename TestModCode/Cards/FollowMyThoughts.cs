using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class FollowMyThoughts : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(6, ValueProp.Move),
        new DynamicVar("Draw", 1)
    ];

    public FollowMyThoughts() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Follow My Thoughts",
            "Deal {Damage:diff()} damage. If you gained Evidence this turn, draw {Draw} card(s).");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(context);

        EvidenceGainTrackerPower? tracker = Owner.Creature.GetPower<EvidenceGainTrackerPower>();
        if (tracker?.EvidenceGainedThisTurn > 0)
            await CardPileCmd.Draw(context, DynamicVars["Draw"].IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars["Draw"].UpgradeValueBy(1);
    }
}
