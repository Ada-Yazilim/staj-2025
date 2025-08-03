using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SigortaYonetimAPI.Validations
{
    public class TurkishPhoneAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success; // Boş değer kabul edilir (opsiyonel)
            }

            string phoneNumber = value.ToString()!;
            
            // Sadece rakamları al
            string digitsOnly = Regex.Replace(phoneNumber, @"[^\d]", "");
            
            // Türkiye telefon numarası formatları:
            // 5XX XXX XX XX (cep telefonu)
            // 2XX XXX XX XX (sabit telefon)
            // 0 5XX XXX XX XX (başında 0 ile)
            // +90 5XX XXX XX XX (başında +90 ile)
            
            if (digitsOnly.StartsWith("90"))
            {
                digitsOnly = digitsOnly.Substring(2);
            }
            
            if (digitsOnly.StartsWith("0"))
            {
                digitsOnly = digitsOnly.Substring(1);
            }
            
            // 10 haneli olmalı ve 5 ile başlamalı (cep telefonu) veya 2 ile başlamalı (sabit)
            if (digitsOnly.Length == 10 && (digitsOnly.StartsWith("5") || digitsOnly.StartsWith("2")))
            {
                return ValidationResult.Success;
            }
            
            return new ValidationResult("Geçerli bir Türkiye telefon numarası giriniz (Örn: 0532 123 45 67 veya 0212 123 45 67)");
        }
    }
} 