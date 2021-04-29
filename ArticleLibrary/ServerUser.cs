using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ArticleLibrary
{
    [DataContract]
    class ServerUser
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string UserName { get; set; }
        public OperationContext OperationContext { get; set; }
        [IgnoreDataMember]
        public IArticleServiceCallback CallbackChannel { get; set; }
    }
}
