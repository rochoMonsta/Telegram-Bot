using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace RochoBot2
{
    public class TelegramUser : INotifyPropertyChanged, IEquatable<TelegramUser>
    {
        public TelegramUser(string NickName, long ChatID)
        {
            this.nick = NickName;
            this.id = ChatID;
            Messages = new ObservableCollection<string>();
        }
        private string nick;
        private long id;
        public string Nick
        {
            get { return this.nick; }
            set
            {
                this.nick = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Nick)));
            }
        }
        public long Id
        {
            get { return this.id; }
            set
            {
                this.id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Id)));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public bool Equals(TelegramUser other) => other.Id == this.id;
        public ObservableCollection<string> Messages { get; set; }
        public void AddMessage(string text) => Messages.Add(text);
    }
}
