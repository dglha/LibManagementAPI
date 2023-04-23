using System;
using System.Collections.Generic;

namespace LibManagementAPI.Models;

public partial class TblBorrower
{
    public int BorrowerCardNo { get; set; }

    public string BorrowerBorrowerName { get; set; } = null!;

    public string BorrowerBorrowerAddress { get; set; } = null!;

    public string BorrowerBorrowerPhone { get; set; } = null!;

    public virtual ICollection<TblBookLoan> TblBookLoans { get; set; } = new List<TblBookLoan>();
}
