using System;
using System.Collections.Generic;

namespace LibManagementAPI.Models;

public partial class TblBookCopy
{
    public int BookCopiesCopiesId { get; set; }

    public int BookCopiesBookId { get; set; }

    public int BookCopiesBranchId { get; set; }

    public int BookCopiesNoOfCopies { get; set; }

    public virtual TblBook BookCopiesBook { get; set; } = null!;

    public virtual TblLibraryBranch BookCopiesBranch { get; set; } = null!;
}
