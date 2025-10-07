using System;

namespace Vanfist.DTOs.Responses
{
    public class TestDriveRequestItem
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public int? ModelId { get; set; }

        public string? ModelName { get; set; }   // để hiển thị tên xe

        public DateTime? PreferredTime { get; set; }

        public string Status { get; set; } = "New";

        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
