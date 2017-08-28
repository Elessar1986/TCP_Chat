namespace TCPServer_DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Messages
    {
        [Key]
        public long MessageID { get; set; }

        public long? FromID { get; set; }

        public long? ToId { get; set; }

        public DateTime? Time { get; set; }

        [StringLength(1000)]
        public string Message { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
