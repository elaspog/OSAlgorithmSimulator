using Simulator.Infrastructure.ViewModels;

namespace Simulator.Infrastructure.Views
{
    // ezen az interfészen keresztül jelölheti meg a View típus, hogy milyen típusú ViewModel-t szeretne használni
    // hátrány: csak egy ViewModel-t jelölhet meg
    // több ViewModel példányra való hivatkozáshoz listára kell módosítani a GetViewModel_UsingAsDataContextType visszatérési értékét
    public interface IModuleViewBase
    {
        //IModuleViewModelBase GetViewModel_UsingAsDataContextType();


        //IModuleViewModelBase GetViewsDataContextAsViewModelPropertyOfModuleViewModel();

        string GetViewsDataContextAsPropertyNameOfModuleViewModel();
    }
}
