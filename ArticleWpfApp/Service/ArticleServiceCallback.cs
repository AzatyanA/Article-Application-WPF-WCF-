using ArticleLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleWpfApp.Service
{
    class ArticleServiceCallback : IArticleServiceCallback
    {
        public delegate void MessageCollectionStrReceivedDelegate(string message);
        public event MessageCollectionStrReceivedDelegate MessageCollectionStrReceivedEvent;

        public delegate void NewMessageReceivedDelegate(Message message);
        public event NewMessageReceivedDelegate NewMessageReceivedEvent;

        public void ReceiveMessageCollectionStr(string message)
        {
            MessageCollectionStrReceivedEvent(message);
        }

        public void ReceiveNewMessage(Message message)
        {
            NewMessageReceivedEvent(message);
        }

    }
}
