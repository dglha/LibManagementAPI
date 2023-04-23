namespace LibManagementAPI.Models;

public partial class TblBookLoan
{
    public int BookLoansLoansId { get; set; }

    public int BookLoansBookId { get; set; }

    public int BookLoansBranchId { get; set; }

    public int BookLoansCardNo { get; set; }

    public string BookLoansDateOut { get; set; } = null!;

    public string BookLoansDueDate { get; set; } = null!;

    public virtual TblBook BookLoansBook { get; set; } = null!;

    public virtual TblLibraryBranch BookLoansBranch { get; set; } = null!;

    public virtual TblBorrower BookLoansCardNoNavigation { get; set; } = null!;
}
