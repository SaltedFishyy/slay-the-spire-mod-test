using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class NewClientKenny : LawyerCard
{
    public NewClientKenny() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self) { }
    public override List<(string, string)>? Localization =>
        new CardLoc("New Client: Kenny", "Whenever you play a card, gain 1 Evidence.");
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay) =>
        await PowerCmd.Apply<KennyPower>(context, Owner.Creature, 1, Owner.Creature, this);
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
