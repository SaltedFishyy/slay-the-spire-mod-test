using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TestMod.TestModCode.Powers;

public sealed class CourtroomControlPower : CustomPowerModel
{
    private bool _triggeredThisTurn;
    private int _trackedTurnNumber = -1;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "res://Resources/Images/Power/CourtroomControlPower.png";
    public override string CustomBigIconPath => "res://Resources/Images/Power/CourtroomControlPower.png";
    public override string CustomBigBetaIconPath => "res://Resources/Images/Power/CourtroomControlPower.png";
    public override List<(string, string)>? Localization =>
        new PowerLoc("Courtroom Control", "The first time you spend Evidence each turn, draw {Amount} card.", "The first time you spend Evidence each turn, draw {Amount} card.");

    public override Task AfterPlayerTurnStart(PlayerChoiceContext context, Player player)
    {
        if (Owner.Player == player)
        {
            _trackedTurnNumber = player.PlayerCombatState?.TurnNumber ?? -1;
            _triggeredThisTurn = false;
        }

        return Task.CompletedTask;
    }

    public async Task AfterEvidenceSpent(PlayerChoiceContext context)
    {
        if (Owner.Player is not { } player || Amount <= 0)
            return;

        int currentTurn = player.PlayerCombatState?.TurnNumber ?? -1;
        if (currentTurn != _trackedTurnNumber)
        {
            _trackedTurnNumber = currentTurn;
            _triggeredThisTurn = false;
        }

        if (_triggeredThisTurn)
            return;

        _triggeredThisTurn = true;
        Flash();
        await CardPileCmd.Draw(context, Amount, player);
    }
}
