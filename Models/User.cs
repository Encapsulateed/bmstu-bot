using System;
using System.Collections.Generic;

namespace bmstu_bot.Models;

public partial class User
{
    public long ChatId { get; set; }

    public string? BmstuGroup { get; set; }

    public bool? Anonim { get; set; }

    public string? TgLink { get; set; }

    public string? ComandLine { get; set; }

    public string? Fio { get; set; }

    public int ComplainType { get; set; }

    public virtual ICollection<Complain> Complains { get; set; } = new List<Complain>();
}
