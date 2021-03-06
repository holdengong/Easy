﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        public T OpsBeforeAdd<T>(HttpContext httpContext)
            where T : BaseEntity
        {
            CreatedOn = DateTime.Now;
            UpdatedOn = DateTime.Now;
            CreatedBy = httpContext?.User?.Claims
                .FirstOrDefault(_ => _.Type.Equals("userid", StringComparison.CurrentCultureIgnoreCase))
                ?.Value;
            return this as T;
        }

        public T OpsBeforeUpdate<T>(HttpContext httpContext)
            where T:BaseEntity
        {
            UpdatedOn = DateTime.Now;
            UpdatedBy = httpContext?.User?.Claims
                .FirstOrDefault(_ => _.Type.Equals("userid", StringComparison.CurrentCultureIgnoreCase))
                ?.Value;
            return this as T;
        }
    }
}
