using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Web.Models
{
    public class LanguageLinkData
    {
        public Language Language { get; set; }
        public string DisplayName { get; set; }
        public bool Selected { get; set; }
    }
}