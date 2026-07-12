using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

// 稀有攻击：额外主动支付 Evidence，并触发 spend tracker。
public sealed class GuiltyAsCharged : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(44, ValueProp.Move),
        new DynamicVar("EvidenceCost", 18)
    ];

    protected override bool IsPlayable
    {
        get
        {
            if (!base.IsPlayable)
            {
                return false;
            }

            return Owner?.Creature is not { } creature ||
                   EvidenceHelper.Get(creature) >= DynamicVars["EvidenceCost"].IntValue;
        }
    }

    public GuiltyAsCharged() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }

    public override List<(string, string)>? Localization =>
        new CardLoc("Guilty as Charged", "Spend {EvidenceCost} Evidence. Deal {Damage} damage.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        bool paid = await EvidenceHelper.Spend(context, Owner.Creature, DynamicVars["EvidenceCost"].IntValue, this);
        if (!paid) return;
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).Execute(context);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
        DynamicVars["EvidenceCost"].UpgradeValueBy(-3);
        DynamicVars.Damage.UpgradeValueBy(6);
    }
}
