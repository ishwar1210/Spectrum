namespace Spectrum.Models;

public class MeetingRoom
{
    public int MeetingRoomId { get; set; }
    public string? MeetingRoom_Name { get; set; }
    public int? MeetingRoom_Floor { get; set; }
    public int? MeetingRoom_Capacity { get; set; }
    public int? MeetingRoom_AminitiesId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
