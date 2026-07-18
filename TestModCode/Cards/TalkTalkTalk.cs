using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class TalkTalkTalk : LawyerCard, IExposeTriggerObserver
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/TalkTalkTalk.png";

    private bool _isResolving;
    private int _exposeTriggers;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(4, ValueProp.Move),
        new RepeatVar(3),
        new BlockVar(2, ValueProp.Move)
    ];

    public TalkTalkTalk() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Talk Talk Talk",
            "Deal {Damage:diff()} damage {Repeat} times. Gain {Block:diff()} Block for each Expose triggered.");

    public void OnExposeTriggered(Creature target)
    {
        if (_isResolving)
            _exposeTriggers++;
    }

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        _exposeTriggers = 0;
        _isResolving = true;
        try
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(DynamicVars.Repeat.IntValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(context);
        }
        finally
        {
            _isResolving = false;
        }

        for (int i = 0; i < _exposeTriggers; i++)
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
        DynamicVars.Block.UpgradeValueBy(1);
    }
}
