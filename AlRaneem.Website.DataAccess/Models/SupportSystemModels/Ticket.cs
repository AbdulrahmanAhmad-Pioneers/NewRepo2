﻿
namespace AlRaneem.Website.DataAccess.Models.SupportSystemModels
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? StatusId { get; set; }
        public Lookup Status { get; set; } = default!;
        public int? PriorityId { get; set; }
        public Lookup Priority { get; set; } = default!;
        public int? SupportTypeId { get; set; }
        public Lookup SupportType { get; set; } = default!;
        public int? CategoryId { get; set; }
        public Lookup Category { get; set; } = default!;
        public int? SubcategoryId { get; set; }
        public Lookup? Subcategory { get; set; }
        public int? AssignedToId { get; set; }
        public UserRole? AssignedTo { get; set; }
        public int? CreatedById { get; set; }
        public UserRole? CreatedBy { get; set; }
    }
}
