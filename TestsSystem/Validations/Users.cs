using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsSystem.Models
{
    public partial class Users : IDataErrorInfo
    {
        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;

                switch (columnName)
                {
                    case "Login":
                        if (!UsersService.SameLogin(Login))
                        {
                            error = "Такого логина нет!";
                        }
                        break;

                        //case "Password":
                        //    if (Password.Length < 6)
                        //        error = "Пароль слишком маленький!";
                        //    break;
                }

                return error;
            }
        }

        public string Error => throw new NotImplementedException();
    }
}
