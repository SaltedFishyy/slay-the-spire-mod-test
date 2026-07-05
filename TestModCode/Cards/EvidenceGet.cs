using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

// 普通攻击牌占位实现：造成伤害后获得 Evidence。
public sealed class EvidenceGet : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8, ValueProp.Move),
        new DynamicVar("Evidence", 3)
    ];

    public EvidenceGet()
        : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

    public override List<(string, string)>? Localization =>
        new CardLoc("Evidence Get!", "Deal {Damage} damage. Gain {Evidence} Evidence.");

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        await PowerCmd.Apply<EvidencePower>(
            choiceContext,
            Owner.Creature,
            DynamicVars["Evidence"].IntValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        // 升级后伤害从 8 提高到 10，获得的 Evidence 从 3 提高到 4。
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars["Evidence"].UpgradeValueBy(1);
    }
}
