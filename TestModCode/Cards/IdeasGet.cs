using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

// 普通攻击牌占位实现：造成伤害后获得 Evidence。
public sealed class IdeasGet : LawyerCard
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/IdeasGet.png";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8, ValueProp.Move),
        new DynamicVar("Evidence", 4)
    ];

    public IdeasGet()
        : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

    public override List<(string, string)>? Localization =>
        new CardLoc("Ideas Get!", "Deal {Damage} damage. Gain {Evidence} Evidence.");

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        await EvidenceHelper.Gain(
            choiceContext,
            Owner.Creature,
            DynamicVars["Evidence"].IntValue,
            this);
    }

    protected override void OnUpgrade()
    {
        // 升级后伤害从 8 提高到 10，获得的 Evidence 从 4 提高到 5。
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars["Evidence"].UpgradeValueBy(1);
    }
}
