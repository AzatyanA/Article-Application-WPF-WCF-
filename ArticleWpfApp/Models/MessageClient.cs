using ArticleLibrary;
using ArticleWpfApp.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace ArticleWpfApp.Models
{  
    class MessageClient : CommonBase
    {
        private Guid id;
        private string author;
        private string content;
        private DateTime date;
        private List<MessageClient> comments;
        private bool isReplyModeOn;

        public Guid Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Author
        {
            get
            {
                return author;
            }
            set
            {
                author = value;
                OnPropertyChanged("IdAuthor");
            }
        }
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
                OnPropertyChanged("Content");
            }
        }
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }
        public List<MessageClient> Comments
        {
            get
            {
                return comments;
            }
            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }
        public bool IsReplyModeOn
        {
            get
            {
                return isReplyModeOn;
            }
            set
            {
                isReplyModeOn = value;
                OnPropertyChanged("IsReplyModeOn");
            }
        }
    }
    class MessageToMessageClient 
    {
        public List<MessageClient> Convert(List<Message> col)
        {
            List<MessageClient> newCol = new List<MessageClient>();
            foreach (var item in col)
            {
                var newItem = new MessageClient
                {
                    Id = item.Id,
                    Author = item.Author,
                    Content = item.Content,
                    Date = item.Date
                };

                if (item.Comments != null)
                {                  
                    newItem.Comments = Convert(item.Comments);
                }

                newCol.Add(newItem);
            }
            return newCol;
        }
    }
}
