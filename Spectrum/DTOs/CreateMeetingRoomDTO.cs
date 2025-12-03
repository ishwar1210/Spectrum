using System.ComponentModel.DataAnnotations;

namespace Spectrum.DTOs;

public class CreateMeetingRoomDTO
{
    [Required]
    [StringLength(200)]
    public string MeetingRoom_Name { get; set; } = null!;
    public int? MeetingRoom_Floor { get; set; }
    public int? MeetingRoom_Capacity { get; set; }
    public int? MeetingRoom_AminitiesId { get; set; }
}
