using System.ComponentModel.DataAnnotations;

namespace Spectrum.DTOs;

public class UpdateMeetingRoomDTO
{
    [StringLength(200)]
    public string? MeetingRoom_Name { get; set; }
    public int? MeetingRoom_Floor { get; set; }
    public int? MeetingRoom_Capacity { get; set; }
    public int? MeetingRoom_AminitiesId { get; set; }
}
