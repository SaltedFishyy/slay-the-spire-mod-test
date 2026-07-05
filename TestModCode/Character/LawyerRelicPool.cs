using BaseLib.Abstracts;
using Godot;

namespace TestMod.TestModCode.Character;

// 定义 Lawyer 专属遗物池，当前只包含起始遗物 CaseFile。
public sealed class LawyerRelicPool : CustomRelicPoolModel
{
    // 暂时复用 Lawyer 的占位主题色绘制遗物轮廓。
    public override Color LabOutlineColor => LawyerCharacter.Color;
}
