using System.ComponentModel.DataAnnotations;

namespace SigortaYonetimAPI.Validations
{
    public class TurkishVergiNoAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success; // Boş değer kabul edilir (opsiyonel)
            }

            string vergiNo = value.ToString()!;
            
            // Sadece rakamları al
            string digitsOnly = string.Join("", vergiNo.Where(char.IsDigit));
            
            // 10 haneli olmalı
            if (digitsOnly.Length != 10)
            {
                return new ValidationResult("Vergi numarası 10 haneli olmalıdır");
            }
            
            // Sadece rakam içermeli
            if (!digitsOnly.All(char.IsDigit))
            {
                return new ValidationResult("Vergi numarası sadece rakam içermelidir");
            }
            
            // Vergi numarası algoritması kontrolü
            if (!IsValidVergiNo(digitsOnly))
            {
                return new ValidationResult("Geçersiz vergi numarası");
            }
            
            return ValidationResult.Success;
        }
        
        private bool IsValidVergiNo(string vergiNo)
        {
            if (vergiNo.Length != 10) return false;
            
            // Son hane kontrol hanesi
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                int digit = int.Parse(vergiNo[i].ToString());
                int temp = (digit + 10 - (i + 1)) % 10;
                sum = (sum + (temp == 9 ? temp : (temp * (int)Math.Pow(2, 10 - (i + 1)) % 9))) % 10;
            }
            
            int lastDigit = int.Parse(vergiNo[9].ToString());
            return (10 - sum) % 10 == lastDigit;
        }
    }
} 