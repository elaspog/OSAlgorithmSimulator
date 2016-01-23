using Simulator.Infrastructure.Models;

namespace Simulator.Infrastructure.ViewModels
{
    public interface IModuleViewModelBaseFacade : IConfigurator, ISimulator, IPresenter
    {
    }
}
