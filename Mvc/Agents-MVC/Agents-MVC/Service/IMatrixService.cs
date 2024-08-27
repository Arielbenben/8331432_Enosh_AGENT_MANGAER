using AgentClient.ViewModel;

namespace Agents_MVC.Service
{
    public interface IMatrixService
    {
        Task<MatrixVm> InitMatrix();
    }
}
