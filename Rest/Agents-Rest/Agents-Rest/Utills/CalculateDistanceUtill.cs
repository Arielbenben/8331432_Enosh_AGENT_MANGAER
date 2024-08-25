using Agents_Rest.Model;

namespace Agents_Rest.Utills
{
    public static class CalculateDistanceUtill
    {
        public static double CalculateDistance(AgentModel agent, TargetModel target)
        {
            return Math.Sqrt(Math.Pow(agent.LocationX - target.LocationX, 2) +
                Math.Pow(agent.LocationY - target.LocationY, 2));
        }

        

    }
}
