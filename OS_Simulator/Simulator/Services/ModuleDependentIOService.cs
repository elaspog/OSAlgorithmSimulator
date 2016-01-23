using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using Simulator.Infrastructure.EventArgs;
using Simulator.Infrastructure.Repository;
using System.IO;

namespace Simulator.Services
{
    class ModuleDependentIOService : IOService
    {
        List<Simulator.Infrastructure.Module> modules = new List<Simulator.Infrastructure.Module>();

        public ModuleDependentIOService()
        {
            modules.Add(new TaskScheduler.TaskScheduler());
            modules.Add(new MemoryAllocator.MemoryAllocator());
            modules.Add(new PageReplacer.PageReplacer());
            modules.Add(new VirtualAddressMapper.VirtualAddressMapper());
        }

        public string[] OpenFiles()
        {
            string[] filenames = null;

            try
            {
                // Configure open file dialog box 
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.FileName = "Document"; // Default file name 
                dlg.DefaultExt = ".xml"; // Default file extension 
                dlg.Filter = "Text documents (.xml)|*.xml"; // Filter files by extension 
                dlg.Multiselect = true;

                // Show open file dialog box 
                Nullable<bool> result = dlg.ShowDialog();

                // Process open file dialog box results 
                if (result == true)
                {
                    // Open document 
                    filenames = dlg.FileNames;
                }
            }
            catch (Exception e)
            {
                Messenger.Default.Send(new SendModalWindowMessage("Error in opening file \n" + e.ToString(), "Exception"));
            }

            return filenames;
        }

        public SimulationRecordWithModuleInfo ReadXmlHeader(string fileName)
        {
            string RootElementName = null;
            string RootTitle = null;
            string RootFQN = null;
            Type moduleType = null;

            try
            {
                System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Load(fileName);
                RootElementName = doc.Root.Name.LocalName;

                IEnumerable<string> titles = from el in doc.Elements(RootElementName)
                                            select (string)el.Attribute("Title");
                foreach (string title in titles)
                {
                    RootTitle += title;
                }

                IEnumerable<string> classnames = from el in doc.Elements(RootElementName)
                                                 select (string)el.Attribute("FQN_NamespaceAndClassName");
                foreach (string classname in classnames)
                {
                    RootFQN += classname;
                }

                try
                {
                    moduleType = Type.GetType(RootFQN + ", " + RootElementName, true);
                    return new SimulationRecordWithModuleInfo(RootTitle, moduleType);
                }
                catch (Exception e)
                {
                    Messenger.Default.Send(new SendModalWindowMessage("Error in getting module type \n" + e.ToString(), "Exception"));
                }
            }
            catch (Exception e)
            {
                Messenger.Default.Send(new SendModalWindowMessage("Error in reading file \n" + e.ToString(), "Exception"));
            }
            return null;
        }

        public Simulator.Infrastructure.Module GetModuleInstanceBySimulationRecord(SimulationRecordWithModuleInfo simulationRecord)
        {
            foreach (Simulator.Infrastructure.Module module in modules)
            {
                if (simulationRecord.SimulationType.Equals(module.GetType()))
                {
                    return module;
                }
            }
            return null;
        }

        public System.IO.FileStream GetFileStream(string SelectedPath)
        {
            return File.OpenRead(SelectedPath);
        }
    }
}
