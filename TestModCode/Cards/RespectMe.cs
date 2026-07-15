using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TestMod.TestModCode.Cards;

public sealed class RespectMe : LawyerCard
{
    protected override bool HasEnergyCostX => true;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(3, ValueProp.Move),
        new DynamicVar("HitsPerEnergy", 2),
        new DynamicVar("WeakPerEnergy", 1)
    ];

    public RespectMe() : base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Respect Me!",
            "For each Energy spent, deal {Damage:diff()} damage {HitsPerEnergy} times and apply {WeakPerEnergy} Weak.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        int x = Math.Max(0, ResolveEnergyXValue());
        if (x == 0)
            return;

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount(DynamicVars["HitsPerEnergy"].IntValue * x)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(context);

        await PowerCmd.Apply<WeakPower>(
            context,
            cardPlay.Target,
            DynamicVars["WeakPerEnergy"].IntValue * x,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade() => DynamicVars["WeakPerEnergy"].UpgradeValueBy(1);
}
