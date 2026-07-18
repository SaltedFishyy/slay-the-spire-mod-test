using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

// Phase 1 技能测试卡：验证“消耗一张牌，然后获得 Evidence”的流程。
public sealed class CollectingEvidence : LawyerCard
{
    public override string? CustomPortraitPath =>
    "res://Resources/Images/Cards/CollectingEvidence.png";


    public CollectingEvidence()
        : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Evidence", 3)
    ];

    public override List<(string, string)>? Localization =>
        new CardLoc("Collecting Evidence", "Exhaust a card. Gain {Evidence} Evidence.");

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 打开原版手牌选择界面，并禁止选择正在结算的这张卡本身。
        CardModel? selectedCard = (await CardSelectCmd.FromHand(
                choiceContext,
                Owner,
                new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1),
                card => card != this,
                this))
            .FirstOrDefault();

        if (selectedCard is not null)
        {
            // 有其他手牌时，先 Exhaust 选中的牌。
            await CardCmd.Exhaust(choiceContext, selectedCard);
        }

        // 然后获得 Evidence。
        await EvidenceHelper.Gain(
            choiceContext,
            Owner.Creature,
            DynamicVars["Evidence"].IntValue,
            this);
    }

    protected override void OnUpgrade() => DynamicVars["Evidence"].UpgradeValueBy(2);
}
