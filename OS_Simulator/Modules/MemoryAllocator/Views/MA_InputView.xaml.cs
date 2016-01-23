using MemoryAllocator.Models;
using Simulator.Infrastructure.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MemoryAllocator.Views
{
    /// <summary>
    /// Interaction logic for MA_InputView.xaml
    /// </summary>
    public partial class MA_InputView : UserControl, IModuleViewBase
    {
        public MA_InputView()
        {
            InitializeComponent();
            DataContextChanged += new DependencyPropertyChangedEventHandler(InputView_DataContextChanged);
        }


        private void InputView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext != null && this.DataContext.GetType() == typeof(MA_Descriptor))
            {
                MA_Descriptor descriptor = (MA_Descriptor)this.DataContext;

                DrawRectangle(descriptor);
            }
        }

        private void DrawRectangle(MA_Descriptor descriptor)
        {
            int WIDTH = 650;
            int HEIGHT = 50;
            
            int physicalMemorySize = descriptor.PhysicalMemory.PhysicalMemorySize;
            int systemMemorySize = descriptor.PhysicalMemory.SystemMemorySize;
            int userMemorySize = descriptor.PhysicalMemory.UserMemorySize;

            int partitionListSize = descriptor.PartitionListSize;
            int remainingSpaceAfterPartition = userMemorySize - partitionListSize;

            float SCALE = (float)WIDTH / (float)physicalMemorySize  ;
            float initialShift = 10;
            float setTop = 10;


            Rectangle systemMemoryRectangle = new Rectangle();
            systemMemoryRectangle.Width = SCALE * systemMemorySize;
            systemMemoryRectangle.Height = HEIGHT;
            systemMemoryRectangle.Fill = new SolidColorBrush(Colors.DarkGray);
            Canvas.SetLeft(systemMemoryRectangle, initialShift);
            Canvas.SetTop(systemMemoryRectangle, setTop);
            canvas.Children.Add(systemMemoryRectangle);
            
            float horizontalShift;
            horizontalShift = initialShift + SCALE * systemMemorySize;

            TextBlock systemMemoryText = new TextBlock();
            systemMemoryText.Text = systemMemorySize.ToString();
            systemMemoryText.FontWeight = FontWeights.Bold;
            Canvas.SetLeft(systemMemoryText, initialShift);
            Canvas.SetTop(systemMemoryText, setTop);
            canvas.Children.Add(systemMemoryText);

            TextBlock labelText = new TextBlock();
            labelText.Text = "KB";
            labelText.FontWeight = FontWeights.Bold;
            Canvas.SetLeft(labelText, initialShift);
            Canvas.SetTop(labelText, setTop + systemMemoryText.FontSize);
            canvas.Children.Add(labelText);

            foreach (PartitionBase partition in descriptor.MemoryAllocation)
            {
                float width = SCALE * (float)partition.Size;

                Rectangle rectangle = new Rectangle();
                rectangle.Width = width;
                rectangle.Height = HEIGHT;
                if (partition.GetType() == typeof(UsedPartition))
                {
                    rectangle.Fill = new SolidColorBrush(Colors.OrangeRed);
                }
                if (partition.GetType() == typeof(FreePartition))
                {
                    rectangle.Fill = new SolidColorBrush(Colors.LightBlue);
                }
                Canvas.SetLeft(rectangle, horizontalShift);
                Canvas.SetTop(rectangle, setTop);
                canvas.Children.Add(rectangle);


                TextBlock partitionText = new TextBlock();
                partitionText.Text = partition.Size.ToString();
                partitionText.FontWeight = FontWeights.Bold;
                Canvas.SetLeft(partitionText, horizontalShift);
                Canvas.SetTop(partitionText, setTop);
                canvas.Children.Add(partitionText);

                TextBlock partLabelText = new TextBlock();
                partLabelText.Text = "KB";
                partLabelText.FontWeight = FontWeights.Bold;
                Canvas.SetLeft(partLabelText, horizontalShift);
                Canvas.SetTop(partLabelText, setTop + systemMemoryText.FontSize);
                canvas.Children.Add(partLabelText);

                horizontalShift += width;
            }

            Rectangle systemMemoryRectangleRemaining = new Rectangle();
            systemMemoryRectangleRemaining.Width = SCALE * remainingSpaceAfterPartition;
            systemMemoryRectangleRemaining.Height = HEIGHT;
            systemMemoryRectangleRemaining.Fill = new SolidColorBrush(Colors.DarkGray);
            Canvas.SetLeft(systemMemoryRectangleRemaining, horizontalShift);
            Canvas.SetTop(systemMemoryRectangleRemaining, setTop);
            canvas.Children.Add(systemMemoryRectangleRemaining);

            TextBlock partitionRemainingText = new TextBlock();
            partitionRemainingText.Text = remainingSpaceAfterPartition.ToString();
            partitionRemainingText.FontWeight = FontWeights.Bold;
            Canvas.SetLeft(partitionRemainingText, horizontalShift);
            Canvas.SetTop(partitionRemainingText, setTop);
            canvas.Children.Add(partitionRemainingText);

            TextBlock remainingLabelText = new TextBlock();
            remainingLabelText.Text = "KB";
            remainingLabelText.FontWeight = FontWeights.Bold;
            Canvas.SetLeft(remainingLabelText, horizontalShift);
            Canvas.SetTop(remainingLabelText, setTop + systemMemoryText.FontSize);
            canvas.Children.Add(remainingLabelText);
        }

        public string GetViewsDataContextAsPropertyNameOfModuleViewModel()
        {
            return "InputDescriptor";
        }
    }
}
