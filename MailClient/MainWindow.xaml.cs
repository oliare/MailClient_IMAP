using System.Windows;

namespace MailClient
{

    public partial class MainWindow : Window
    {

        private string gmail;
        private string password;
        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(string gmail, string password)
        {
            this.gmail = gmail;
            this.password = password;
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            string gmail = gmailTextBox.Text;
            string password = passwordBox.Password;
            if (string.IsNullOrEmpty(gmail) || string.IsNullOrEmpty(password) || gmail.Contains(" "))
            {
                MessageBox.Show("Enter the correct data in the fields");
            }
            else
            {
                var win = new ControlWin(gmail, password);
                win.Show();
                this.Close();
            }
        }
    }
}
