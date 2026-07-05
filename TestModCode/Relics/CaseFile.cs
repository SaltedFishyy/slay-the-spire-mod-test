using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TestMod.TestModCode.Character;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Relics;

// Lawyer 的 Phase 1 起始遗物，注册在 Lawyer 专属遗物池中。
[Pool(typeof(LawyerRelicPool))]
public sealed class CaseFile : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override List<(string, string)>? Localization =>
        new RelicLoc(
            "Case File",
            "At the start of combat, gain 3 Evidence.",
            "Every case begins with a file.");

    public override async Task BeforeCombatStart()
    {
        // 战斗状态建立后、首回合开始前给予 3 层 Evidence。
        Flash();
        await PowerCmd.Apply<EvidencePower>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            3,
            Owner.Creature,
            null);

        MainFile.Logger.Info("CaseFile granted 3 Evidence.");
    }
}
