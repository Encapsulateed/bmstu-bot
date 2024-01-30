using System;
using System.Collections.Generic;

namespace bmstu_bot.Models;

public partial class Complain
{
    public int Id { get; set; }

    public long From { get; set; }

    public string? Message { get; set; }

    public DateTime? Date { get; set; }

    public string? Admin { get; set; }

    public int? Prev { get; set; }

    public int Type { get; set; }

    public string? Answer { get; set; }

    public bool IsAnon { get; set; }

    public virtual ICollection<Entry> Entries { get; set; } = new List<Entry>();

    public virtual User FromNavigation { get; set; } = null!;
}
