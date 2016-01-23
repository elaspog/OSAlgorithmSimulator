using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Simulator.Infrastructure.Repository
{
    public class SimulationRecordWithModuleInfo
    {
        string simulationName;
        Type simulationType;
        string assemblyName = "";
        string fullName = "";
        FileStream fileStream;
        SimulationStatus status;

        public SimulationRecordWithModuleInfo(string simulationName, Type simulationType) 
        {
            this.simulationName = simulationName;
            this.SimulationType = simulationType;
            status = SimulationStatus.Initial;
        }

        public string SimulationName
        {
            get { return simulationName; }
            set { simulationName = value; }
        }

        public Type SimulationType
        {
            get { return simulationType; }
            set 
            { 
                simulationType = value;
                fullName = value.FullName;
                assemblyName = GetAssemblyNameWithoutParams(value.AssemblyQualifiedName);
            }
        }

        public SimulationStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        public FileStream FileStream
        {
            get { return fileStream; }
            set { fileStream = value; }
        }

        private string GetAssemblyNameWithoutParams(string input)
        {
            string pattern = @"^\S+\s(?<moduleName>[a-zA-Z]*).*";
            string replacement = @"${moduleName}";
            string result = Regex.Replace(input, pattern, replacement);

            return result;
        }

        public string AssemblyName
        {
            get { return assemblyName; }
        }

        public string FullName
        {
            get { return fullName; }
        }

        public static T GetObjectSafeCastedAs<T>(object _ObjToCast)
        {
            if (_ObjToCast.GetType().IsAssignableFrom(typeof(T)))
            {
                return (T)_ObjToCast;
            }
            else return default(T);
        }

        public override bool Equals(object obj)
        {
            if (GetObjectSafeCastedAs<SimulationRecordWithModuleInfo>(obj) == null)
            {
                return false;
            }

            return ((GetObjectSafeCastedAs<SimulationRecordWithModuleInfo>(obj)).SimulationName.Equals(this.SimulationName)
                    && (GetObjectSafeCastedAs<SimulationRecordWithModuleInfo>(obj)).SimulationType.Equals(this.SimulationType));
        }

    }
}
