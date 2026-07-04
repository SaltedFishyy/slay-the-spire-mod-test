using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace TestMod.TestModCode.Cards;

[Pool(typeof(TokenCardPool))]
public sealed class DebugWinCard : CustomCardModel
{
    public DebugWinCard()
        : base(0, CardType.Skill, CardRarity.Token, TargetType.None, showInCardLibrary: false)
    {
    }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Debug Win",
            "Lose 20 HP. Win this combat.");

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Owner.Creature.LoseHpInternal(20, ValueProp.Unblockable);

        if (CombatState is not { } combatState)
        {
            return;
        }

        await CreatureCmd.Kill(combatState.Enemies, true);
    }
}
