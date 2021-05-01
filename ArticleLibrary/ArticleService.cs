using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;

namespace ArticleLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ArticleService : IArticleService
    {
        private List<Message> MessagesCol=new List<Message>();
        private List<ServerUser> Users = new List<ServerUser>();

      
        public Guid Logon(string userName)
        {
            var newUser = new ServerUser { Id=Guid.NewGuid(),UserName = userName, OperationContext = OperationContext.Current, CallbackChannel= OperationContext.Current.GetCallbackChannel<IArticleServiceCallback>() };
            Users.Add(newUser);
            return newUser.Id;
        }

        public void Logoff(Guid userId)
        {
            var user = Users.FirstOrDefault(u=>u.Id==userId);
            if (user != null)
                Users.Remove(user);         
        }
      
        public string GetMessagesColStr()
        {
            //if (MessagesCol.Count == 0)
            //{
            //    var com1 = new Message { Id=Guid.NewGuid(), Author = "Artyom", Content = "This is my comment", Date = DateTime.Now };
            //    var com2 = new Message { Id = Guid.NewGuid(), Author = "Victor", Content = "It's great!", Date = DateTime.Now };

            //    com1.Comments = new List<Message>();
            //    com1.Comments.Add(com2);

            //    var com3 = new Message { Id = Guid.NewGuid(), Author = "New", Content = "wow!", Date = DateTime.Now };
            //    com2.Comments = new List<Message>();
            //    com2.Comments.Add(com3);

            //    var com4 = new Message { Id = Guid.NewGuid(), Author = "Forth", Content = "Weee!", Date = DateTime.Now };
            //    com3.Comments = new List<Message>();
            //    com3.Comments.Add(com4);

            //    MessagesCol.Add(com1);
            //}
            XmlSerializer serializer = new XmlSerializer(typeof(List<Message>));

            var stringwriter = new System.IO.StringWriter();
            serializer.Serialize(stringwriter, MessagesCol);

            return stringwriter.ToString();
        }

        public void SendMessage(Message message)
        {
            message.Id = Guid.NewGuid();
            MessagesCol.Add(message);
            foreach (var user in Users)
            {
                user.OperationContext.GetCallbackChannel<IArticleServiceCallback>().ReceiveMessageCollectionStr(this.GetMessagesColStr());
            }
        }

        public void SendReplyMessage(Message message, Guid id)
        {
            ReturnMessage(MessagesCol, id);
            if (checkedMessage != null)
            {
                message.Id = Guid.NewGuid();
                if (checkedMessage.Comments == null) checkedMessage.Comments = new List<Message>();
                checkedMessage.Comments.Add(message);

                foreach (var user in Users)
                {
                    user.OperationContext.GetCallbackChannel<IArticleServiceCallback>().ReceiveMessageCollectionStr(this.GetMessagesColStr());
                }
                checkedMessage = null;
            }
        }
        private Message checkedMessage;

        private void ReturnMessage(List<Message> list, Guid id)
        {
            foreach (var item in list)
            {
                if (item.Id == id)
                {
                    checkedMessage = item;
                    return;
                }
                if (item.Comments != null)
                {
                    ReturnMessage(item.Comments, id);
                }
            }
        }

    }
}
