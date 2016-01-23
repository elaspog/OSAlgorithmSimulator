using PageReplacer.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PageReplacer.Views.UserControls
{
    /// <summary>
    /// Interaction logic for HistoricalPageTable.xaml
    /// </summary>
    public partial class HistoricalPageTable : UserControl
    {
        public HistoricalPageTable()
        {
            InitializeComponent();
            DataContextChanged += new DependencyPropertyChangedEventHandler(SimulatorView_DataContextChanged);
        }

        private Type simulationType;
        public Type SimulationType
        {
            get { return simulationType; }
            set { simulationType = value; }
        }

        private void SimulatorView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (SimulationType == null)
            {
                if (this.DataContext != null && this.DataContext.GetType() == typeof(PR_SimulatorModel))
                {
                    SimulationType = ((PR_SimulatorModel)this.DataContext).PageReplacerType;

                    this.Resources.Add("SimulationType", SimulationType);
                }
            }
        }
    }
}
