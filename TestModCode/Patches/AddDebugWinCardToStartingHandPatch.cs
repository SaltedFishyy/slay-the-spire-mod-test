using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using TestMod.TestModCode.Cards;

namespace TestMod.TestModCode.Patches;

[HarmonyPatch(typeof(CombatManager), "SetupPlayerTurn")]
internal static class AddDebugWinCardToStartingHandPatch
{
    private static void Postfix(Player __0, ref Task __result)
    {
        MainFile.Logger.Info("Starting-hand patch ran.");
        __result = AddCardAfterInitialDraw(__result, __0);
    }

    private static async Task AddCardAfterInitialDraw(Task setupTask, Player player)
    {
        await setupTask;

        if (player.PlayerCombatState is not { } playerCombatState)
        {
            MainFile.Logger.Info($"Player {player.NetId} has no combat state yet.");
            return;
        }

        MainFile.Logger.Info(
            $"Found combat hand for player {player.NetId} with {playerCombatState.Hand.Cards.Count} cards.");

        bool cardAlreadyAdded = playerCombatState.AllCards
            .Any(card => card is DebugWinCard);

        if (cardAlreadyAdded)
        {
            MainFile.Logger.Info($"Skipped duplicate DebugWinCard for player {player.NetId}.");
            return;
        }

        if (playerCombatState.TurnNumber != 1)
        {
            return;
        }

        if (player.Creature.CombatState is not CombatState combatState)
        {
            MainFile.Logger.Info($"Player {player.NetId} has no active combat.");
            return;
        }

        DebugWinCard card = combatState.CreateCard<DebugWinCard>(player);
        await CardPileCmd.Add(
            card,
            playerCombatState.Hand,
            CardPilePosition.Random,
            card,
            false);

        MainFile.Logger.Info($"Added DebugWinCard to player {player.NetId}'s hand.");
    }
}
