using Agents_Rest.Model;

namespace Agents_Rest.Utills
{
    public static class CalculateDistanceUtill
    {
        public static double CalculateDistance(AgentModel agent, TargetModel target)
        {
            return Math.Sqrt(Math.Pow(agent.Location_x - target.Location_x, 2) +
                Math.Pow(agent.Location_y - target.Location_y, 2));
        }

        

    }
}
