using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QlNhanSu_Backend.Models;

public partial class CaLam
{
    public int IdCaLam { get; set; }

    public string TenCaLam { get; set; } = null!;

    public TimeOnly ThoiGianBatDau { get; set; }

    public double ThoiGianTreChoPhep { get; set; }

    public TimeOnly ThoiGianKetThuc { get; set; }

    public double ThoiGianSomChoPhep { get; set; }
    [JsonIgnore]
    public virtual ICollection<DiemDanh> DiemDanhs { get; set; } = new List<DiemDanh>();
}
