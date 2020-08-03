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

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            foreach (UIElement uiEle in LayoutRoot.Children)
            {
                //WPF设计上的问题,Button.Clicked事件Supress掉了Mouse.MouseLeftButtonDown附加事件等.
                //不加这个Button、TextBox等无法拖动
                if (uiEle is Button || uiEle is TextBox)
                {
                    uiEle.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(Element_MouseLeftButtonDown), true);
                    uiEle.AddHandler(Button.MouseMoveEvent, new MouseEventHandler(Element_MouseMove), true);
                    uiEle.AddHandler(Button.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Element_MouseLeftButtonUp), true);
                    continue;
                }
                //
                uiEle.MouseMove += new MouseEventHandler(Element_MouseMove);
                uiEle.MouseLeftButtonDown += new MouseButtonEventHandler(Element_MouseLeftButtonDown);
                uiEle.MouseLeftButtonUp += new MouseButtonEventHandler(Element_MouseLeftButtonUp);
            }
        }

        bool isDragDropInEffect = false;
        Point pos = new Point();

        void Element_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragDropInEffect)
            {
                FrameworkElement currEle = sender as FrameworkElement;
                double xPos = e.GetPosition(null).X - pos.X + (double)currEle.GetValue(Canvas.LeftProperty);
                double yPos = e.GetPosition(null).Y - pos.Y + (double)currEle.GetValue(Canvas.TopProperty);
                currEle.SetValue(Canvas.LeftProperty, xPos);
                currEle.SetValue(Canvas.TopProperty, yPos);
                pos = e.GetPosition(null);
            }
        }

        void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            FrameworkElement fEle = sender as FrameworkElement;
            isDragDropInEffect = true;
            pos = e.GetPosition(null);
            fEle.CaptureMouse();
            fEle.Cursor = Cursors.Hand;
        }

        void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragDropInEffect)
            {
                FrameworkElement ele = sender as FrameworkElement;
                isDragDropInEffect = false;
                ele.ReleaseMouseCapture();
            }
        }

        
    }

}
