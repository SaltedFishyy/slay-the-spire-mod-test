using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

// Phase 1 攻击测试卡：伤害会读取打出时的当前 Evidence。
public sealed class Statement : LawyerCard
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/Statement.png";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(6, ValueProp.Move),
        ..MakeCalculatedVar(
            "EvidenceDamage",
            0,
            (card, _) => ((Statement)card).GetEvidenceDamage())
    ];

    public Statement()
        : base(2, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
    }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Statement",
            "{IfUpgraded:show:Deal {EvidenceDamage} damage from Evidence.|Deal {Damage} damage + {EvidenceDamage} damage from Evidence.}");

    private int GetEvidenceDamage()
    {
        int evidence = EvidenceHelper.Get(Owner?.Creature);
        return IsUpgraded ? (int)Math.Floor(evidence * 1.5m) : evidence;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        // 没有 EvidencePower 时按 0 层处理。
        // 这张牌只读取 Evidence 计算伤害，不会消耗或减少 Evidence。
        decimal totalDamage = DynamicVars.Damage.BaseValue + GetEvidenceDamage();

        await DamageCmd.Attack(totalDamage)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
        DynamicVars.Damage.UpgradeValueBy(2);
    }
}
