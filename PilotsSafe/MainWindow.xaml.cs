using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PilotsSafe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly List<Button> buttons = new();
        PlayField field = new(4);
        readonly BitmapImage bitmapImage;
        readonly BitmapImage bitmapImageInverted;
        int countMoves = 0;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            bitmapImage = BitmapImageInit(false);
            bitmapImageInverted = BitmapImageInit(true);
            InitializationPlayingField(field.Size);
        }

        void InitializationPlayingField(int size)
        {
            countMoves = 0;
            buttons.Clear();
            grid.Children.Clear();
            field = new PlayField(size);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)

                {
                    KeyValuePair<int, int> coordinatesButton = new(i, j);
                    Button button = new()
                    {
                        Tag = coordinatesButton
                    };
                    if (field.Field[i, j] == 1)
                    {
                        button.Content = CreateImage(bitmapImage);
                    } else
                    {
                        button.Content = CreateImage(bitmapImageInverted);
                    }
                    buttons.Add(button);
                    button.Click += new RoutedEventHandler(ButtonClick);
                    grid.Children.Add(button);
                }
            }
        }

        void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            field.Motion((KeyValuePair<int, int>)button.Tag);
            countMoves++;
            textBlock.Text = countMoves.ToString();
            UpdateViewField();
            if (field.VictoryCheck())
            {
                ViewVictoryMessage();
            }
        }
        void ChangeSizeField(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(textBox.Text, out int size) && size > 2)
            {
                InitializationPlayingField(size);
            } 
            else
            {
                MessageBox.Show("Invalid value");
            }
        }

        void UpdateViewField()
        {
            int indexButtons = 0;
            for (int i = 0; i < field.Size; i++)
            {
                for (int j = 0; j < field.Size; j++)
                {
                    buttons[indexButtons].Content = field.Field[i, j];
                    if (field.Field[i, j] == 1)
                    {
                        buttons[indexButtons].Content = CreateImage(bitmapImage);
                    }
                    else
                    {
                        buttons[indexButtons].Content = CreateImage(bitmapImageInverted);
                    }
                    indexButtons++;
                }
            }
        }

        void ViewVictoryMessage()
        {
            string messageBoxText = "Congratulations! You won";
            string caption = "You WIN!";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;

            MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }

        BitmapImage BitmapImageInit(bool isInvert)
        {
            BitmapImage bi = new();
            bi.BeginInit();
            try
            {
                bi.UriSource = new Uri(GetPathFile(@"resources\handle.png"));
            } 
            catch(FileNotFoundException e)
            {
                MessageBox.Show($"{e.GetType().Name}: {e.Message}");
            }          
            if (isInvert)
            {
                bi.Rotation = Rotation.Rotate270;
            }
            bi.EndInit();
            return bi;
        }

        string GetPathFile(string relativePath)
        {
            var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fullPath = "";
            if (appDir != null)
            {
                fullPath = Path.Combine(appDir, relativePath);
            }
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"{relativePath} not found");
            }
            return fullPath;
        }

        Image CreateImage(BitmapImage bi)
        {
            Image img = new()
            {
                Source = bi
            };
            return img;
        }
    }
}
