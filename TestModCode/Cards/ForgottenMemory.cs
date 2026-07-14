using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace TestMod.TestModCode.Cards;

public sealed class ForgottenMemory : LawyerCard
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/ForgottenMemory.png";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new BlockVar(10, ValueProp.Move)];

    public ForgottenMemory() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self) { }

    protected override PileType GetResultPileTypeForCardPlay() => PileType.Exhaust;

    public override List<(string, string)>? Localization =>
        new CardLoc("Forgotten Memory", "Gain {Block:diff()} Block. Exhaust.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay) =>
        await CommonActions.CardBlock(this, cardPlay);

    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3);
}
