using System;
using System.Collections.Generic;

namespace LibManagementAPI.Models;

public partial class TblBookAuthor
{
    public int BookAuthorsAuthorId { get; set; }

    public int BookAuthorsBookId { get; set; }

    public string BookAuthorsAuthorName { get; set; } = null!;

    public virtual TblBook BookAuthorsBook { get; set; } = null!;
}
