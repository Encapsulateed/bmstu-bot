using System;
using System.Collections.Generic;

namespace bmstu_bot.Models;

public partial class Admin
{
    public long ChatId { get; set; }

    public string? Link { get; set; }

    public virtual ICollection<Entry> Entries { get; set; } = new List<Entry>();
}
