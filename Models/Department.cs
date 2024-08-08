namespace UDEMY_PROJECT.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Seller> Sellers { get; set; } = new List<Seller>();

        public Department() 
        { 
            
        }
        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void addSeller(Seller seller) 
        { 
            Sellers.Add(seller);
        }

        public double totalSales(DateTime dataInicio, DateTime dataFim)
        {
            return Sellers.Sum(seller => seller.totalSales(dataInicio, dataFim));
        }
    }
}
