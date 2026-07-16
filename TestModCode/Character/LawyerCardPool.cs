using BaseLib.Abstracts;
using Godot;

namespace TestMod.TestModCode.Character;

// 定义 Lawyer 专属卡池，让 Lawyer 卡牌不会混入其他角色的卡池。
public sealed class LawyerCardPool : CustomCardPoolModel
{
    // 这是卡池的内部标识，不是玩家最终看到的角色名称。
    public override string Title => LawyerCharacter.CharacterId;
    public override string CardFrameMaterialPath => "card_frame_pink";
    public override Color DeckEntryCardColor => LawyerCharacter.Color;
    public override string BigEnergyIconPath =>
        "res://Resources/Images/UI/LawyerEnergy.png";
    public override string TextEnergyIconPath =>
        "res://Resources/Images/UI/LawyerEnergy.png";

    // Lawyer 是独立角色卡池，不属于无色卡池。
    public override bool IsColorless => false;
}
