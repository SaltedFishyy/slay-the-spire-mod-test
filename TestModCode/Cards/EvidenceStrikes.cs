using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

// 普通攻击：造成固定伤害，并失去2点证据。
public sealed class EvidenceStrikes : LawyerCard
{
    private const int EvidenceCost = 2;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(12, ValueProp.Move)];

    protected override bool IsPlayable =>
        base.IsPlayable &&
        (Owner?.Creature is not { } creature || EvidenceHelper.Get(creature) >= EvidenceCost);

    public EvidenceStrikes() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

    public override List<(string, string)>? Localization =>
        new CardLoc("Evidence Strikes!", "Spend 2 Evidence. Deal {Damage} damage.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        bool paid = await EvidenceHelper.Spend(context, Owner.Creature, EvidenceCost, this);
        if (!paid)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(context);
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3);
}
