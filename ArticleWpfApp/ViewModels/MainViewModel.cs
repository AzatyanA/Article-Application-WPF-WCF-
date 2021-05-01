using ArticleLibrary;
using ArticleWpfApp.Common;
using ArticleWpfApp.Infrastructure;
using ArticleWpfApp.Models;
using ArticleWpfApp.Service;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;

namespace ArticleWpfApp.ViewModels
{
    class MainViewModel : ViewModelBase
    {

        #region private fields
        private ArticleServiceProxy _articleServiceProxy;
        private IArticleService _serviceChannel;

        private string _userName = string.Empty;
        private string _messageContent = string.Empty;
        private string _replyContent=string.Empty;
        private MessageClient _checkedComment;
        private Guid _userId;
        private bool _logonSucceed;
        private string _error=string.Empty;
        private ObservableCollection<MessageClient> _messagesColl;
        #endregion

        #region properties
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                OnPropertyChanged("CurrentUser");
            }
        }
        public string MessageContent
        {
            get
            {
                return _messageContent;
            }
            set
            {
                _messageContent = value;
                OnPropertyChanged("MessageContent");
            }
        }
        public string ReplyContent
        {
            get
            {
                return _replyContent;
            }
            set
            {
                _replyContent = value;
                OnPropertyChanged("ReplyContent");
            }
        }
        public MessageClient CheckedComment
        {
            get
            {
                return _checkedComment;
            }
            set
            {
                _checkedComment = value;
                OnPropertyChanged("CheckedComment");
            }
        }
        public ObservableCollection<MessageClient> MessagesColl
        {
            get
            {
                return _messagesColl;
            }
            set
            {
                _messagesColl = value;
                OnPropertyChanged("MessagesColl");
            }
        }
        public bool LogonSucceed
        {
            get
            {
                return _logonSucceed;
            }
            set
            {
                _logonSucceed = value;
                OnPropertyChanged("LogonSucceed");
            }
        }
        public string Error
        {
            get
            {
                return _error;
            }
            set
            {
                _error = value;
                OnPropertyChanged("Error");
            }
        }
        public string Article { get; set; }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            var articleService = new ArticleServiceCallback();
            articleService.MessageCollectionStrReceivedEvent += ArticleService_MessageCollectionStrReceivedEvent; ;

            _articleServiceProxy = new ArticleServiceProxy(articleService);
            _serviceChannel = _articleServiceProxy.ChannelFactory.CreateChannel();

            Article = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Article.txt");

            var colStr = _serviceChannel.GetMessagesColStr();
            var  mesList =GetColFromStr.GetColFromString(colStr);
            MessageToMessageClient converter = new MessageToMessageClient();

            MessagesColl = new ObservableCollection<MessageClient>(converter.Convert(mesList));
        }

        private void ArticleService_MessageCollectionStrReceivedEvent(string message)
        {
            var mesList = GetColFromStr.GetColFromString(message);
            MessageToMessageClient converter = new MessageToMessageClient();
            MessagesColl = new ObservableCollection<MessageClient>(converter.Convert(mesList));
        }
        #endregion

        #region logon
        RelayCommand logonCommand;
        public ICommand Logon
        {
            get
            {
                if (logonCommand == null)
                    logonCommand = new RelayCommand(ExecuteLogonCommand, CanExecuteLogonCommand);

                return logonCommand;
            }
        }

        private bool CanExecuteLogonCommand(object obj)
        {
            return UserName.Trim().Length > 0;
        }

        private void ExecuteLogonCommand(object obj)
        {
            try
            {
                _userId = _serviceChannel.Logon(UserName);
                LogonSucceed = true;
                Error = string.Empty;
            }
            catch (Exception)
            {
                Error="Failed to logon. Make sure the server application is running.";
                return;
            }
        }
        #endregion

        #region logoff
        RelayCommand logoffCommand;
        public ICommand Logoff
        {
            get
            {
                if (logoffCommand == null)
                    logoffCommand = new RelayCommand(ExecuteLogoffCommand, CanExecuteLogoffCommand);

                return logoffCommand;
            }
        }

        private bool CanExecuteLogoffCommand(object obj)
        {
            return true;
        }

        private void ExecuteLogoffCommand(object obj)
        {         
            try
            {
                _serviceChannel.Logoff(_userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region send message
        RelayCommand sendMessageCommand;
        public ICommand SendMessage
        {
            get
            {
                if (sendMessageCommand == null)
                    sendMessageCommand = new RelayCommand(ExecuteSendMessageCommand, CanExecuteSendMessageCommand);

                return sendMessageCommand;
            }
        }

        private bool CanExecuteSendMessageCommand(object obj)
        {
            return  LogonSucceed && MessageContent.Trim().Length > 0;
        }

        private void ExecuteSendMessageCommand(object obj)
        {
            try
            {
                var Message = new Message { Author = UserName, Content = MessageContent, Date = DateTime.Now };
                _serviceChannel.SendMessage(Message);
                MessageContent = string.Empty;
                Error = string.Empty;
            }
            catch (Exception ex)
            {
                Error = $"Failed send message. {ex}";
            }
        }
        #endregion

        #region reply
        RelayCommand replyCommand;
        public ICommand Reply
        {
            get
            {
                if (replyCommand == null)
                    replyCommand = new RelayCommand(ExecuteReplyCommand, CanExecuteReplyCommand);

                return replyCommand;
            }
        }

        private bool CanExecuteReplyCommand(object obj)
        {
            return LogonSucceed;
        }

        private void ExecuteReplyCommand(object obj)
        {
            if (obj != null)
            {
                var ent = obj as MessageClient;
                if (ent != null)
                {
                    if (ent.Author==UserName)
                    {
                        Error = "Sorry, you can't reply on your own comments.";
                        return;
                    }
                    if (CheckedComment != null)
                    {
                        if (CheckedComment.IsReplyModeOn) CheckedComment.IsReplyModeOn = false;
                    }
                    CheckedComment = ent;
                    CheckedComment.IsReplyModeOn = !CheckedComment.IsReplyModeOn;
                    ReplyContent = string.Empty;
                    Error = string.Empty;
                }
                else
                {
                    Error = "Unknown exception";                   
                }
            }
            else
            {
                Error = "Message is null";    
            }
        }
        #endregion

        #region send reply
        RelayCommand sendReplyCommand;
        public ICommand SendReply
        {
            get
            {
                if (sendReplyCommand == null)
                    sendReplyCommand = new RelayCommand(ExecuteSendReplyCommand, CanExecuteSendReplyCommand);

                return sendReplyCommand;
            }
        }

        private bool CanExecuteSendReplyCommand(object obj)
        {
            return ReplyContent.Trim().Length>0 ;
        }

        private void ExecuteSendReplyCommand(object obj)
        {
            try
            {
                var reply = new Message { Author = UserName, Date = DateTime.Now, Content = ReplyContent };
                _serviceChannel.SendReplyMessage(reply, CheckedComment.Id);
                CheckedComment.IsReplyModeOn = false;
                ReplyContent = string.Empty;
                Error = string.Empty;
            }
            catch (Exception ex)
            {
                Error = $"Failed send reply. {ex}";
            }
        }
        #endregion
    }
       
}
