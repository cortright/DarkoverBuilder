using DarkoverBuilder.Model.Enumerations;
using DarkoverBuilder.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkoverBuilder.Model
{
    public class Room : IRoom
    {
        #region Properties
        public int VNUM { get; set; }
        public string Title { get; set; }
        public string MainDescription { get; set; }
        public int ZoneNumber { get; set; }
        public SectorType SectorType { get; set; }
        public List<RoomFlag> RoomFlags { get; set; }
        public List<IExit> Exits { get; set; }
        public List<IExtraDescription> ExtraDescriptions { get; set; }
        #endregion

        #region Constructors
        public Room()
        {
            RoomFlags = new List<RoomFlag>();
            Exits = new List<IExit>();
            ExtraDescriptions = new List<IExtraDescription>();
        }
        #endregion

        public override string ToString()
        {
            var sb = new StringBuilder();

            //vnum line
            sb.Append("#");
            sb.AppendLine(VNUM.ToString());

            //title line
            sb.Append(Title);
            sb.AppendLine("~");

            //description
            sb.AppendLine(MainDescription);
            sb.AppendLine("~");

            // zone number, flags, sector line
            sb.Append(ZoneNumber.ToString());
            // space before flags
            sb.Append(" ");
            if (RoomFlags.Count == 0)
            {
                sb.Append("0");
            }
            else
            {
                var sortedRoomFlags = RoomFlags.OrderBy(rf => (int)rf);
                sb.Append("^");  // prepend the first item's caret, take care of the rest with this join
                sb.Append(string.Join("|^", sortedRoomFlags.Select(x => ((int)x).ToString()).ToArray()));
            }
            // space before sector
            sb.Append(" ");
            sb.AppendLine(((int)SectorType).ToString());

            // exits - only get the ones that are enabled
            var sortedExits = Exits.OrderBy(e => ((int)e.Direction)).Where(e => e.Enabled == true).ToList();
            foreach(var exit in sortedExits)
            {
                //D line
                sb.Append("D");
                sb.AppendLine(((int)exit.Direction).ToString());
                
                //Exit description
                sb.AppendLine(exit.Description);
                sb.AppendLine("~");

                // Door keywords line
                if (exit.DoorType != DoorType.None)
                {
                    sb.Append(exit.DoorKeywords);
                }
                sb.AppendLine("~");
                
                // Door config line
                sb.Append(((int)exit.DoorType).ToString());
                sb.Append(" ");
                sb.Append(exit.DoorKeyVnum);
                sb.Append(" ");
                sb.AppendLine(exit.DestinationVnum.ToString());
            }

            // extra descriptions
            foreach(var ex in ExtraDescriptions)
            {
                // E line
                sb.AppendLine("E");

                // Extra description keywords line
                sb.Append(ex.Keywords);
                sb.AppendLine("~");

                // Extra description
                sb.AppendLine(ex.Description);
                sb.AppendLine("~");
            }

            // closing S
            sb.AppendLine("S");

            return sb.ToString();
        }
    }
}
