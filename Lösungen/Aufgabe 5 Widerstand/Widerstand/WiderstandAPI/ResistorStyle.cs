using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiderstandAPI.Resistors;

namespace WiderstandAPI
{
    public struct ResistorStyle
    {
        public Color ResistorColor, WireColor, TextColor, BackColor;
        public string Font;

        public int ResistorWidth, ResistorHeight;
        public int WireLength, WireThickness;
    }
}
