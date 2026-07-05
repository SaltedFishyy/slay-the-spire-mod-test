using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TestMod.TestModCode.Character;

namespace TestMod.TestModCode.Powers;

// Evidence 使用标准 Power 表示，因此可以直接复用游戏现有的状态图标和层数显示。
public sealed class EvidencePower : CustomPowerModel
{
    // Counter 允许重复获得 Evidence 时把层数累加。
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Evidence",
            "You have {Amount} Evidence.",
            "You have {Amount} Evidence.");

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 双重检查角色和卡牌所有者，确保该惩罚不会影响其他角色。
        bool isLawyer = Owner.Player?.Character is LawyerCharacter;
        bool isOwnersAttack = cardPlay.Card.Owner == Owner.Player
            && cardPlay.Card.Type == CardType.Attack;

        if (!isLawyer || !isOwnersAttack)
        {
            return;
        }

        // Evidence 惩罚是每次攻击消耗 1/4 层 Evidence，向下取整。
        int evidenceLost = (int)(Amount / 4f);
        if (evidenceLost <= 0)
        {
            return;
        }

        // AfterCardPlayed 在卡牌自身效果完成后运行，所以先造成伤害，再扣 Evidence。
        await PowerCmd.ModifyAmount(
            choiceContext,
            this,
            -evidenceLost,
            Owner,
            cardPlay.Card);

        MainFile.Logger.Info($"Lawyer attack penalty: Evidence reduced by {evidenceLost} to {Amount}.");
    }
}
