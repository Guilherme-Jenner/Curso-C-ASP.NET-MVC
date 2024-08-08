using UDEMY_PROJECT.Models;

namespace UDEMY_PROJECT.Data
{
    public class SeedingService
    {
        private UDEMY_PROJECTContext _context;

        public SeedingService(UDEMY_PROJECTContext context) 
        { 
            _context = context;
        }

        public void Seed()
        {
            if (_context.Department.Any() || _context.Seller.Any() || _context.SalesRecords.Any())
            {
                return;
            }

            Department d1 = new Department(1, "Eletrônico");
            Department d2 = new Department(2, "Móveis");
            Department d3 = new Department(3, "Livros");

            Seller s1 = new Seller(1, "Guilherme Jenner", "guilhermejenner10@gmail.com" , new DateTime(2003, 1, 30), 4500, d1);
            Seller s2 = new Seller(2, "Leonardo", "leo@gmail.com", new DateTime(2000, 1, 30), 2500, d2);
            Seller s3 = new Seller(3, "Roberto", "roberto@gmail.com", new DateTime(2004, 1, 30), 2200, d3);


            SalesRecord salesRecord = new SalesRecord(1, DateTime.UtcNow, 10, Models.Enums.SaleStatus.Billed, s1);
            SalesRecord salesRecord2 = new SalesRecord(2, DateTime.UtcNow, 10, Models.Enums.SaleStatus.Billed, s2);
            SalesRecord salesRecord3 = new SalesRecord(3, DateTime.UtcNow, 10, Models.Enums.SaleStatus.Billed, s3);


            _context.Department.AddRange(d1,d2, d3);
            _context.Seller.AddRange(s1,s2,s3);
            _context.SalesRecords.AddRange(salesRecord,salesRecord2,salesRecord3);

            _context.SaveChanges();
        }
    }
}
