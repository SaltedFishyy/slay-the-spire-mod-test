using BaseLib.Abstracts;
using Godot;

namespace TestMod.TestModCode.Character;

// 自定义角色需要一个药水池；Phase 1 暂时不添加专属药水。
public sealed class LawyerPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => LawyerCharacter.Color;
}
