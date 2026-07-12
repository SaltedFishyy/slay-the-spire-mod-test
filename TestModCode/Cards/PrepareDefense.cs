using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class PrepareDefense : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Evidence", 3)];
    public PrepareDefense() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }
    public override List<(string, string)>? Localization =>
        new CardLoc("Prepare Defense", "Discard all Attack cards in your hand. Draw that many cards. Gain {Evidence} Evidence.");
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        List<CardModel> attacks = Owner.PlayerCombatState?.Hand?.Cards
            .Where(card => card.Type == CardType.Attack)
            .ToList() ?? [];
        if (attacks.Count > 0)
        {
            await CardCmd.Discard(context, attacks);
            await CardPileCmd.Draw(context, attacks.Count, Owner);
        }
        await EvidenceHelper.Gain(context, Owner.Creature, DynamicVars["Evidence"].IntValue, this);
    }
    protected override void OnUpgrade() => DynamicVars["Evidence"].UpgradeValueBy(2);
}
