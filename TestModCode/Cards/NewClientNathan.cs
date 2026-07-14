using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class NewClientNathan : LawyerCard
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/NewClientNathan.png";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("BonusEvidence", 2)];

    public NewClientNathan() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self) { }
    public override List<(string, string)>? Localization =>
        new CardLoc("New Client: Nathan", "Whenever you would gain Evidence, gain {BonusEvidence} more.");
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay) =>
        await PowerCmd.Apply<NathanPower>(context, Owner.Creature, DynamicVars["BonusEvidence"].IntValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["BonusEvidence"].UpgradeValueBy(2);
}
