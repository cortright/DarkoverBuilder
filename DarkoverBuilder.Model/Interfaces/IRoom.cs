using DarkoverBuilder.Model.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DarkoverBuilder.Model.Interfaces
{
    interface IRoom
    {
        public int VNUM { get; set; }
        public string Title{ get; set; }
        public string MainDescription { get; set; }
        public int ZoneNumber { get; set; }
        public SectorType SectorType { get; set; }
        public List<RoomFlag> RoomFlags { get; set; }
        public List<IExit> Exits { get; set; }
        public List<IExtraDescription> ExtraDescriptions { get; set; }
        public string ToString();

    }
}
