using BaseLib.Abstracts;
using Godot;

namespace TestMod.TestModCode.Character;

// 定义 Lawyer 专属卡池，让 Lawyer 卡牌不会混入其他角色的卡池。
public sealed class LawyerCardPool : CustomCardPoolModel
{
    // Godot 从模组 PCK 内读取裁剪并放大的 Lawyer 费用图标。
    private const string LawyerEnergyIconPath = "res://Resources/Images/UI/LawyerEnergyIcon.png";

    // 这是卡池的内部标识，不是玩家最终看到的角色名称。
    public override string Title => LawyerCharacter.CharacterId;


    public override Color DeckEntryCardColor => new("612D53");

    // 卡牌左上角的费用图标使用 256x256 正方形版本。
    public override string BigEnergyIconPath => LawyerEnergyIconPath;

    // 卡牌描述文字中的能量图标暂时复用同一张图片。
    public override string TextEnergyIconPath => LawyerEnergyIconPath;

    // Lawyer 是独立角色卡池，不属于无色卡池。
    public override bool IsColorless => false;
}
