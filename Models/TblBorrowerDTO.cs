namespace LibManagementAPI.Models
{
    public class TblBorrowerDTO
    {
        public int BorrowerCardNo { get; set; }

        public string BorrowerBorrowerName { get; set; } = null!;

        public string BorrowerBorrowerAddress { get; set; } = null!;

        public string BorrowerBorrowerPhone { get; set; } = null!;
    }
}
