namespace LibManagementAPI.Models
{
    public class TblBookAuthorDTO
    {
        public int BookAuthorsAuthorId { get; set; }

        public int BookAuthorsBookId { get; set; }

        public string BookAuthorsAuthorName { get; set; } = null!;
    }
}
