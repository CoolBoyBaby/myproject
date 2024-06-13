using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace manuu
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Page
    {
        public Auth()
        {
            InitializeComponent();
        }
        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Loginer_Click(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text == "user" && Pas.Password == "654321")
            {
                Manager.MainFrame.Navigate(new MainWindow());
                return;
            }

            int Try = 3;
            if (Try != 0)
            {
                try
                {
                    var search_user = for_restoranEntities.GetContext().Dolj.Where(p => p.login == txtUsername.Text && p.password == Pas.Password).Single();

                    if (txtUsername.Text == "boss" && Pas.Password == "123456")
                    {
                        Manager.MainFrame.Navigate(new MainWindow());
                    }

                    else
                        Console.WriteLine("Пароль");

                    Console.WriteLine($"{search_user.login}");
                }
                catch
                {
                    MessageBox.Show("Ошибка, что-то неправильно");
                }

            }

        }
    }
}
