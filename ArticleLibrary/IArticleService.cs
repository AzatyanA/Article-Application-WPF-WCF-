using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ArticleLibrary
{
    [ServiceContract(CallbackContract = typeof(IArticleServiceCallback))]
    public interface IArticleService
    {
        [OperationContract]
        Guid Logon(string userName);
        [OperationContract]
        void Logoff(Guid userId);
        [OperationContract]
        string GetMessagesColStr();
        [OperationContract(IsOneWay = true)]
        void SendMessage(Message message);
        [OperationContract(IsOneWay = true)]
        void SendReplyMessage(Message message, Guid id);
    }

    [ServiceContract]
    public interface IArticleServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveNewMessage(Message message);
        [OperationContract(IsOneWay = true)]
        void ReceiveMessageCollectionStr(string mes);

    }
}
