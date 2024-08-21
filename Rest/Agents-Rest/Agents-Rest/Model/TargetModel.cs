﻿namespace Agents_Rest.Model
{
    public enum StatusTarget
    {
        Living,
        Killed
    }

    public class TargetModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public int Location_x { get; set; } = 0;
        public int Location_y { get; set; } = 0;
        public StatusTarget StatusTarget { get; set; } = StatusTarget.Living;

    }
}
