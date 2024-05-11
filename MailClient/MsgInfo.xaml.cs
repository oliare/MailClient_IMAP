using MimeKit;
using System.Windows;

namespace MailClient
{
    public partial class MsgInfo : Window
    {
        public MsgInfo(MimeMessage message)
        {
            InitializeComponent();

            senderBox.Text = message.From?.ToString() ?? "-";
            recieverBox.Text = message.To?.ToString() ?? "-";
            dateBox.Text = message.Date.ToString() ?? "-";
            themeBox.Text = message.Subject ?? "-";
            bodyBox.Text = message.Body?.ToString() ?? "-";
            filesBox.Text = message.Attachments?.ToString() ?? "-";
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }

}
