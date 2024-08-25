using System.ComponentModel.DataAnnotations;


namespace RealStateApp.Core.Application.ViewModels.Improvements
{
    public class SaveImprovementViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Debe ingresar el nombre")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Debe ingresar la descripcion")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
