using Simulator.Infrastructure.Services;

namespace Simulator.Services
{
    interface IOService : IOBaseService
    {
        string[] OpenFiles();

        System.IO.FileStream GetFileStream(string SelectedPath);
    }
}
