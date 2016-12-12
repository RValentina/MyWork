using Microsoft.Win32;
using System.Windows;
using AccountViewModel;

namespace ATTSCodeAssignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new AccViewModel();
        }

    }
}
