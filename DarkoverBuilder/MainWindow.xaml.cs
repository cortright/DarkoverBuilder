using DarkoverBuilder.Model;
using DarkoverBuilder.Model.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DarkoverBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // bind controls to enums defined in model
            Sector.ItemsSource = Enum.GetValues(typeof(SectorType));
            Flags.ItemsSource = Enum.GetValues(typeof(RoomFlag));            
            North_Door_Type.ItemsSource = Enum.GetValues(typeof(DoorType));
            East_Door_Type.ItemsSource = Enum.GetValues(typeof(DoorType));
            South_Door_Type.ItemsSource = Enum.GetValues(typeof(DoorType));
            West_Door_Type.ItemsSource = Enum.GetValues(typeof(DoorType));
            Up_Door_Type.ItemsSource = Enum.GetValues(typeof(DoorType));
            Down_Door_Type.ItemsSource = Enum.GetValues(typeof(DoorType));

            // calling these puts the controls into a ready state where they are disabled by default and have default selection in dropdowns
            North_Enabled_Checked(sender, e);
            East_Enabled_Checked(sender, e);
            South_Enabled_Checked(sender, e);
            West_Enabled_Checked(sender, e);
            Up_Enabled_Checked(sender, e);
            Down_Enabled_Checked(sender, e);
        }

        private bool FormatCheck()
        {
            var sb = new StringBuilder();

            var zoneIsInteger = int.TryParse(Zone.Text, out int parsedZone);
            if (!zoneIsInteger || parsedZone <= 0)
            {
                sb.AppendLine("Zone must be a positive integer");
            }

            var vnumIsInteger = int.TryParse(VNUM.Text, out int parsedVNUM);
            if (!vnumIsInteger || parsedVNUM <= 0)
            {
                sb.AppendLine("VNUM must be a positive integer");
            }

            if (VNUM.Text.IndexOf(Zone.Text) != 0)
            {
                sb.AppendLine("VNUM must begin with the same number as Zone");
            }

            if (Sector.SelectedValue == null)
            {
                sb.AppendLine("Sector must be selected");
            }

            if (Title.Text.Length == 0)
            {
                sb.AppendLine("Title must have text");
            }

            if (Description.Text.Length == 0)
            {
                sb.AppendLine("Description must have text");
            }

            if (North_Enabled.IsChecked ?? false)
            {
                var doorVnumIsInteger = int.TryParse(North_Destination.Text, out int parsedDoorVNUM);
                if (!doorVnumIsInteger || parsedDoorVNUM <= 0)
                {
                    sb.AppendLine("North destination VNUM must be a positive integer");
                }
            }

            if (East_Enabled.IsChecked ?? false)
            {
                var doorVnumIsInteger = int.TryParse(East_Destination.Text, out int parsedDoorVNUM);
                if (!doorVnumIsInteger || parsedDoorVNUM <= 0)
                {
                    sb.AppendLine("East destination VNUM must be a positive integer");
                }
            }

            if (South_Enabled.IsChecked ?? false)
            {
                var doorVnumIsInteger = int.TryParse(South_Destination.Text, out int parsedDoorVNUM);
                if (!doorVnumIsInteger || parsedDoorVNUM <= 0)
                {
                    sb.AppendLine("South destination VNUM must be a positive integer");
                }
            }

            if (West_Enabled.IsChecked ?? false)
            {
                var doorVnumIsInteger = int.TryParse(West_Destination.Text, out int parsedDoorVNUM);
                if (!doorVnumIsInteger || parsedDoorVNUM <= 0)
                {
                    sb.AppendLine("West destination VNUM must be a positive integer");
                }
            }

            if (Up_Enabled.IsChecked ?? false)
            {
                var doorVnumIsInteger = int.TryParse(Up_Destination.Text, out int parsedDoorVNUM);
                if (!doorVnumIsInteger || parsedDoorVNUM <= 0)
                {
                    sb.AppendLine("Up destination VNUM must be a positive integer");
                }
            }

            if (Down_Enabled.IsChecked ?? false)
            {
                var doorVnumIsInteger = int.TryParse(Down_Destination.Text, out int parsedDoorVNUM);
                if (!doorVnumIsInteger || parsedDoorVNUM <= 0)
                {
                    sb.AppendLine("Down destination VNUM must be a positive integer");
                }
            }

            /* ghetto style, report errors to the same textbox output would come in */
            var errorsFound = sb.ToString();

            if (errorsFound.Length > 0)
            {
                worldFileText.Text = "ERRORS FOUND: \n" + errorsFound;
                return false;
            }
            return true;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if(FormatCheck())
            {
                // if we pass format check, create a room model object and ToString() it - the overloaded ToString outputs DO room format
                var room = new Room
                {
                    VNUM = int.Parse(VNUM.Text),
                    ZoneNumber = int.Parse(Zone.Text),
                    Title = Title.Text,
                    MainDescription = "   " + Description.Text.Trim(),
                    SectorType = (SectorType)Enum.Parse(typeof(SectorType), Sector.SelectedValue.ToString())
                };

                foreach (var f in Flags.SelectedItems)
                {
                    room.RoomFlags.Add((RoomFlag)Enum.Parse(typeof(RoomFlag), f.ToString()));
                }

                if (North_Enabled.IsChecked ?? false)
                {
                    room.Exits.Add(
                        new Exit
                        {
                            Enabled = true,
                            Direction = Direction.North,
                            Description = North_Description.Text,
                            DoorKeywords = North_Door_Descriptions.Text,
                            DoorType = (DoorType)Enum.Parse(typeof(DoorType), North_Door_Type.SelectedValue.ToString()),
                            DoorKeyVnum = North_Door_Type.SelectedIndex == 0 ? 0 : int.TryParse(North_Key_VNUM.Text, out int keyVnum) ? keyVnum : -1,
                            DestinationVnum = int.Parse(North_Destination.Text)
                        }
                    );
                }

                if (East_Enabled.IsChecked ?? false)
                {
                    room.Exits.Add(
                        new Exit
                        {
                            Enabled = true,
                            Direction = Direction.East,
                            Description = East_Description.Text,
                            DoorKeywords = East_Door_Descriptions.Text,
                            DoorType = (DoorType)Enum.Parse(typeof(DoorType), East_Door_Type.SelectedValue.ToString()),
                            DoorKeyVnum = East_Door_Type.SelectedIndex == 0 ? 0 : int.TryParse(East_Key_VNUM.Text, out int keyVnum) ? keyVnum : -1,
                            DestinationVnum = int.Parse(East_Destination.Text)
                        }
                    );
                }

                if (South_Enabled.IsChecked ?? false)
                {
                    room.Exits.Add(
                        new Exit
                        {
                            Enabled = true,
                            Direction = Direction.South,
                            Description = South_Description.Text,
                            DoorKeywords = South_Door_Descriptions.Text,
                            DoorType = (DoorType)Enum.Parse(typeof(DoorType), South_Door_Type.SelectedValue.ToString()),
                            DoorKeyVnum = South_Door_Type.SelectedIndex == 0 ? 0 : int.TryParse(South_Key_VNUM.Text, out int keyVnum) ? keyVnum : -1,
                            DestinationVnum = int.Parse(South_Destination.Text)
                        }
                    );
                }

                if (West_Enabled.IsChecked ?? false)
                {
                    room.Exits.Add(
                        new Exit
                        {
                            Enabled = true,
                            Direction = Direction.West,
                            Description = West_Description.Text,
                            DoorKeywords = West_Door_Descriptions.Text,
                            DoorType = (DoorType)Enum.Parse(typeof(DoorType), West_Door_Type.SelectedValue.ToString()),
                            DoorKeyVnum = West_Door_Type.SelectedIndex == 0 ? 0 : int.TryParse(West_Key_VNUM.Text, out int keyVnum) ? keyVnum : -1,
                            DestinationVnum = int.Parse(West_Destination.Text)
                        }
                    );
                }

                if (Up_Enabled.IsChecked ?? false)
                {
                    room.Exits.Add(
                        new Exit
                        {
                            Enabled = true,
                            Direction = Direction.Up,
                            Description = Up_Description.Text,
                            DoorKeywords = Up_Door_Descriptions.Text,
                            DoorType = (DoorType)Enum.Parse(typeof(DoorType), Up_Door_Type.SelectedValue.ToString()),
                            DoorKeyVnum = Up_Door_Type.SelectedIndex == 0 ? 0 : int.TryParse(Up_Key_VNUM.Text, out int keyVnum) ? keyVnum : -1,
                            DestinationVnum = int.Parse(Up_Destination.Text)
                        }
                    );
                }

                if (Down_Enabled.IsChecked ?? false)
                {
                    room.Exits.Add(
                        new Exit
                        {
                            Enabled = true,
                            Direction = Direction.Down,
                            Description = Down_Description.Text,
                            DoorKeywords = Down_Door_Descriptions.Text,
                            DoorType = (DoorType)Enum.Parse(typeof(DoorType), Down_Door_Type.SelectedValue.ToString()),
                            DoorKeyVnum = Down_Door_Type.SelectedIndex == 0 ? 0 : int.TryParse(Down_Key_VNUM.Text, out int keyVnum) ? keyVnum : -1,
                            DestinationVnum = int.Parse(Down_Destination.Text)
                        }
                    );
                }


                worldFileText.Text = room.ToString();
            }
        }

        private void North_Enabled_Checked(object sender, RoutedEventArgs e)
        {
            bool enabled = North_Enabled.IsChecked ?? false;
            North_Description.IsEnabled = enabled;
            North_Destination.IsEnabled = enabled;
            North_Door_Type.IsEnabled = enabled;
            North_Key_VNUM.IsEnabled = enabled;
            North_Door_Descriptions.IsEnabled = enabled;

            if (!enabled)
            {
                North_Description.Text = string.Empty;
                North_Destination.Text = string.Empty;
                North_Door_Type.SelectedIndex = 0;
                North_Key_VNUM.Text = string.Empty;
                North_Door_Descriptions.Text = string.Empty;
            }
        }

        private void East_Enabled_Checked(object sender, RoutedEventArgs e)
        {
            bool enabled = East_Enabled.IsChecked ?? false;
            East_Description.IsEnabled = enabled;
            East_Destination.IsEnabled = enabled;
            East_Door_Type.IsEnabled = enabled;
            East_Key_VNUM.IsEnabled = enabled;
            East_Door_Descriptions.IsEnabled = enabled;

            if (!enabled)
            {
                East_Description.Text = string.Empty;
                East_Destination.Text = string.Empty;
                East_Door_Type.SelectedIndex = 0;
                East_Key_VNUM.Text = string.Empty;
                East_Door_Descriptions.Text = string.Empty;
            }
        }

        private void South_Enabled_Checked(object sender, RoutedEventArgs e)
        {
            bool enabled = South_Enabled.IsChecked ?? false;
            South_Description.IsEnabled = enabled;
            South_Destination.IsEnabled = enabled;
            South_Door_Type.IsEnabled = enabled;
            South_Key_VNUM.IsEnabled = enabled;
            South_Door_Descriptions.IsEnabled = enabled;

            if (!enabled)
            {
                South_Description.Text = string.Empty;
                South_Destination.Text = string.Empty;
                South_Door_Type.SelectedIndex = 0;
                South_Key_VNUM.Text = string.Empty;
                South_Door_Descriptions.Text = string.Empty;
            }
        }

        private void West_Enabled_Checked(object sender, RoutedEventArgs e)
        {
            bool enabled = West_Enabled.IsChecked ?? false;
            West_Description.IsEnabled = enabled;
            West_Destination.IsEnabled = enabled;
            West_Door_Type.IsEnabled = enabled;
            West_Key_VNUM.IsEnabled = enabled;
            West_Door_Descriptions.IsEnabled = enabled;

            if (!enabled)
            {
                West_Description.Text = string.Empty;
                West_Destination.Text = string.Empty;
                West_Door_Type.SelectedIndex = 0;
                West_Key_VNUM.Text = string.Empty;
                West_Door_Descriptions.Text = string.Empty;
            }
        }

        private void Up_Enabled_Checked(object sender, RoutedEventArgs e)
        {
            bool enabled = Up_Enabled.IsChecked ?? false;
            Up_Description.IsEnabled = enabled;
            Up_Destination.IsEnabled = enabled;
            Up_Door_Type.IsEnabled = enabled;
            Up_Key_VNUM.IsEnabled = enabled;
            Up_Door_Descriptions.IsEnabled = enabled;

            if (!enabled)
            {
                Up_Description.Text = string.Empty;
                Up_Destination.Text = string.Empty;
                Up_Door_Type.SelectedIndex = 0;
                Up_Key_VNUM.Text = string.Empty;
                Up_Door_Descriptions.Text = string.Empty;
            }
        }

        private void Down_Enabled_Checked(object sender, RoutedEventArgs e)
        {
            bool enabled = Down_Enabled.IsChecked ?? false;
            Down_Description.IsEnabled = enabled;
            Down_Destination.IsEnabled = enabled;
            Down_Door_Type.IsEnabled = enabled;
            Down_Key_VNUM.IsEnabled = enabled;
            Down_Door_Descriptions.IsEnabled = enabled;

            if (!enabled)
            {
                Down_Description.Text = string.Empty;
                Down_Destination.Text = string.Empty;
                Down_Door_Type.SelectedIndex = 0;
                Down_Key_VNUM.Text = string.Empty;
                Down_Door_Descriptions.Text = string.Empty;
            }
        }
    }
}
