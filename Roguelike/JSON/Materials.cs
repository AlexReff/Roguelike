using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Roguelike.JSON
{
    public struct Materials
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Descriptor { get; set; }
        public double Density { get; set; }
        public double Hardness { get; set; }
        public double HeatRateOfChange { get; set; }
        public int MeltingPoint { get; set; }
        public int IgnitePoint { get; set; }
        public int BoilingPoint { get; set; }
        public int Value { get; set; }

        public Materials(JsonElement m)
        {
            ID = m.GetProperty("ID").GetString();
            Name = m.GetProperty("Name").GetString();
            Descriptor = m.GetProperty("Descriptor").GetString();
            Density = m.GetProperty("Density").GetDouble();
            Hardness = m.GetProperty("Hardness").GetDouble();
            HeatRateOfChange = m.GetProperty("HeatRateOfChange").GetDouble();
            MeltingPoint = m.GetProperty("MeltingPoint").GetInt32();
            IgnitePoint = m.GetProperty("IgnitePoint").GetInt32();
            BoilingPoint = m.GetProperty("BoilingPoint").GetInt32();
            Value = m.GetProperty("Value").GetInt32();
        }
    }
}
