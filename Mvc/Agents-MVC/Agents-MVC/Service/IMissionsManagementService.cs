using Agents_MVC.Models;
using Agents_MVC.ViewModel;

namespace Agents_MVC.Service
{
    public interface IMissionsManagementService
    {
        Task<List<MissionManagementVM>> CreateAllMissionsVm();
    }
}
