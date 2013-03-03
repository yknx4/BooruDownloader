using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Presentation;
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
using System.IO;
using booruGui__TextBoxStreamWriter_;
using booruGui__Modern_.Pages;

namespace booruGui__Modern_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        
        public MainWindow()
        {
            
            InitializeComponent();
            AppearanceManager.AccentColor = Color.FromRgb(0xff, 0x45, 0x00);
        //    _writer = new TextBoxStreamWriter(logPage.txtLog);
            // Redirect the out Console stream
         //  Console.SetOut(_writer);
        }
    }
}
