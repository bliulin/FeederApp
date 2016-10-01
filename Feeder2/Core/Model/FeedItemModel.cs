using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feeder.DataModel
{
    public class FeedItemModel
    {
        public string Id
        {
            get;
            set;
        }

        public string ImageUri
        {
            get;
            set;
        }

        public DateTime PublishDate
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Summary
        {
            get;
            set;
        }

        public string ShortDescription { get; set; }        

        public string ItemUri
        {
            get;
            set;
        }

        public string Author
        {
            get;
            set;
        }

        public bool IsRead
        {
            get; set;
        }
    }
}
