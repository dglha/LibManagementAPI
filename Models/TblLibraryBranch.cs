using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LibManagementAPI.Models;

public partial class TblLibraryBranch
{
    public int LibraryBranchBranchId { get; set; }

    public string LibraryBranchBranchName { get; set; } = null!;

    public string LibraryBranchBranchAddress { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<TblBookCopy> TblBookCopies { get; set; } = new List<TblBookCopy>();
    [JsonIgnore]
    public virtual ICollection<TblBookLoan> TblBookLoans { get; set; } = new List<TblBookLoan>();
}
