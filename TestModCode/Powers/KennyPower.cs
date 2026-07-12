using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TestMod.TestModCode.Cards;

namespace TestMod.TestModCode.Powers;

public sealed class KennyPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigBetaIconPath => "res://Resources/Images/UI/Evidence.png";
    public override List<(string, string)>? Localization =>
        new PowerLoc("New Client: Kenny", "Whenever you play a card, gain {Amount} Evidence.", "Whenever you play a card, gain {Amount} Evidence.");
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card is NewClientKenny || cardPlay.Card.Owner?.Creature != Owner) return;
        await EvidenceHelper.Gain(context, Owner, Amount, cardPlay.Card);
    }
}
