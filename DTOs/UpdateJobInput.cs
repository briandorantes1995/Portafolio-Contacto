using Portafolio.Models;
namespace Portafolio.DTOs;
using System.ComponentModel.DataAnnotations;

public class UpdateJobInput
{
    [Required]
    public string Title { get; set; } = string.Empty;

    public decimal? Budget { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    public Statuses Status { get; set; }

    public SubStatuses SubStatus { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}