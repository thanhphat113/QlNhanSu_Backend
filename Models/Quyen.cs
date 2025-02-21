using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QlNhanSu_Backend.Models;

public partial class Quyen
{
    public int IdQuyen { get; set; }

    public string TenQuyen { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<VaiTro> IdVaiTros { get; set; } = new List<VaiTro>();
}
