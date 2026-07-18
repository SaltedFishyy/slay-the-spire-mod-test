using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TestMod.TestModCode.Powers;

namespace TestMod.TestModCode.Cards;

public sealed class Page78Please : LawyerCard
{
    public override string? CustomPortraitPath =>
        "res://Resources/Images/Cards/Page78Please.png";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [new DamageVar(6, ValueProp.Move), new DynamicVar("EvidenceCost", 5), new DynamicVar("Expose", 1), new DynamicVar("Weak", 1)];
    protected override bool IsPlayable => base.IsPlayable &&
        (Owner?.Creature is not { } creature || EvidenceHelper.Get(creature) >= DynamicVars["EvidenceCost"].IntValue);
    public Page78Please() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
    public override List<(string, string)>? Localization =>
        new CardLoc("Page 78, Please", "Spend {EvidenceCost} Evidence. Deal {Damage:diff()} damage. Apply {Expose} Expose and {Weak} Weak.");
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        if (!await EvidenceHelper.Spend(context, Owner.Creature, DynamicVars["EvidenceCost"].IntValue, this)) return;
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).Execute(context);
        await ExposeHelper.Apply(context, cardPlay.Target, DynamicVars["Expose"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<WeakPower>(context, cardPlay.Target, DynamicVars["Weak"].IntValue, Owner.Creature, this);
    }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2);
}
