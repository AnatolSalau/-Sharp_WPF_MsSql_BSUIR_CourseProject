using Sebestoimost.Model;
using System.Linq;
using System.Windows;

namespace Sebestoimost
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            LstUsers.ItemsSource = App.db.Users.Where(p => p.Enabled).ToList();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (LstUsers.SelectedItem != null)
            {
                User user = LstUsers.SelectedItem as User;
                if (user.Password.Equals(App.GetMD5(FldPassword.Password)))
                {
                    App.SetLogText("Пользователь авторизован\t" + user.Name);
                    Hide();
                    App.user = user;
                    Main frm = new Main();
                    frm.ShowDialog();
                    App.SetLogText("Завершение работы\t" + user.Name);
                    Close();
                }
                else
                {
                    FldPassword.Password = "";
                    MessageBox.Show("Неверный пароль!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                    App.SetLogText("Ошибка авторизации\t" + user.Name);
                }
            }
            else
                MessageBox.Show("Необходимо выбрать пользователя!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
