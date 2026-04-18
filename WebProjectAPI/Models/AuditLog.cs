using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebProjectAPI.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Action {get;set;}
        public string TableName { get; set;}
        public string OldValue { get;set; }
        public string NewValue {  get;set; }
        public DateTime? CreatedAt { get; set; } 


    }
}
