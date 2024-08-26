using System.ComponentModel.DataAnnotations;

namespace Agents_Rest.Model
{
    public enum StatusAgent
    {
        Dormant,
        Active
    }

    public class AgentModel
    {
        public int Id { get; set; }

        [Required] 
        public string NickName {  get; set; }
        public string ImageUrl {  get; set; }
        public int LocationX { get; set; } = 0;
        public int LocationY { get; set; } = 0;
        public StatusAgent Status { get; set; } = StatusAgent.Dormant;

    }
}
