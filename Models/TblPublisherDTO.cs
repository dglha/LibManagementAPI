namespace LibManagementAPI.Models
{
    public class TblPublisherDTO
    {
        public string PublisherPublisherName { get; set; } = null!;

        public string PublisherPublisherAddress { get; set; } = null!;

        public string PublisherPublisherPhone { get; set; } = null!;

        //public Guid Rowguid { get; set; }

        //public virtual ICollection<TblBook> TblBooks { get; set; } = new List<TblBook>();
    }
}
