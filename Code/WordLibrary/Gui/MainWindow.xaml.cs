using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Control draggedItem;
        Point itemRelativePosition;
        bool IsDragging;
        List<WordButton> buttons = new List<WordButton>();

        public MainWindow()
        {
            InitializeComponent();
            IsDragging = false;
        }

        private void LoadWords(string[] words)
        {
            Random r = new Random();

            foreach (string word in words)
            {
                if (!string.IsNullOrWhiteSpace(word))
                {
                    WordButton button = new WordButton { Content = word };
                    button.PreviewMouseLeftButtonDown += btn_PreviewMouseLeftButtonDown;
                    button.PreviewMouseLeftButtonUp += btn_PreviewMouseLeftButtonUp;
                    button.PreviewMouseMove += btn_PreviewMouseMove;
                    //button.Click += Button_Click;

                    buttons.Add(button);
                    WordCanvas.Children.Add(button);

                    Canvas.SetLeft(button, r.Next(0, (int)(this.Width - button.Width - 50)));
                    Canvas.SetTop(button, r.Next(0, (int)((this.Height - 250) - button.Height)));
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsDragging = true;
            draggedItem = (Button)sender;
            itemRelativePosition = e.GetPosition(draggedItem);
        }

        private void btn_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsDragging)
                return;

            IsDragging = false;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)

        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string[] words = File.ReadAllLines(openFileDialog.FileName);
                MessageBoxResult result = MessageBox.Show(this, "Would you like to remove the currently loaded set?", "Load Words", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                    WordCanvas.Children.Clear();
                LoadWords(words);
            }
        }

        private void btn_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!IsDragging)
                return;

            Point canvasRelativePosition = e.GetPosition(WordCanvas);

            Canvas.SetTop(draggedItem, canvasRelativePosition.Y - itemRelativePosition.Y);
            Canvas.SetLeft(draggedItem, canvasRelativePosition.X - itemRelativePosition.X);
        }
    }
}
