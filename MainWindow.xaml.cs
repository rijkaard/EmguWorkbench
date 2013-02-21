using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Emgu;
using Emgu.CV;

namespace EmguWorkbench
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EmguController emguController;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            emguController = new EmguController();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    emguController.testPolygonTest();
                    break;
                case Key.Enter:
                    emguController.testCvSolve2();
                    break;
                case Key.E:
                    emguController.testEigen();
                    break;
                case Key.S:
                    emguController.testSVD();
                    break;
                case Key.C:
                    CamCaptureWindow nw = new CamCaptureWindow();
                    if(null!=nw)
                        nw.ShowDialog();
                    break;
                default:
                    break;
            }
        }
    }
}
