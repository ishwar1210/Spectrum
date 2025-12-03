using System.ComponentModel.DataAnnotations;

namespace Spectrum.DTOs;

public class UpdateRoomBookingDTO
{
    public int? RoomBooking_MeetingroomId { get; set; }
    public int? RoomBooking_UserID { get; set; }
    public int? RoomBooking_VisitorID { get; set; }
    public DateTime? RoomBooking_MeetingDate { get; set; }
    public TimeSpan? RoomBooking_Starttime { get; set; }
    public TimeSpan? RoomBooking_Endtime { get; set; }
}
