using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Web.Models
{
    public class NewsUpdateModel
    {
        public NewsUpdateModel()
        {}

        public NewsUpdateModel(News newsRecord)
        {
            NewsDate = newsRecord.Date;
            NewsId = newsRecord.Id;
            AllowAnonymous = newsRecord.AllowAnonymous;
            NewsText englishText = newsRecord.NewsTexts.Single(text => text.Language == Language.Eng);
            EnglishText = englishText.Text;
            EnglishTextId = englishText.Id;
            NewsText russianText = newsRecord.NewsTexts.Single(text => text.Language == Language.Rus);
            RussianText = russianText.Text;
            RussianTextId = russianText.Id;
        }

        public Guid NewsId
        {
            get;
            set;
        }

        public Guid EnglishTextId
        {
            get;
            set;
        }

        public Guid RussianTextId
        {
            get;
            set;
        }

        [DisplayName("Дата выхода новости")]
        public DateTime NewsDate
        {
            get;
            set;
        }

        [DisplayName("Доступно незарегистрированным пользователям")]
        public bool AllowAnonymous
        {
            get;
            set;
        }

        [DisplayName("Текст на английском")]
        public string EnglishText
        {
            get;
            set;
        }

        [DisplayName("Текст на русском")]
        public string RussianText
        {
            get;
            set;
        }

        public News ToNewsRecord()
        {
            News newsRecord = new News
            {
                Date = NewsDate,
                AllowAnonymous = AllowAnonymous,
                Id = NewsId,
                NewsTexts = new []
                {
                    new NewsText()
                    {
                        Id = EnglishTextId,
                        Language = Language.Eng,
                        Text = EnglishText
                    },
                    new NewsText()
                    {
                        Id = RussianTextId,
                        Language = Language.Rus,
                        Text = RussianText
                    }
                }
            };
            return newsRecord;
        }
    }
}