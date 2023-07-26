using System;
using System.Collections.Generic;

namespace TodoApi;

public partial class Item
{
    public int IdItem { get; set; }

    public string? Name { get; set; }

    public bool? IsComplete { get; set; }
}
