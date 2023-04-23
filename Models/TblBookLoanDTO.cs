namespace LibManagementAPI.Models
{
    public class TblBookLoanDTO
    {
        public int BookLoansLoansId { get; set; }

        public int BookLoansBookId { get; set; }

        public int BookLoansBranchId { get; set; }

        public int BookLoansCardNo { get; set; }

        public string BookLoansDateOut { get; set; } = null!;

        public string BookLoansDueDate { get; set; } = null!;
        public virtual TblBookDTO? BookLoansBook { get; set; } = null!;
        public virtual TblLibraryBranch? BookLoansBranch { get; set; } = null!;


    }
}
