using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class CourtroomControl : LawyerCard
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/CourtroomControl.png";
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        IsUpgraded ? [CardKeyword.Retain] : [];

    public CourtroomControl() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self) { }
    public override List<(string, string)>? Localization =>
        new CardLoc("Courtroom Control", "{IfUpgraded:show:Retain. |}The first time you gain Evidence each turn, draw 1 card.");
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay) =>
        await PowerCmd.Apply<CourtroomControlPower>(context, Owner.Creature, 1, Owner.Creature, this);
    protected override void OnUpgrade() { }
}
