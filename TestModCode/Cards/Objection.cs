using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class Objection : LawyerCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(4, ValueProp.Move),
        new DynamicVar("EvidenceCost", 3)
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

    public Objection() : base(0, CardType.Skill, CardRarity.Rare, TargetType.AnyPlayer) { }

    protected override PileType GetResultPileTypeForCardPlay() => PileType.Hand;

    public override List<(string, string)>? Localization =>
        new CardLoc("OBJECTION!", "Spend {EvidenceCost} Evidence. Give any player {Block:diff()} Block. Return this card to your hand.");

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (Owner?.Creature is not { } owner)
            return;

        var target = cardPlay.Target ?? owner;
        bool paid = await EvidenceHelper.Spend(context, Owner.Creature, DynamicVars["EvidenceCost"].IntValue, this);
        if (!paid)
        {
            return;
        }
        await CreatureCmd.GainBlock(target, DynamicVars.Block, cardPlay);
    }

    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(2);
}
