﻿using System.ComponentModel.DataAnnotations;

namespace Vanfist.DTOs.Requests;

public class TestDriveRequestCreate
{
    [Required, StringLength(80)]
    public string FullName { get; set; } = default!;

    [EmailAddress, StringLength(120)]
    public string? Email { get; set; }

    [Phone, StringLength(20)]
    public string? Phone { get; set; }

    public int? ModelId { get; set; }

    public DateTime? PreferredTime { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }
}