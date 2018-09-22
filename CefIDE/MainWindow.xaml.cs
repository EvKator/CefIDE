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
using CefSharp;
using CefSharp.Wpf;
using CefPreter;
using System.IO;
namespace WpfApplication5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly int numOfColumns = 4;
        Interpreter interpreter;

        public MainWindow()
        {
            //numOfColumns = ((FrameworkElement[])this.Resources["els"]).Length;
            const string filename = "script.ap";
            InitializeComponent();
            interpreter = (Interpreter)getElByName((FrameworkElement[])this.Resources["els"], "interpreter");
            
            if (File.Exists(filename))
            {
                interpreter.Code = File.ReadAllText(filename);
            }
            else
            {
                File.Create(filename);
            }
            DataContext = this;

            checkBoxCode.IsChecked = true;
            checkBoxLog.IsChecked = true;
            checkBoxBrowser.IsChecked = true;
        }

        #region properties

        public List<FrameworkElement> AddedEls
        {
            get { return (List<FrameworkElement>)GetValue(AddedElsProperty); }
            set { SetValue(AddedElsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddedEls.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddedElsProperty =
            DependencyProperty.Register("AddedEls", typeof(List<FrameworkElement>), typeof(MainWindow), new PropertyMetadata(new List<FrameworkElement>(), new PropertyChangedCallback(elsListChanged)));


        public static void elsListChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            MainWindow mw = (MainWindow)obj;
            mw.RedrawGrid();
        }
        #endregion
        
        #region controlsManip

        FrameworkElement getElByName(FrameworkElement[] arr, string Name)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Name == Name)
                    return arr[i];
            }
            return null;
        }


        public void RemoveListElsFromGrid()
        {
            var els = this.Resources["els"] as FrameworkElement[];
            foreach (var el in els)
            {
                var foundEl = getElByName(els, el.Name);
                MainGrid.Children.Remove(foundEl as UIElement);
            }
        }

        public void AddListElsToGrid()
        {
            for (int i = 0; i < AddedEls.Count; i++)
            {
                Grid.SetColumn(AddedEls[i], i);
                if (i == AddedEls.Count - 1)
                    Grid.SetColumnSpan(AddedEls[i], numOfColumns - i);
                else
                    Grid.SetColumnSpan(AddedEls[i], 1);
                if (i > 0)
                {
                    Grid.SetRowSpan(AddedEls[i], 2);
                }
                else
                    Grid.SetRowSpan(AddedEls[i], 1);
                MainGrid.Children.Add(AddedEls[i]);
            }
        }

        public void RedrawGrid()
        {
            RemoveListElsFromGrid();
            AddListElsToGrid();
        }
        #endregion
        
        #region eventhandlers

        public void checkBoxChecked(bool? IsChecked, FrameworkElement el)
        {
            if (IsChecked == true)
            {
                AddedEls.Add(el);
                AddedEls = new List<FrameworkElement>(AddedEls);
            }
            else
            {
                AddedEls.Remove(el);
                AddedEls = new List<FrameworkElement>(AddedEls);
            }
        }

        private void CheckBoxCode_Checked(object sender, RoutedEventArgs e)
        {
            var el = getElByName((FrameworkElement[])this.Resources["els"], "rtbCode");
            checkBoxChecked((sender as CheckBox).IsChecked, el);
        }

        private void checkBoxCode_Unchecked(object sender, RoutedEventArgs e)
        {
            var el = getElByName((FrameworkElement[])this.Resources["els"], "rtbCode");
            checkBoxChecked((sender as CheckBox).IsChecked, el);
        }


        private void CheckBoxStack_Checked(object sender, RoutedEventArgs e)
        {
            var el = getElByName((FrameworkElement[])this.Resources["els"], "rtbStack");
            checkBoxChecked((sender as CheckBox).IsChecked, el);
        }

        private void checkBoxStack_Unchecked(object sender, RoutedEventArgs e)
        {
            var el = getElByName((FrameworkElement[])this.Resources["els"], "rtbStack");
            checkBoxChecked((sender as CheckBox).IsChecked, el);
        }


        private void CheckBoxLog_Checked(object sender, RoutedEventArgs e)
        {
            var el = getElByName((FrameworkElement[])this.Resources["els"], "rtbLog");
            checkBoxChecked((sender as CheckBox).IsChecked, el);
        }

        private void checkBoxLog_Unchecked(object sender, RoutedEventArgs e)
        {
            var el = getElByName((FrameworkElement[])this.Resources["els"], "rtbLog");
            checkBoxChecked((sender as CheckBox).IsChecked, el);
        }


        private void CheckBoxBrowser_Checked(object sender, RoutedEventArgs e)
        {
            var el = getElByName((FrameworkElement[])this.Resources["els"], "interpreter");
            checkBoxChecked((sender as CheckBox).IsChecked, el);
        }

        private void checkBoxBrowser_Unchecked(object sender, RoutedEventArgs e)
        {
            var el = getElByName((FrameworkElement[])this.Resources["els"], "interpreter");
            checkBoxChecked((sender as CheckBox).IsChecked, el);
        }


        

        private void Window_Closed(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText("script.ap", interpreter.Code);
        }


        #endregion


        private void RunBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!RunBtn.Torision)
            {
                interpreter.Stop();
            }
            else
            {
                RunBtn.Content = "Stop";
                interpreter.Analize();
                interpreter.Completed += () =>
                {
                    RunBtn.Content = "Run";
                    RunBtn.Torision = false;
                };
                interpreter.Stopped += () =>
                {
                    RunBtn.Content = "Run";
                };
                interpreter.Run();
            }
        }

        
    }
}
