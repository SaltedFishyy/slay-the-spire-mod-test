using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace TestMod.TestModCode.Cards;

// 每次主动 Spend Evidence 都会让这张全体攻击在本场战斗中降低 3 点费用。
public sealed class ClassAction : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(20, ValueProp.Move)];

    public ClassAction()
        : base(9, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
    }

    public override List<(string, string)>? Localization =>
        new CardLoc("Class Action", "Whenever you gain Evidence this combat, this card costs 2 less. Deal {Damage:diff()} damage to ALL enemies.");

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is not { } combatState)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(combatState)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        // 升级只把全体伤害从 20 提高到 26。
        DynamicVars.Damage.UpgradeValueBy(6);
    }
}
