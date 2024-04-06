using System.ComponentModel.DataAnnotations;

namespace RoomService.Models;

public class ImageForRoomType
{
    [Key]
    public string Path { get; set; }
    public Guid RoomTypeId { get; set; }
    public RoomType RoomType { get; set; }
}