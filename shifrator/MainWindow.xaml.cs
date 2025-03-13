using Microsoft.Win32;
using System.Data.Common;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

namespace shifrator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
        const String russAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        private int columns = 0;
        private int rows = 0;
        private string key = string.Empty, text = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
        }

        private List<Button> buttons = new List<Button>();

        private void CreateDynamicMatrix(int rows, int columns)
        {
            double cellSize = 50;
            // Add rows and columns to the Grid
            for (int i = 0; i < rows; i++)
            {
                MyMatrix.RowDefinitions.Add(new RowDefinition { Height = new GridLength(cellSize) });
            }

            for (int j = 0; j < columns; j++)
            {
                MyMatrix.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(cellSize) });
            }

            // Create buttons and add them to the Grid and the list
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Button button = new Button
                    {
                        Width = Double.NaN,
                        Height = Double.NaN,
                        Padding = new Thickness(0),
                        FontSize = 10,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center
                    };
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    MyMatrix.Children.Add(button);
                    buttons.Add(button); // Add the button to the list
                }
            }
        }


        private void AccessButton(int row, int column, string content)
        {
            int color = 0;
            // Предполагается, что количество строк и столбцов неизменны
            int index = row * columns + column;

            if (row == 0)
            {
                color = 1;
            }
            else if (row == 1)
            {
                color = 2;
            }


            if (index >= 0 && index < buttons.Count)
            {
                Button button = buttons[index];
                if (color == 1)
                {
                    button.Background = Brushes.Green;
                }
                else if (color == 2)
                {
                    button.Background = Brushes.Red;
                }
                button.Content = content.ToString();
                button.HorizontalContentAlignment = HorizontalAlignment.Center;
                button.VerticalContentAlignment = VerticalAlignment.Center;
            }
        }

        private string GetButtonString(int row, int column)
        {
            int index = row * columns + column;
            if (index >= 0 && index < buttons.Count)
            {
                Button button = buttons[index];
                if (button.Content != null)
                    return button.Content.ToString();
            }
            return "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            clearMatrix();
            if (!isValid(sender))
            {
                return;
            }
            calcMatrixSize(sender);
            CreateDynamicMatrix(rows, columns);
            fillMatrix(sender, rows, columns, key, text);
            SypheredText.Text = encryptText(rows, columns);
        }

        private void DesypherButton_Click(object sender, RoutedEventArgs e)
        {
            clearMatrix();
            if (!isValid(sender))
            {
                return;
            }
            calcMatrixSize(sender);
            CreateDynamicMatrix(rows, columns);
            fillMatrix(sender, rows, columns, key, text);
            TextToSypher.Text = decryptText(rows, columns);
        }

        private void clearMatrix()
        {
            buttons.Clear();
            MyMatrix.RowDefinitions.Clear();
            MyMatrix.ColumnDefinitions.Clear();
            MyMatrix.Children.Clear();
            rows = 0;
            columns = 0;
            key = string.Empty;
            text = string.Empty;
        }

        private bool isValid(object sender)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                if (clickedButton.Name == SypherButton.Name)
                {
                    if (TextToSypher.Text.Length == 0 || SypherKey.Text.Length == 0)
                    {
                        clearMatrix();
                        //throw not valid message
                        return false;
                    }

                    bool isValid = false;

                    for (int i = 0; i < SypherKey.Text.Length; i++)
                    {
                        for (int j = 0; j < russAlphabet.Length; j++)
                        {
                            if (SypherKey.Text[i] == russAlphabet[j] || SypherKey.Text[i] == char.ToUpper(russAlphabet[j]))
                            {
                                isValid = true;
                                break;
                            }
                        }

                        if (isValid)
                            break;
                    }

                    if (isValid)
                    {
                        for (int i = 0; i < TextToSypher.Text.Length; i++)
                        {
                            for (int j = 0; j < russAlphabet.Length; j++)
                            {
                                if (TextToSypher.Text[i] == russAlphabet[j] || TextToSypher.Text[i] == char.ToUpper(russAlphabet[j]))
                                {
                                    isValid = true;
                                    break;
                                }
                            }

                            if (isValid)
                                break;
                        }
                    }
                    else
                    {
                        // throw key error
                    }



                    if (!isValid)
                    {
                        // Throw text not valid message
                    }

                    return isValid;
                }
                else if (clickedButton.Name == DesypherButton.Name)
                {
                    if (SypheredText.Text.Length == 0 || SypherKey.Text.Length == 0)
                    {
                        clearMatrix();
                        //throw not valid message
                        return false;
                    }

                    bool isValid = false;

                    for (int i = 0; i < SypherKey.Text.Length; i++)
                    {
                        for (int j = 0; j < russAlphabet.Length; j++)
                        {
                            if (SypherKey.Text[i] == russAlphabet[j] || SypherKey.Text[i] == char.ToUpper(russAlphabet[j]))
                            {
                                isValid = true;
                                break;
                            }
                        }

                        if (isValid)
                            break;
                    }

                    if (isValid)
                    {
                        for (int i = 0; i < SypheredText.Text.Length; i++)
                        {
                            for (int j = 0; j < russAlphabet.Length; j++)
                            {
                                if (SypheredText.Text[i] == russAlphabet[j] || SypheredText.Text[i] == char.ToUpper(russAlphabet[j]))
                                {
                                    isValid = true;
                                    break;
                                }
                            }

                            if (isValid)
                                break;
                        }
                    }
                    else
                    {
                        // throw key error
                    }



                    if (!isValid)
                    {
                        // Throw text not valid message
                    }

                    return isValid;
                } else if (clickedButton.Name == VignerSyperButton.Name)
                {
                    if (VignerTextToSypher.Text.Length == 0 || VignerSypherKey.Text.Length == 0)
                    {
                        
                        //throw not valid message
                        return false;
                    }

                    bool isValid = false;

                    for (int i = 0; i < VignerSypherKey.Text.Length; i++)
                    {
                        for (int j = 0; j < russAlphabet.Length; j++)
                        {
                            if (VignerSypherKey.Text[i] == russAlphabet[j] || VignerSypherKey.Text[i] == char.ToUpper(russAlphabet[j]))
                            {
                                isValid = true;
                                break;
                            }
                        }

                        if (isValid)
                            break;
                    }

                    if (isValid)
                    {
                        for (int i = 0; i < VignerTextToSypher.Text.Length; i++)
                        {
                            for (int j = 0; j < russAlphabet.Length; j++)
                            {
                                if (VignerTextToSypher.Text[i] == russAlphabet[j] || VignerTextToSypher.Text[i] == char.ToUpper(russAlphabet[j]))
                                {
                                    isValid = true;
                                    break;
                                }
                            }

                            if (isValid)
                                break;
                        }
                    }
                    else
                    {
                        // throw key error
                    }



                    if (!isValid)
                    {
                        // Throw text not valid message
                    }

                    return isValid;
                } else if (clickedButton.Name == VignerDesypherButton.Name)
                {
                    if (VignerSypheredText.Text.Length == 0 || VignerSypherKey.Text.Length == 0)
                    {
                        clearMatrix();
                        //throw not valid message
                        return false;
                    }

                    bool isValid = false;

                    for (int i = 0; i < VignerSypherKey.Text.Length; i++)
                    {
                        for (int j = 0; j < russAlphabet.Length; j++)
                        {
                            if (VignerSypherKey.Text[i] == russAlphabet[j] || VignerSypherKey.Text[i] == char.ToUpper(russAlphabet[j]))
                            {
                                isValid = true;
                                break;
                            }
                        }

                        if (isValid)
                            break;
                    }

                    if (isValid)
                    {
                        for (int i = 0; i < VignerSypheredText.Text.Length; i++)
                        {
                            for (int j = 0; j < russAlphabet.Length; j++)
                            {
                                if (VignerSypheredText.Text[i] == russAlphabet[j] || VignerSypheredText.Text[i] == char.ToUpper(russAlphabet[j]))
                                {
                                    isValid = true;
                                    break;
                                }
                            }

                            if (isValid)
                                break;
                        }
                    }
                    else
                    {
                        // throw key error
                    }



                    if (!isValid)
                    {
                        // Throw text not valid message
                    }

                    return isValid;
                }
            }


            return false;
        }

        private void calcMatrixSize(object sender)
        {
            int i, j, l;

            for (j = 0; j < SypherKey.Text.Length; j++)
            {
                for (i = 0; i < russAlphabet.Length; i++)
                {
                    if (russAlphabet[i] == SypherKey.Text[j] || char.ToUpper(russAlphabet[i]) == SypherKey.Text[j])
                    {
                        key += SypherKey.Text[j];
                    }
                }
            }

            Button clickedButton = sender as Button;

            if (clickedButton != null)
            {
                if (clickedButton.Name == SypherButton.Name)
                {
                    for (l = 0; l < TextToSypher.Text.Length; l++)
                    {
                        for (i = 0; i < russAlphabet.Length; i++)
                        {
                            if (russAlphabet[i] == TextToSypher.Text[l] || char.ToUpper(russAlphabet[i]) == TextToSypher.Text[l])
                                text += TextToSypher.Text[l];
                        }
                    }

                    columns = key.Length;
                    if (text.Length / columns > 1 || text.Length % columns != 0)
                    {
                        rows = text.Length % columns == 0 ? text.Length / columns + 2 : text.Length / columns + 3;
                    }
                    else
                    {
                        rows = 3;
                    }
                }
                else if (clickedButton.Name == DesypherButton.Name)
                {
                    for (l = 0; l < SypheredText.Text.Length; l++)
                    {
                        for (i = 0; i < russAlphabet.Length; i++)
                        {
                            if (russAlphabet[i] == SypheredText.Text[l] || char.ToUpper(russAlphabet[i]) == SypheredText.Text[l])
                                text += SypheredText.Text[l];
                        }
                    }

                    columns = key.Length;
                    if (text.Length / columns > 1 || text.Length % columns != 0)
                    {
                        rows = text.Length % columns == 0 ? text.Length / columns + 2 : text.Length / columns + 3;
                    }
                    else
                    {
                        rows = 3;
                    }
                }
            }


        }

        private string decryptText(int rows, int cols)
        {
            string desypheredText = string.Empty;

            for (int i = 2; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (GetButtonString(i, j) != null)
                    {
                        desypheredText += GetButtonString(i, j);
                    }
                }
            }

            return desypheredText;
        }

        private string encryptText(int rows, int cols)
        {
            string sypheredText = string.Empty;
            int sypheredTextCount = 0;
            int i = 0;
            int currentNumber = 1;
            int letterCount = 0;
            bool shouldIncrease = true;

            while (i != cols)
            {
                shouldIncrease = true;
                if (currentNumber == 13 || currentNumber == 20 || currentNumber == 21)
                {
                    currentNumber = currentNumber;
                }

                if (GetButtonString(0, i) == currentNumber.ToString())
                {
                    for (int j = 2; j < rows; j++)
                    {
                        if (GetButtonString(j, i) != "")
                        {
                            sypheredText += GetButtonString(j, i);
                            sypheredTextCount++;
                            //i = 0;
                        }

                        if (sypheredTextCount == 5)
                        {
                            sypheredText += " ";
                            sypheredTextCount = 0;
                        }
                    }

                    currentNumber++;
                    i = 0;
                    shouldIncrease = false;
                }

                if (shouldIncrease)
                    i++;
            }

            return sypheredText;
        }

        private void fillMatrix(object sender, int rows, int cols, string key, string text)
        {
            Button clickedButton = sender as Button;

            if (clickedButton != null)
            {
                if (clickedButton.Name == SypherButton.Name)
                {
                    int index = 1;

                    for (int m = 0; m < russAlphabet.Length; m++)
                    {
                        for (int n = 0; n < key.Length; n++)
                        {
                            if (russAlphabet[m] == key[n] || char.ToUpper(russAlphabet[m]) == key[n])
                            {
                                AccessButton(0, n, index.ToString());
                                index++;
                            }
                        }
                    }

                    for (int i = 0; i < cols; i++)
                    {
                        AccessButton(1, i, key[i].ToString());
                    }

                    int j = 0;
                    int t = 0;

                    int b = 2;

                    while (t != text.Length)
                    {
                        if (j == cols)
                        {
                            j = 0;
                            b++;
                        }


                        AccessButton(b, j, text[t].ToString());

                        j++;
                        t++;
                    }
                }
                else
                {
                    int index = 1;

                    for (int m = 0; m < russAlphabet.Length; m++)
                    {
                        for (int n = 0; n < key.Length; n++)
                        {
                            if (russAlphabet[m] == key[n] || char.ToUpper(russAlphabet[m]) == key[n])
                            {
                                AccessButton(0, n, index.ToString());
                                index++;
                            }
                        }
                    }

                    for (int i = 0; i < cols; i++)
                    {
                        AccessButton(1, i, key[i].ToString());
                    }

                    int j = 2;
                    int t = 0;

                    int b = 0;
                    int currentNumber = 1;
                    int difference = (rows - 2) * cols - text.Length;

                    while (t != text.Length)
                    {
                        while (GetButtonString(0, b) != currentNumber.ToString())
                        {
                            b++;
                            //if (currentNumber == cols + 1) break;
                        }

                        if (difference == 0)
                        {
                            if (j == rows)
                            {
                                j = 2;
                                b = 0;
                                currentNumber++;
                                continue;
                            }
                        }
                        else
                        {
                            if (j == rows - 1 && cols - difference <= b)
                            {
                                j = 2;
                                b = 0;
                                currentNumber++;
                                continue;
                            }
                            else if (j == rows)
                            {
                                j = 2;
                                b = 0;
                                currentNumber++;
                                continue;
                            }
                        }


                        AccessButton(j, b, text[t].ToString());

                        j++;
                        t++;
                    }
                }
            }


        }


        // ВИЖНЕР

        private const string RussianAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        private const string RussianAlphabetUpper = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

        public static string VigenereEncryptAutoKey(string text, string key)
        {
            StringBuilder result = new StringBuilder();
            key = key.ToLower();
            key += text.ToLower(); // Добавляем сам текст в ключ

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                bool isUpperCase = char.IsUpper(c);
                char lowerChar = char.ToLower(c);

                if (RussianAlphabet.Contains(lowerChar))
                {
                    char k = key[i]; // Теперь ключ дополняется текстом
                    int textIndex = RussianAlphabet.IndexOf(lowerChar);
                    int keyIndex = RussianAlphabet.IndexOf(k);
                    char encryptedChar = RussianAlphabet[(textIndex + keyIndex) % RussianAlphabet.Length];
                    result.Append(isUpperCase ? char.ToUpper(encryptedChar) : encryptedChar);
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        private void VignerSyperButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isValid(sender))
            {
                return;
            }

            string textToSypher = string.Empty;
            string key = string.Empty;

            for (int i = 0; i < VignerTextToSypher.Text.Length; i++)
            {
                for (int j = 0; j < russAlphabet.Length; j++)
                {
                    if (VignerTextToSypher.Text[i] == russAlphabet[j] || VignerTextToSypher.Text[i] == char.ToUpper(russAlphabet[j]))
                    {
                        textToSypher += VignerTextToSypher.Text[i];
                    }
                }
            }

            for (int i = 0; i < VignerSypherKey.Text.Length; i++)
            {
                for (int j = 0; j < russAlphabet.Length; j++)
                {
                    if (VignerSypherKey.Text[i] == russAlphabet[j] || VignerSypherKey.Text[i] == char.ToUpper(russAlphabet[j]))
                    {
                        key += VignerSypherKey.Text[i];
                    }
                }
            }

            VignerSypheredText.Text = VigenereEncryptAutoKey(textToSypher, key);
        }

        private void VignerDesypherButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isValid(sender))
            {
                return;
            }

            string textToSypher = string.Empty;
            string key = string.Empty;

            for (int i = 0; i < VignerSypheredText.Text.Length; i++)
            {
                for (int j = 0; j < russAlphabet.Length; j++)
                {
                    if (VignerSypheredText.Text[i] == russAlphabet[j] || VignerSypheredText.Text[i] == char.ToUpper(russAlphabet[j]))
                    {
                        textToSypher += VignerSypheredText.Text[i];
                    }
                }
            }

            for (int i = 0; i < VignerSypherKey.Text.Length; i++)
            {
                for (int j = 0; j < russAlphabet.Length; j++)
                {
                    if (VignerSypherKey.Text[i] == russAlphabet[j] || VignerSypherKey.Text[i] == char.ToUpper(russAlphabet[j]))
                    {
                        key += VignerSypherKey.Text[i];
                    }
                }
            }

            VignerTextToSypher.Text = VigenereDecryptAutoKey(textToSypher, key);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;

            int selectedTab = myTabControl.SelectedIndex;
            if (selectedTab == 0)
            {
                if (item != null)
                {
                    if (item.Name == MenuItemOpenKey.Name)
                    {
                        OpenFile(SypherKey);
                    }
                    else if (item.Name == MenuItemOpenTextToSypher.Name)
                    {
                        OpenFile(TextToSypher);
                    }
                    else if (item.Name == MenuItemOpenTextToDesypher.Name)
                    {
                        OpenFile(SypheredText);
                    }
                    else if (item.Name == MenuItemSave.Name)
                    {
                        SaveFile(TextToSypher, SypherKey, SypheredText);
                    }
                }
            }
            else if (selectedTab == 1)
            {
                if (item != null)
                {
                    if (item.Name == MenuItemOpenKey.Name)
                    {
                        OpenFile(VignerSypherKey);
                    }
                    else if (item.Name == MenuItemOpenTextToSypher.Name)
                    {
                        OpenFile(VignerTextToSypher);
                    }
                    else if (item.Name == MenuItemOpenTextToDesypher.Name)
                    {
                        OpenFile(VignerSypheredText);
                    }
                    else if (item.Name == MenuItemSave.Name)
                    {
                        SaveFile(VignerTextToSypher, VignerSypherKey, VignerSypheredText);
                    }
                }
            }
        }

        public static string VigenereDecryptAutoKey(string text, string key)
        {
            StringBuilder result = new StringBuilder();
            key = key.ToLower();
            StringBuilder fullKey = new StringBuilder(key); // Восстанавливаем ключ по ходу дешифрования

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                bool isUpperCase = char.IsUpper(c);
                char lowerChar = char.ToLower(c);

                if (RussianAlphabet.Contains(lowerChar))
                {
                    char k = fullKey[i]; // Берём текущий символ ключа
                    int textIndex = RussianAlphabet.IndexOf(lowerChar);
                    int keyIndex = RussianAlphabet.IndexOf(k);
                    char decryptedChar = RussianAlphabet[(textIndex - keyIndex + RussianAlphabet.Length) % RussianAlphabet.Length];

                    result.Append(isUpperCase ? char.ToUpper(decryptedChar) : decryptedChar);

                    fullKey.Append(decryptedChar); // Восстанавливаем ключ
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        public static void OpenFile(System.Windows.Controls.TextBox targetTextBox)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt",
                Title = "Open Text File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string content = File.ReadAllText(openFileDialog.FileName);
                    targetTextBox.Text = content;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading file: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public static void SaveFile(System.Windows.Controls.TextBox textBox, System.Windows.Controls.TextBox keyBox, System.Windows.Controls.TextBox sypheredBox)
        {
            // Show Save File Dialog
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt",
                Title = "Save Text File",
                DefaultExt = ".txt",
                AddExtension = true
            };

            bool? result = saveFileDialog.ShowDialog();

            // If user cancels, exit
            if (result != true)
                return;

            // Get selected file path and directory
            string baseFilePath = saveFileDialog.FileName;
            string baseFileName = Path.GetFileNameWithoutExtension(baseFilePath);
            string directory = Path.GetDirectoryName(baseFilePath);

            try
            {
                // Save the main text file
                File.WriteAllText(baseFilePath, textBox.Text);

                // Save key file if it has content
                if (!string.IsNullOrWhiteSpace(keyBox.Text))
                {
                    string keyFilePath = Path.Combine(directory, baseFileName + "_key.txt");
                    File.WriteAllText(keyFilePath, keyBox.Text);
                }

                // Save encrypted text file if it has content
                if (!string.IsNullOrWhiteSpace(sypheredBox.Text))
                {
                    string sypheredFilePath = Path.Combine(directory, baseFileName + "_sypheredtext.txt");
                    File.WriteAllText(sypheredFilePath, sypheredBox.Text);
                }

                MessageBox.Show("Files saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving file: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}