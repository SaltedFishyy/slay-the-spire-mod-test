using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using TestMod.TestModCode.Cards;
using TestMod.TestModCode.Relics;

namespace TestMod.TestModCode.Character;

#pragma warning disable STS001 // Prototype uses inline placeholder localization.
// PlaceholderCharacterModel 会为尚未制作的角色美术使用原版占位资源。
public sealed class LawyerCharacter : PlaceholderCharacterModel
{
    // 角色与卡池共用的内部标识和占位主题色。
    public const string CharacterId = "Lawyer";
    public static readonly Color Color = new("612D53");

    public override Color NameColor => LawyerCharacter.Color;
    public override Color MapDrawingColor => LawyerCharacter.Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 80;
    // 游戏内左上角、统计界面、每日挑战和历史记录等位置使用的 Lawyer 头像纹理。
    public override string? CustomIconTexturePath =>
        "res://Resources/Images/Character/LawyerProfile.png";

    // 地图上的角色标记以及多人模式表情轮盘使用的 Lawyer 头像。
    public override string? CustomMapMarkerPath =>
        "res://Resources/Images/Character/LawyerProfile.png";

    public override string? CustomCharacterSelectIconPath =>
        "res://Resources/Images/Character/CharacterMiniIcon.png";

    public override string CustomEnergyCounterPath =>
        "res://Resources/Scenes/LawyerEnergyCounter.tscn";

    public override List<(string, string)>? Localization =>
        new CharacterLoc(
            "Lawyer",
            "the Lawyer",
            "A theatrical attorney who builds Evidence, controls the courtroom, and turns proof into decisive attacks.",
            "them",
            "they",
            "theirs",
            "their",
            "The evidence speaks for itself.",
            "Your turn.",
            "No further questions.",
            "Objection!",
            "A fair settlement.",
            "Lawyer cards",
            "Cards available to the Lawyer.",
            ("THE_ARCHITECT.talk.TESTMOD-LAWYER_CHARACTER.0-0r.char", "I represent the evidence."),
            ("THE_ARCHITECT.talk.TESTMOD-LAWYER_CHARACTER.0-0r.next", "Proceed."),
            ("THE_ARCHITECT.talk.TESTMOD-LAWYER_CHARACTER.0-1r.ancient", "The record is clear."),
            ("THE_ARCHITECT.talk.TESTMOD-LAWYER_CHARACTER.0-attack", "Objection!"));

    // Phase 1 起始牌组：4 张打击、4 张防御和两张机制测试卡。
    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<LawyerStrike>(),
        ModelDb.Card<LawyerStrike>(),
        ModelDb.Card<LawyerStrike>(),
        ModelDb.Card<LawyerStrike>(),
        ModelDb.Card<LawyerDefend>(),
        ModelDb.Card<LawyerDefend>(),
        ModelDb.Card<LawyerDefend>(),
        ModelDb.Card<LawyerDefend>(),
        ModelDb.Card<CollectingEvidence>(),
        ModelDb.Card<Statement>()
    ];

    // Papers 是 Lawyer 当前唯一的起始遗物。
    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<Papers>()
    ];

    // 将角色连接到各自独立的卡牌、遗物和药水池。
    public override CardPoolModel CardPool => ModelDb.CardPool<LawyerCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<LawyerRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<LawyerPotionPool>();
}
#pragma warning restore STS001
