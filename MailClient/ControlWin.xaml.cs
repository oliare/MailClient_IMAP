using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MailClient
{

    public partial class ControlWin : Window
    {
        private string gmail;
        private string password;
        private ImapClient client;
        public ObservableCollection<string> folders { get; set; }
        public ObservableCollection<MimeMessage> messages { get; set; }

        public ControlWin(string gmail, string password)
        {
            InitializeComponent();

            this.gmail = gmail;
            this.password = password;
            accountBlock.Text = "Account:  " + gmail;

            client = new ImapClient();
            try
            {
                client.Connect("imap.gmail.com", 993, MailKit.Security.SecureSocketOptions.SslOnConnect);
                client.Authenticate(gmail, password);
            }
            catch (Exception)
            {
                MessageBox.Show("User with such data doesn`t exist . . .");
                return;
            }

            folders = new ObservableCollection<string>();
            messages = new ObservableCollection<MimeMessage>();

            GetFolders();
            DataContext = this;
        }
        private void GetFolders()
        {
            var dirs = client.GetFolders(client.PersonalNamespaces[0]);
            foreach (var dir in dirs)
                folders.Add(dir.Name);
        }

        private async Task GetMessagesAsync()
        {
            try
            {
                var inbox = client.GetFolder(SpecialFolder.All);
                await inbox.OpenAsync(FolderAccess.ReadOnly);

                messages.Clear();

                var uids = await inbox.SearchAsync(SearchQuery.All);
                foreach (var uid in uids)
                {
                    var message = await inbox.GetMessageAsync(uid);

                    var msgFrom = message.From.Mailboxes.FirstOrDefault(); 
                    if (msgFrom != null)
                    {
                        var mailbox = new MailboxAddress(msgFrom.Name, msgFrom.Address);
                        var mime = new MimeMessage
                        {
                            From = { mailbox },
                            Subject = message.Subject,
                            Date = message.Date
                        };
                        messages.Add(mime);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var win = new SendMsg(gmail, password);
            win.Show();
            this.Close();
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Label)sender).Foreground = Brushes.DarkGray;
        }
        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Label)sender).Foreground = Brushes.Black;
        }

        private void Messages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                var msg = e.AddedItems[0] as MimeMessage;
                if (msg != null)
                {
                    var win = new MsgInfo(msg);
                    win.Show();
                }
            }
        }

        private async void Folders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                messages.Clear();
                await GetMessagesAsync();
            }
        }
    }

}


