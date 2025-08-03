using System.ComponentModel.DataAnnotations;

namespace SigortaYonetimAPI.Validations
{
    public class TurkishTcKimlikAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success; // Boş değer kabul edilir (opsiyonel)
            }

            string tcKimlik = value.ToString()!;
            
            // Sadece rakamları al
            string digitsOnly = string.Join("", tcKimlik.Where(char.IsDigit));
            
            // 11 haneli olmalı
            if (digitsOnly.Length != 11)
            {
                return new ValidationResult("TC Kimlik numarası 11 haneli olmalıdır");
            }
            
            // Sadece rakam içermeli
            if (!digitsOnly.All(char.IsDigit))
            {
                return new ValidationResult("TC Kimlik numarası sadece rakam içermelidir");
            }
            
            // İlk hane 0 olamaz
            if (digitsOnly[0] == '0')
            {
                return new ValidationResult("TC Kimlik numarası 0 ile başlayamaz");
            }
            
            // TC Kimlik algoritması kontrolü
            if (!IsValidTcKimlik(digitsOnly))
            {
                return new ValidationResult("Geçersiz TC Kimlik numarası");
            }
            
            return ValidationResult.Success;
        }
        
        private bool IsValidTcKimlik(string tcKimlik)
        {
            if (tcKimlik.Length != 11) return false;
            
            // İlk 10 hanenin toplamının birler basamağı 11. hane olmalı
            int sum1 = 0;
            int sum2 = 0;
            
            for (int i = 0; i < 9; i++)
            {
                int digit = int.Parse(tcKimlik[i].ToString());
                sum1 += digit;
                if (i % 2 == 0)
                {
                    sum2 += digit;
                }
                else
                {
                    sum2 += digit * 3;
                }
            }
            
            int digit10 = int.Parse(tcKimlik[9].ToString());
            int digit11 = int.Parse(tcKimlik[10].ToString());
            
            // 10. hane kontrolü
            if ((sum1 % 10) != digit10) return false;
            
            // 11. hane kontrolü
            if ((sum2 % 10) != digit11) return false;
            
            return true;
        }
    }
} 