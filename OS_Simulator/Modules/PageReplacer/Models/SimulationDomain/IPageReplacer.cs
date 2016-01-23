
namespace PageReplacer.Models
{
    interface IPageReplacer
    {
        PageRecord processPageRequestAndReturnNewPageRecordAccordingToHistory(int i, PageRecord pageRecord);

        bool usesPeriodsToRemoveRbits();
    }
}
