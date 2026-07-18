using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TestMod.TestModCode.Powers;

// # Expose：敌人每被友方 Attack 的一段伤害命中，就消耗 1 层并受到 10 点额外伤害。
public sealed class ExposePower : CustomPowerModel
{
    public const int BonusDamage = 10;

    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    // # 项目尚无 Expose.png；暂用已打包的 Evidence 图标作为安全占位，避免 NOPE/null texture。
    public override string CustomPackedIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigIconPath => "res://Resources/Images/UI/Evidence.png";
    public override string CustomBigBetaIconPath => "res://Resources/Images/UI/Evidence.png";

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Expose",
            "On Attack hit, lose 1 Expose and take 10 bonus damage. Can be triggered by allies.",
            "On Attack hit, lose 1 Expose and take 10 bonus damage. Can be triggered by allies.");

    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        // # 只接受玩家阵营的 Attack card hit；毒、Power 伤害和 0 伤害不会触发。
        if (target != Owner ||
            Owner.Side != CombatSide.Enemy ||
            dealer?.Side != CombatSide.Player ||
            cardSource?.Type != CardType.Attack ||
            !props.IsPoweredAttack() ||
            result.TotalDamage <= 0 ||
            Amount <= 0)
        {
            return;
        }

        Flash();
        if (!await ExposeHelper.Decrement(target))
        {
            return;
        }

        if (cardSource is IExposeTriggerObserver observer)
        {
            observer.OnExposeTriggered(target);
        }

        List<Creature> playerCreatures = target.CombatState?.PlayerCreatures.ToList() ?? [];
        foreach (Creature playerCreature in playerCreatures)
        {
            BigBrainPower? bigBrain = playerCreature.GetPower<BigBrainPower>();
            if (bigBrain is null)
                continue;

            await bigBrain.OnExposeTriggered(choiceContext);
        }

        if (!target.IsAlive)
        {
            return;
        }

        int spotlightBonus = playerCreatures
            .Select(creature => creature.GetPower<CourtroomSpotlightPower>()?.Amount ?? 0)
            .Sum();

        // # Unpowered 且没有 cardSource，确保这 10 点 bonus damage 不会再次触发 Expose。
        await CreatureCmd.Damage(
            choiceContext,
            target,
            BonusDamage + spotlightBonus,
            ValueProp.Unpowered | ValueProp.SkipHurtAnim,
            dealer,
            null);
    }
}
