using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class NewClientKenny : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("EvidenceThreshold", 7)];

    public NewClientKenny() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self) { }
    public override List<(string, string)>? Localization =>
        new CardLoc("New Client: Kenny", "Whenever you have gained a total of {EvidenceThreshold} Evidence, draw 1 card.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        KennyPower? power = await PowerCmd.Apply<KennyPower>(
            context,
            Owner.Creature,
            1,
            Owner.Creature,
            this);
        power?.AddTracker(DynamicVars["EvidenceThreshold"].IntValue);
    }

    protected override void OnUpgrade() => DynamicVars["EvidenceThreshold"].UpgradeValueBy(-2);
}
