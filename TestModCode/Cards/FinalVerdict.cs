using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

// 稀有攻击：失去全部 Evidence，并造成失去数量两倍的伤害。
public sealed class FinalVerdict : LawyerCard
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/FinalVerdict.png";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Multiplier", 2)
    ];

    public FinalVerdict() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }

    public override List<(string, string)>? Localization =>
        new CardLoc("Final Verdict", "Lose all Evidence. Deal damage equal to twice the Evidence lost.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        int lostEvidence = EvidenceHelper.Get(Owner.Creature);
        await EvidenceHelper.Lose(context, Owner.Creature, lostEvidence, this);
        await DamageCmd.Attack(lostEvidence * DynamicVars["Multiplier"].IntValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(context);
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
