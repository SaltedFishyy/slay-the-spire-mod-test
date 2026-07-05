using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using TestMod.TestModCode.Character;

namespace TestMod.TestModCode.Cards;

// 所有 Lawyer 卡牌继承这个基类，因此会自动注册进 LawyerCardPool。
[Pool(typeof(LawyerCardPool))]
public abstract class LawyerCard(int cost, CardType type, CardRarity rarity, TargetType target)
    : CustomCardModel(cost, type, rarity, target)
{
}
