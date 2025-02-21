using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QlNhanSu_Backend.Models;

public partial class ChucVu
{
    public int IdChucVu { get; set; }

    public string TenChucVu { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
}
