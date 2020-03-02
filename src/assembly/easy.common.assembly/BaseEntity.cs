﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Easy.Common.Assembly
{
    public class BaseEntity
    {
        [Key]
        [MaxLength(50)]
        public string Id { get; set; }
        public DateTime CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        [MaxLength(50)]
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        public T OpsBeforeAdd<T>()
            where T : BaseEntity
        {
            CreatedOn = DateTime.Now;
            return this as T;
        }

        public T OpsBeforeUpdate<T>()
            where T:BaseEntity
        {
            UpdatedOn = DateTime.Now;
            return this as T;
        }
    }
}
