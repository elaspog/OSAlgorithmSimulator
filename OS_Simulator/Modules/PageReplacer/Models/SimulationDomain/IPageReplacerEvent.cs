
namespace PageReplacer.Models
{
    interface IPageReplacerEvent : IPageReplacer
    {
        PageRecord reactOnPeriodicalRbitRemovalEvent(PageRecord oldPageRecord);
    }
}
