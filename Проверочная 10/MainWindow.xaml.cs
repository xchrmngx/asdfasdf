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
using Word = Microsoft.Office.Interop.Word;

namespace Проверочная_10
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        КнижкиEntities3 db = new КнижкиEntities3();
        List<Книги_> Books;
        List<Заказы> Orders;
        bool IsCanCalcul = true;
        double Price = 0, FullPrice = 0;

        public MainWindow()
        {
            InitializeComponent();

            Books = db.Книги_.ToList();

            for (int i = 0; i < Books.Count; i++)
            {
                WrapPanel wp = new WrapPanel();
                System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                Label lb = new Label();

                wp.Height = 200;
                wp.Width = 100;

                lb.Content = Books[i].Название.ToString();

                string savePath = "C:\\Users\\nvidi\\OneDrive\\Рабочий стол\\Проверочная 10\\Проверочная 10\\res\\Image";
                savePath = savePath + "\\" + Books[i].Изображение.ToString() + ".jpg";
                BitmapImage bm = new BitmapImage();
                bm.BeginInit();
                bm.UriSource = new Uri(savePath);
                bm.EndInit();
                img.Source = bm;

                img.MouseDown += new MouseButtonEventHandler(MyImage_MouseDown);

                img.Height = 150;
                img.Width = 100;

                img.Uid = Books[i].Код.ToString();

                wp.Children.Add(img);
                wp.Children.Add(lb);
                listView.Items.Add(wp);
            }
        }

        private void MyImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point mousePoint = Mouse.GetPosition(this);
            IInputElement element = InputHitTest(mousePoint);
            string elementName = (element as FrameworkElement)?.Uid;

            int id = Convert.ToInt32(elementName);

            var book = Books.Find(x => x.Код == id);
            string savePath = "C:\\Users\\nvidi\\OneDrive\\Рабочий стол\\Проверочная 10\\Проверочная 10\\res\\Image";
            savePath = savePath + "\\" + book.Изображение.ToString() + ".jpg";
            BitmapImage bm = new BitmapImage();
            bm.BeginInit();
            bm.UriSource = new Uri(savePath);
            bm.EndInit();
            imgBook.Source = bm;
            Price = Convert.ToDouble(book.Цена);
            BookName.Text = book.Название.ToString();
            ContentBook.Text = book.Описание.ToString();

            if (IsCanCalcul)
            {
                FullPrice = Price * Convert.ToDouble(CountBook.Text);
                FullCoast.Content = "К оплате: " + FullPrice;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) // Минус к количеству
        {
            if (IsCanCalcul && Convert.ToDouble(CountBook.Text) > 0)
            {
                double count = Convert.ToDouble(CountBook.Text);
                count--;
                CountBook.Text = count.ToString();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) //Плюс к количеству
        {
            if (IsCanCalcul)
            {
                double count = Convert.ToDouble(CountBook.Text);
                count++;
                CountBook.Text = count.ToString();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) 
        {
            try
            {
                double count = Convert.ToDouble(CountBook.Text);
                FullPrice = Price * count;
                IsCanCalcul = true;
                FullCoast.Content = "К оплате: " + FullPrice;
            }
            catch
            {
                IsCanCalcul = false;
                FullCoast.Content = "Введите количество коректно!";
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) //Купить
        {
            Orders = db.Заказы.ToList();
            var order = new Заказы
            {
                Название = BookName.Text,
                Количество = Convert.ToInt32(CountBook.Text),
                Итог = Convert.ToInt32(FullPrice),
                Дата = DateTime.Today
            };
            Orders.Add(order);
            db.Заказы.Add(order);
            db.SaveChanges();

            var WordApp = new Word.Application();
            WordApp.Visible = false;
            var Worddoc = WordApp.Documents.Open(Environment.CurrentDirectory +
            @"\Чек.docx");

            Repwo("{Код}", order.Код.ToString(), Worddoc);
            Repwo("{Дата}", DateTime.Now.ToString(), Worddoc);
            Repwo("{Название книги}", order.Название.ToString(), Worddoc);
            Repwo("{Количество}", order.Количество.ToString(), Worddoc);
            Repwo("{Итог}", order.Итог.ToString(), Worddoc);

            Worddoc.SaveAs2(Environment.CurrentDirectory + $@"\Чек{Orders.Count}.docx");
            MessageBox.Show("Билет сохранен!");
        }

        private void Repwo(string subToReplace, string text, Word.Document worddoc)
        {
            var range = worddoc.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: subToReplace, ReplaceWith: text);
        }
    }
}
