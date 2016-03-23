using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NewsPortal.Domain.Security;

namespace NewsPortal.Web.Models
{
    public class LoginDataModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите имя пользователя")]
        [Display(Name = "Имя пользователя")]
        public string UserName
        {
            get;
            set;
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password
        {
            get;
            set;
        }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe
        {
            get;
            set;
        }
    }
}