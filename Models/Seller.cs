using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace UDEMY_PROJECT.Models
{
    public class Seller
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(48, MinimumLength = 3, ErrorMessage = "{0} Size should be between {2} and {1}")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [Range(100.0,500.0, ErrorMessage = "{0} must be from {1} to {2}")]
        [Display(Name = "Base Salary")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double BaseSalary { get; set; }
        [AllowNull]
        public virtual Department Department { get; set; }
        public int DepartmentId { get; set; }
        public virtual ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller()
        {

        }

        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        public void AddSales(SalesRecord sales)
        {
            Sales.Add(sales);
        }

        public void RemoveSales(SalesRecord sales) 
        { 
            Sales.Remove(sales); 
        }

        public double totalSales(DateTime dataInicio, DateTime dataFim)
        {
            return Sales.Where(a => a.Date >= dataInicio || a.Date <= dataFim).Sum(a => a.Amount);
        }
    }
}
