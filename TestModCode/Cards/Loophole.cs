using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class Loophole : LawyerCard
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/Loophole.png";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [new DynamicVar("EvidenceCost", 5), new DynamicVar("Energy", 2)];
    protected override bool IsPlayable => base.IsPlayable &&
        (Owner?.Creature is not { } creature || EvidenceHelper.Get(creature) >= DynamicVars["EvidenceCost"].IntValue);
    public Loophole() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }
    public override List<(string, string)>? Localization =>
        new CardLoc("Loophole", "Spend {EvidenceCost} Evidence. Gain {Energy} Energy.");
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (!await EvidenceHelper.Spend(context, Owner.Creature, DynamicVars["EvidenceCost"].IntValue, this)) return;
        await PlayerCmd.GainEnergy(DynamicVars["Energy"].IntValue, Owner);
    }
    protected override void OnUpgrade() => DynamicVars["EvidenceCost"].UpgradeValueBy(-2);
}
