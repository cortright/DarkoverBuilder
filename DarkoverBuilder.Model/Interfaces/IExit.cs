using DarkoverBuilder.Model.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DarkoverBuilder.Model.Interfaces
{
    public interface IExit
    {
        public bool Enabled { get; set; }
        public Direction Direction { get; set; }
        public string Description { get; set; }
        public string DoorKeywords { get; set; }
        public DoorType DoorType { get; set; }
        public int DoorKeyVnum { get; set; }
        public int DestinationVnum { get; set; }
    }
}
