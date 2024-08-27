using Agents_MVC.Models;
using System.Collections.Generic;
namespace AgentClient.ViewModel
{
    public class MatrixVm
    {
        public string[,] Matrix { get; set; }
        public List<AgentModel> Agents { get; set; }
        public List<TargetModel> Targets { get; set; }
        public MatrixVm(int rows, int cols)
        {
            Matrix = new string[rows, cols];
            Agents = new List<AgentModel>();
            Targets = new List<TargetModel>();
        }
    }
}