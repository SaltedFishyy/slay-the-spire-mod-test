using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

// Evidence 较低时，在造成伤害后补充 Evidence。
public sealed class CaseBuild : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(13, ValueProp.Move),
        new DynamicVar("EvidenceThreshold", 10),
        new DynamicVar("EvidenceGain", 8)
    ];

    public CaseBuild()
        : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    public override List<(string, string)>? Localization =>
        new CardLoc("Case Build", "Deal {Damage} damage. If you have less than {EvidenceThreshold} Evidence, gain {EvidenceGain} Evidence.");

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        // 按需求在伤害结算后检查当前 Evidence。
        int currentEvidence = Owner.Creature.GetPower<EvidencePower>()?.Amount ?? 0;
        if (currentEvidence < DynamicVars["EvidenceThreshold"].IntValue)
        {
            await EvidenceHelper.Gain(
                choiceContext,
                Owner.Creature,
                DynamicVars["EvidenceGain"].IntValue,
                this);
        }
    }

    protected override void OnUpgrade()
    {
        // 升级同步提高伤害、触发阈值和 Evidence 获得量。
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars["EvidenceThreshold"].UpgradeValueBy(3);
        DynamicVars["EvidenceGain"].UpgradeValueBy(2);
    }
}
