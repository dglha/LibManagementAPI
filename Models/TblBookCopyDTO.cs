namespace LibManagementAPI.Models
{
    public class TblBookCopyDTO
    {
        public int BookCopiesCopiesId { get; set; }

        public int BookCopiesBookId { get; set; }

        public int BookCopiesBranchId { get; set; }

        public int BookCopiesNoOfCopies { get; set; }

        public virtual TblLibraryBranchDTO? BookCopiesBranch { get; set; } = null!;
    }
}
