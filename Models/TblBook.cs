using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LibManagementAPI.Models;

public partial class TblBook
{
    public int BookBookId { get; set; }

    public string BookTitle { get; set; } = null!;

    public string BookPublisherName { get; set; } = null!;

    public virtual TblPublisher BookPublisherNameNavigation { get; set; } = null!;
    
    public virtual ICollection<TblBookAuthor> TblBookAuthors { get; set; } = new List<TblBookAuthor>();
    
    public virtual ICollection<TblBookCopy> TblBookCopies { get; set; } = new List<TblBookCopy>();
    [JsonIgnore]
    public virtual ICollection<TblBookLoan> TblBookLoans { get; set; } = new List<TblBookLoan>();
}
