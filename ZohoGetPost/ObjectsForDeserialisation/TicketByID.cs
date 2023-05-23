using Newtonsoft.Json;
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
            public string photoURL { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string id { get; set; }
            public string email { get; set; }
        }

        public class Cf
        {
            public string cf_software_issue { get; set; }
            public object cf_installation_type { get; set; }
            public string cf_directories_cleared_or_not_required { get; set; }
            public object cf_picklist_import_values_test { get; set; }
            public object cf_partner_improvement { get; set; }
            public object cf_developer_estimated_hours { get; set; }
            public string cf_version { get; set; }
            public object cf_tfs_aha_devops_url { get; set; }
            public string cf_complexity_of_issue { get; set; }
            public string cf_product_team { get; set; }
            public object cf_resolve_by { get; set; }
            public object cf_multiselect_1 { get; set; }
            public object cf_developers_involved { get; set; }
        }

        public class Contact
        {
            public string firstName { get; set; }
            public string lastName { get; set; }
            public object phone { get; set; }
            public object mobile { get; set; }
            public string id { get; set; }
            public bool isSpam { get; set; }
            public object type { get; set; }
            public string email { get; set; }
            public Account account { get; set; }
        }

        public class CustomFields
        {
            [JsonProperty("Developers Involved")]
            public object DevelopersInvolved { get; set; }

            [JsonProperty("Partner Improvement")]
            public object PartnerImprovement { get; set; }

            [JsonProperty("Software Issue")]
            public string SoftwareIssue { get; set; }

            [JsonProperty("Installation Type")]
            public object InstallationType { get; set; }

            [JsonProperty("Picklist Import Values Test")]
            public object PicklistImportValuesTest { get; set; }

            [JsonProperty("TFS/Aha/Devops URL")]
            public object TFSAhaDevopsURL { get; set; }

            [JsonProperty("Software Version")]
            public string SoftwareVersion { get; set; }

            [JsonProperty("Resolve By")]
            public object ResolveBy { get; set; }

            [JsonProperty("Product Team")]
            public string ProductTeam { get; set; }

            [JsonProperty("Developer Estimated Hours")]
            public object DeveloperEstimatedHours { get; set; }

            [JsonProperty("Multiselect 1")]
            public object Multiselect1 { get; set; }

            [JsonProperty("Directories Cleared")]
            public string DirectoriesCleared { get; set; }

            [JsonProperty("Complexity of Issue")]
            public string ComplexityofIssue { get; set; }
        }

        public class Department
        {
            public string name { get; set; }
            public string id { get; set; }
        }

        public class LayoutDetails
        {
            public string id { get; set; }
            public string layoutName { get; set; }
        }

        public class Root
        {
            public DateTime modifiedTime { get; set; }
            public string subCategory { get; set; }
            public string statusType { get; set; }
            public string subject { get; set; }
            public object dueDate { get; set; }
            public string departmentId { get; set; }
            public string channel { get; set; }
            public DateTime onholdTime { get; set; }
            public string language { get; set; }
            public Source source { get; set; }
            public object resolution { get; set; }
            public List<object> sharedDepartments { get; set; }
            public object closedTime { get; set; }
            public string approvalCount { get; set; }
            public bool isOverDue { get; set; }
            public bool isTrashed { get; set; }
            public Contact contact { get; set; }
            public DateTime createdTime { get; set; }
            public string id { get; set; }
            public bool isResponseOverdue { get; set; }
            public DateTime customerResponseTime { get; set; }
            public object productId { get; set; }
            public string contactId { get; set; }
            public string threadCount { get; set; }
            public List<object> secondaryContacts { get; set; }
            public string priority { get; set; }
            public string classification { get; set; }
            public string commentCount { get; set; }
            public string taskCount { get; set; }
            public string accountId { get; set; }
            public object phone { get; set; }
            public string webUrl { get; set; }
            public Assignee assignee { get; set; }
            public bool isSpam { get; set; }
            public string status { get; set; }
            public List<object> entitySkills { get; set; }
            public string ticketNumber { get; set; }
            public object sentiment { get; set; }
            public CustomFields customFields { get; set; }
            public bool isArchived { get; set; }
            public string description { get; set; }
            public string timeEntryCount { get; set; }
            public object channelRelatedInfo { get; set; }
            public object responseDueDate { get; set; }
            public bool isDeleted { get; set; }
            public string modifiedBy { get; set; }
            public Department department { get; set; }
            public string followerCount { get; set; }
            public string email { get; set; }
            public LayoutDetails layoutDetails { get; set; }
            public object channelCode { get; set; }
            public object product { get; set; }
            public bool isFollowing { get; set; }
            public Cf cf { get; set; }
            public string slaId { get; set; }
            public object team { get; set; }
            public string layoutId { get; set; }
            public string assigneeId { get; set; }
            public string createdBy { get; set; }
            public object teamId { get; set; }
            public string tagCount { get; set; }
            public string attachmentCount { get; set; }
            public bool isEscalated { get; set; }
            public string category { get; set; }
        }

        public class Source
        {
            public object appName { get; set; }
            public object extId { get; set; }
            public object permalink { get; set; }
            public string type { get; set; }
            public object appPhotoURL { get; set; }
        }
    }
}
