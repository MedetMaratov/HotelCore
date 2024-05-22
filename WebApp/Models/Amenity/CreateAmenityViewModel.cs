using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Amenity;

public class CreateAmenityViewModel
{
    [Required(ErrorMessage = "Поле 'Название' обязательно для заполнения")]
    public string Title { get; set; }
    public string? Description { get; set; }
}