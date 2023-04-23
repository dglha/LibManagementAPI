using System;
using System.Collections.Generic;

namespace LibManagementAPI.Models;

public partial class TblPublisher
{
    public string PublisherPublisherName { get; set; } = null!;

    public string PublisherPublisherAddress { get; set; } = null!;

    public string PublisherPublisherPhone { get; set; } = null!;

    public virtual ICollection<TblBook> TblBooks { get; set; } = new List<TblBook>();
}
