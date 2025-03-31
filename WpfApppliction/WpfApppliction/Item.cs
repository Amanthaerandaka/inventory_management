using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfApppliction.db
{
    public class Item
    {
        [Key]
        public int ItemID { get; set; }

        [Required(ErrorMessage = "Item Name is required.")]
        [StringLength(100, ErrorMessage = "Item Name cannot exceed 100 characters.")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Unit Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit Price must be a positive number.")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Case Per Unit is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Case Per Unit must be at least 1.")]
        public int CasePerUnit { get; set; }

        [Required(ErrorMessage = "Supplier is required.")]
        [StringLength(50, ErrorMessage = "Supplier name cannot exceed 50 characters.")]
        public string Supplier { get; set; }

        public DateTime? Date { get; set; }

        // input filed validation
        public bool Validate(out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrWhiteSpace(ItemName))
            {
                errorMessage = "Item Name is required.";
                return false;
            }

            if (ItemName.Length > 100)
            {
                errorMessage = "Item Name cannot exceed 100 characters.";
                return false;
            }

            if (UnitPrice <= 0)
            {
                errorMessage = "Unit Price must be a positive number.";
                return false;
            }

            if (CasePerUnit <= 0)
            {
                errorMessage = "Case Per Unit must be a positive integer.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Supplier) || !Regex.IsMatch(Supplier, @"^[A-Za-z\s]+$"))
            {
                errorMessage = "Supplier must contain only letters.";
                return false;
            }

            if (Date == default)
            {
                errorMessage = "Date is required.";
                return false;
            }

            return true; // All validations passed
        }
    }
}
