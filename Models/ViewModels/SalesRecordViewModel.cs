using UDEMY_PROJECT.Models.Enums;

namespace UDEMY_PROJECT.Models.ViewModels
{
    public class SalesRecordViewModel
    {
        public SalesRecord Record;

        public ICollection<SaleStatus> Status;
    }
}
