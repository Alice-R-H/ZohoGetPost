using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZohoGetPost.ObjectsForDeserialisation
{
    internal class AllTickets
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Account
        {
            public string accountName { get; set; }
            public string website { get; set; }
            public string id { get; set; }
        }

        public class Assignee
        {
            public string id { get; set; }
            public string email { get; set; }
            public string photoURL { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
        }

        public class Contact
        {
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string email { get; set; }
            public object mobile { get; set; }
            public string phone { get; set; }
            public object type { get; set; }
            public Account account { get; set; }
            public string id { get; set; }
        }

        public class Datum
        {
            public string id { get; set; }
            public string ticketNumber { get; set; }
            public string layoutId { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public string subject { get; set; }
            public string status { get; set; }
            public string statusType { get; set; }
            public DateTime createdTime { get; set; }
            public string category { get; set; }
            public string language { get; set; }
            public string subCategory { get; set; }
            public string priority { get; set; }
            public string channel { get; set; }
            public DateTime? dueDate { get; set; }
            public object responseDueDate { get; set; }
            public string commentCount { get; set; }
            public object sentiment { get; set; }
            public string threadCount { get; set; }
            public DateTime? closedTime { get; set; }
            public DateTime? onholdTime { get; set; }
            public string departmentId { get; set; }
            public string contactId { get; set; }
            public string productId { get; set; }
            public string assigneeId { get; set; }
            public object teamId { get; set; }
            public Department department { get; set; }
            public Contact contact { get; set; }
            public object team { get; set; }
            public Assignee assignee { get; set; }
            public Product product { get; set; }
            public object channelCode { get; set; }
            public string webUrl { get; set; }
            public bool isSpam { get; set; }
            public LastThread lastThread { get; set; }
            public DateTime customerResponseTime { get; set; }
            public bool isArchived { get; set; }
            public Source source { get; set; }
        }

        public class Department
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class LastThread
        {
            public string channel { get; set; }
            public bool isDraft { get; set; }
            public bool isForward { get; set; }
            public string direction { get; set; }
        }

        public class Product
        {
            public string productName { get; set; }
            public string id { get; set; }
        }

        public class Root
        {
            public List<Datum> data { get; set; }
        }

        public class Source
        {
            public object appName { get; set; }
            public object appPhotoURL { get; set; }
            public object permalink { get; set; }
            public string type { get; set; }
            public object extId { get; set; }
        }
    }
}
