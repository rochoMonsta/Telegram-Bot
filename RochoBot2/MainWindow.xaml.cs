using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using Telegram.Bot;

namespace RochoBot2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<TelegramUser> Users;
        TelegramBotClient bot;
        public MainWindow()
        {
            InitializeComponent();
            Users = new ObservableCollection<TelegramUser>();
            usersList.ItemsSource = Users;
            string token = File.ReadAllText(@"F:\All\Documents\3 семестр\C#\telegramConnection.txt", Encoding.UTF8).Replace("\n", "");

            bot = new TelegramBotClient(token);

            bot.OnMessage += delegate (object sender, Telegram.Bot.Args.MessageEventArgs e)
            {
                string msg = $"{DateTime.Now}: {e.Message.Chat.FirstName} {e.Message.Chat.Id} {e.Message.Text}";
                File.AppendAllText("data.log", $"{msg}\n");
                this.Dispatcher.Invoke(() =>
                {
                    var person = new TelegramUser(e.Message.Chat.FirstName, e.Message.Chat.Id);
                    if (!Users.Contains(person))
                        Users.Add(person);
                    Users[Users.IndexOf(person)].AddMessage($"{person.Nick}: {e.Message.Text}");
                });
            };
            bot.StartReceiving();
            txtBxSendMsg.KeyDown += (s, e) => { if (e.Key == Key.Return) { SendMsg(); } };
            btnSendMsg.Click += delegate { SendMsg(); };
        }
        public void SendMsg()
        {
            try
            {
                var concreteUser = Users[Users.IndexOf(usersList.SelectedItem as TelegramUser)];
                if (txtBxSendMsg.Text != String.Empty)
                {
                    string responseMsg = $"rocho: {txtBxSendMsg.Text}";
                    concreteUser.Messages.Add(responseMsg);

                    bot.SendTextMessageAsync(concreteUser.Id, txtBxSendMsg.Text);
                    string logText = $"\n{DateTime.Now}: >> {concreteUser.Id} {concreteUser.Nick} {responseMsg}";
                    File.AppendAllText("data.log", logText);
                    txtBxSendMsg.Text = String.Empty;
                }
            }
            catch
            {
                MessageBox.Show("You can't send message!");
            }
            finally
            {
                txtBxSendMsg.Text = String.Empty;
            }
        }
    }
}
