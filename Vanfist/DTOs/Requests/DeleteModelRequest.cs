using System.ComponentModel.DataAnnotations;

namespace Vanfist.DTOs.Requests;

public class DeleteModelRequest
{
    [Required]
    public int Id { get; set; }
}
