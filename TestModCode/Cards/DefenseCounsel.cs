using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class DefenseCounsel : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(8, ValueProp.Move),
        new DynamicVar("Expose", 1)
    ];

    public DefenseCounsel() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies) { }

    public override List<(string, string)>? Localization =>
        new CardLoc("Defense Counsel", "Gain {Block} Block. Apply {Expose} Expose to ALL enemies.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(
            Owner.Creature,
            DynamicVars.Block.BaseValue,
            ValueProp.Move,
            cardPlay,
            false);

        foreach (Creature enemy in this.GetTargets())
        {
            await ExposeHelper.Apply(context, enemy, DynamicVars["Expose"].IntValue, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(2);
}
