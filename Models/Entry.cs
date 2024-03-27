using System;
using System.Collections.Generic;

namespace bmstu_bot.Models;

public partial class Entry
{
    public long AdminChat { get; set; }

    public int ComplainId { get; set; }

    public long MessageId { get; set; }

    public int Id { get; set; }

    public virtual Complain Complain { get; set; } = null!;
}
