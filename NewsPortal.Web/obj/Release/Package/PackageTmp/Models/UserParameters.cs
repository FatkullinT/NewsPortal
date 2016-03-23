using System;
using System.ComponentModel.DataAnnotations;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Web.Models
{
    public class UserParameters
    {
        public UserParameters()
        {}

        public UserParameters(Account account)
        {
            Email = account.Email;
            SendNews = account.SendNews;
            Id = account.Id;
        }

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "¬ведите им€ адрес электронной почты")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "¬ключить рассылку")]
        public bool SendNews { get; set; }

        public Account ToUserDataRecord()
        {
            return new Account
            {
                Id = Id,
                Email = Email,
                SendNews = SendNews
            };
        }
    }
}