﻿using System;
using System.Collections.Generic;

namespace ContactManagementApi.OutputDirectory;

public partial class Contact
{
    public int ContactId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
