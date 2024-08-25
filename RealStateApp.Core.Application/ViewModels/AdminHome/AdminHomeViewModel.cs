

namespace RealStateApp.Core.Application.ViewModels.AdminHome
{
    public class AdminHomeViewModel
    {
        public int AllProperties { get; set; } = 0;
        public int AllAgentsActive { get; set; }= 0;

        public int AllAgentsInactive { get; set; } = 0;
        public int AllClientsActive { get; set; } = 0;

        public int AllClientsInactive { get; set; } = 0;
        public int AllDevelopersActive { get; set; } = 0;

        public int AllDevelopersInactive { get; set; } = 0;
    }
}
