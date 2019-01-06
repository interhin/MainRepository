using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TestsSystem.Validations
{
    class EmptyRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var val = value.ToString();
            string errorMessage = String.Empty;
            if (String.IsNullOrWhiteSpace(val))
                errorMessage = "Поле не может быть пустым!";
            else if (val.Length < 4)
                errorMessage = "Значение поля не может быть меньше 4 символов!";

            if (errorMessage != String.Empty)
                return new ValidationResult(false, errorMessage);
            else
                return new ValidationResult(true, null);

        }
    }
}
