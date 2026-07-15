using MegaCrit.Sts2.Core.Entities.Creatures;

namespace TestMod.TestModCode.Powers;

public interface IExposeTriggerObserver
{
    void OnExposeTriggered(Creature target);
}
