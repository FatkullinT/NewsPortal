using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI.WebControls;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Web.Models
{
    public class NewsTextModel
    {

        public NewsTextModel(NewsText textRecord)
        {
            Text = textRecord.Text;
            NewsDate = textRecord.News.Date;
            NewsId = textRecord.News.Id;
        }

        public Guid NewsId
        {
            get;
            set;
        }

        public DateTime NewsDate
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }
    }
}