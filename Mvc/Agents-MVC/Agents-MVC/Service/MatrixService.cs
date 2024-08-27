using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using AgentClient.ViewModel;
using Agents_MVC.Service;
namespace AgentClient.Service
{
    public class MatrixService(IServiceProvider serviceProvider) : IMatrixService
    {
        private ISystemDashboardService generalService => serviceProvider.GetRequiredService<ISystemDashboardService>();

        public async Task<MatrixVm> InitMatrix()
        {
            try
            {
                var agents = await generalService.GetAllAgents();
                var targets = await generalService.GetAllTargets();
                if (!agents.Any() && !targets.Any())
                {
                    return new MatrixVm(1, 1);
                }
                // Determine the matrix size
                int maxX = Math.Max(agents.Max(a => a.LocationX), targets.Max(t => t.LocationX)) + 1;
                int maxY = Math.Max(agents.Max(a => a.LocationY), targets.Max(t => t.LocationY)) + 1;
                var model = new MatrixVm(maxX, maxY);
                // Place agents in the matrix
                foreach (var agent in agents)
                {
                    if (agent.LocationX >= 0 && agent.LocationX < maxX && agent.LocationY >= 0 && agent.LocationY < maxY)
                    {
                        model.Matrix[agent.LocationX, agent.LocationY] += $"{agent.NickName}";
                    }
                }
                // Place targets in the matrix
                foreach (var target in targets)
                {
                    if (target.LocationX >= 0 && target.LocationX < maxX && target.LocationY >= 0 && target.LocationY < maxY)
                    {
                        model.Matrix[target.LocationX, target.LocationY] += $"{target.Name}";
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}