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
            var mes = MessagesCol.FirstOrDefault(x => x.Id == id);
            if (mes != null)
            {
                if (mes.Comments == null)
                {
                    mes.Comments = new List<Message>();
                }
                mes.Comments.Add(message);

                foreach (var user in Users)
                {
                    user.OperationContext.GetCallbackChannel<IArticleServiceCallback>().ReceiveMessageCollectionStr(this.GetMessagesColStr());
                }
            }
        }

    }
}
