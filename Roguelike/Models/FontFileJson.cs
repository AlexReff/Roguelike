using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike.Models
{
    class FontFileJson
    {
        public string Name { get; set; } //: "Aesomatica",
        public string FilePath { get; set; } //: "Aesomatica-16x16.png",
        public int GlyphHeight { get; set; } //: 16,
        public int GlyphPadding { get; set; } //: 0,
        public int GlyphWidth { get; set; } //: 16,
        public int SolidGlyphIndex { get; set; } //: 219,
        public int Columns { get; set; } //: 16,
        public bool IsSadExtended { get; set; } //: false
    }
}
