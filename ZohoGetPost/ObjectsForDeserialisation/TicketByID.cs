using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZohoGetPost.ObjectsForDeserialisation
{

    //obj for ticket info, scope: Desk.Tickets.READ

    public class TicketByID
    {
        public class Account
        {
            public string website { get; set; }
            public string accountName { get; set; }
            public string id { get; set; }
        }

        public class Assignee
        {
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string photoURL { get; set; }
            public string id { get; set; }
            public string email { get; set; }
        }

        public class Cf
        {
            public object cf_permanentaddress { get; set; }
            public object cf_dateofpurchase { get; set; }
            public object cf_phone { get; set; }
            public object cf_numberofitems { get; set; }
            public object cf_url { get; set; }
            public object cf_secondaryemail { get; set; }
            public string cf_severitypercentage { get; set; }
            public string cf_modelname { get; set; }
        }

        public class ChannelRelatedInfo
        {
            public string topicId { get; set; }
            public bool isTopicDeleted { get; set; }
            public string forumStatus { get; set; }
            public string sourceLink { get; set; }
            public string topicType { get; set; }
        }

        public class Contact
        {
            public string lastName { get; set; }
            public string firstName { get; set; }
            public string phone { get; set; }
            public string mobile { get; set; }
            public string id { get; set; }
            public bool isSpam { get; set; }
            public object type { get; set; }
            public string email { get; set; }
            public Account account { get; set; }
        }

        public class Department
        {
            public string name { get; set; }
            public string id { get; set; }
        }

        public class Root
        {
            public DateTime modifiedTime { get; set; }
            public string subCategory { get; set; }
            public string statusType { get; set; }
            public string subject { get; set; }
            public DateTime dueDate { get; set; }
            public string departmentId { get; set; }
            public string channel { get; set; }
            public object onholdTime { get; set; }
            public string language { get; set; }
            public Source source { get; set; }
            public object resolution { get; set; }
            public List<SharedDepartment> sharedDepartments { get; set; }
            public object closedTime { get; set; }
            public string sharedCount { get; set; }
            public string approvalCount { get; set; }
            public bool isOverDue { get; set; }
            public bool isTrashed { get; set; }
            public Contact contact { get; set; }
            public DateTime createdTime { get; set; }
            public string id { get; set; }
            public DateTime customerResponseTime { get; set; }
            public object productId { get; set; }
            public string contactId { get; set; }
            public string threadCount { get; set; }
            public List<string> secondaryContacts { get; set; }
            public string priority { get; set; }
            public object classification { get; set; }
            public string commentCount { get; set; }
            public string taskCount { get; set; }
            public string phone { get; set; }
            public string webUrl { get; set; }
            public bool isSpam { get; set; }
            public Assignee assignee { get; set; }
            public string status { get; set; }
            public List<string> entitySkills { get; set; }
            public string ticketNumber { get; set; }
            public bool isRead { get; set; }
            public string description { get; set; }
            public string timeEntryCount { get; set; }
            public ChannelRelatedInfo channelRelatedInfo { get; set; }
            public string isDeleted { get; set; }
            public Department department { get; set; }
            public string followerCount { get; set; }
            public string email { get; set; }
            public object channelCode { get; set; }
            public object product { get; set; }
            public Cf cf { get; set; }
            public string isFollowing { get; set; }
            public Team team { get; set; }
            public string assigneeId { get; set; }
            public string teamId { get; set; }
            public object contractId { get; set; }
            public string tagCount { get; set; }
            public string attachmentCount { get; set; }
            public bool isEscalated { get; set; }
            public string category { get; set; }
        }

        public class SharedDepartment
        {
            public string name { get; set; }
            public string id { get; set; }
            public string type { get; set; }
        }

        public class Source
        {
            public object appName { get; set; }
            public object extId { get; set; }
            public string type { get; set; }
            public object permalink { get; set; }
            public object appPhotoURL { get; set; }
        }

        public class Team
        {
            public string name { get; set; }
            public string id { get; set; }
        }
    }
}
