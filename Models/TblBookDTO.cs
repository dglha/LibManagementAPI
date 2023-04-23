namespace LibManagementAPI.Models
{
    public class TblBookDTO
    {
        public int BookBookId { get; set; }

        public string BookTitle { get; set; } = null!;

        public string BookPublisherName { get; set; } = null!;

        public virtual ICollection<TblBookAuthorDTO> TblBookAuthors { get; set; } = new List<TblBookAuthorDTO>();

        public virtual ICollection<TblBookCopyDTO> TblBookCopies { get; set; } = new List<TblBookCopyDTO>();
    }
}
