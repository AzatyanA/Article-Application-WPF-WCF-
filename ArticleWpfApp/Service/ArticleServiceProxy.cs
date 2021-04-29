
using ArticleLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ArticleWpfApp.Service
{
    class ArticleServiceProxy : DuplexClientBase<IArticleService>
    {
        public ArticleServiceProxy(IArticleServiceCallback callbackInstance) : base(callbackInstance)
        { }
    }
}
