﻿using System.ComponentModel.DataAnnotations;

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
        public string Image_url {  get; set; }
        public int Location_x { get; set; } = 0;
        public int Location_y { get; set; } = 0;
        public StatusAgent Status { get; set; } = StatusAgent.Dormant;

    }
}
