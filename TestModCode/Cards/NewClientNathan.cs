using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class NewClientNathan : LawyerCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        IsUpgraded ? [] : [CardKeyword.Ethereal];
    public NewClientNathan() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self) { }
    public override List<(string, string)>? Localization =>
        new CardLoc("New Client: Nathan", "{IfUpgraded:show:|Ethereal. }Whenever you gain Evidence, gain 50% more.");
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay) =>
        await PowerCmd.Apply<NathanPower>(context, Owner.Creature, 1, Owner.Creature, this);
    protected override void OnUpgrade() { }
}
