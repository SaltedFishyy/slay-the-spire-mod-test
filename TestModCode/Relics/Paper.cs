using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TestMod.TestModCode.Character;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Relics;




// Lawyer 的 Phase 1 起始遗物，注册在 Lawyer 专属遗物池中。
[Pool(typeof(LawyerRelicPool))]
public sealed class Papers : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override string PackedIconPath => "res://Resources/Images/Relics/Papers.png";
    protected override string PackedIconOutlinePath => "res://Resources/Images/Relics/Papers.png";
    protected override string BigIconPath => "res://Resources/Images/Relics/Papers.png";

    public override List<(string, string)>? Localization =>
        new RelicLoc(
            "Papers",
            "At the start of combat, gain 3 Evidence.",
            "Every case begins with a stack of papers :)");

    public override async Task BeforeCombatStart()
    {
        // 战斗状态建立后、首回合开始前给予 3 层 Evidence。
        Flash();
        await EvidenceHelper.Gain(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            3,
            null);

        MainFile.Logger.Info("Papers granted 3 Evidence.");
    }
}
