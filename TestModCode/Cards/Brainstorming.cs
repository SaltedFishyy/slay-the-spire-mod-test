using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class Brainstorming : LawyerCard
{
    public Brainstorming() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }
    public override List<(string, string)>? Localization =>
        new CardLoc("Brainstorming", "Gain Block equal to half your Evidence. End your turn. Next turn, gain extra Energy equal to your base Energy.");
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        int block = EvidenceHelper.Get(Owner.Creature) / 2;
        await CreatureCmd.GainBlock(Owner.Creature, block, ValueProp.Move, cardPlay, false);
        await PowerCmd.Apply<EnergyNextTurnPower>(context, Owner.Creature, Owner.MaxEnergy, Owner.Creature, this);
        CombatManager.Instance.SetReadyToEndTurn(Owner, true, () => Task.CompletedTask);
    }
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
