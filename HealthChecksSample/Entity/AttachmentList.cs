using System;
using System.Collections.Generic;

namespace HealthChecksSample.Entity
{
    public partial class AttachmentList
    {
        public long Id { get; set; }
        public string AttachmentGuid { get; set; }
        public byte[] AttachmentData { get; set; }
        public long? AttachmentSize { get; set; }
        public string AttachmentDesc { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsOrphaned { get; set; }
    }
}
