﻿using System.ComponentModel.DataAnnotations;

namespace Agents_Rest.Model
{
    public enum StatusTarget
    {
        Living,
        Assigned,
        Killed
    }

    public class TargetModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Position { get; set; }
        public int Location_x { get; set; } = 0;
        public int Location_y { get; set; } = 0;
        public string Image_url { get; set; }
        public StatusTarget Status { get; set; } = StatusTarget.Living;

    }
}
