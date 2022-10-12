using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Models
{
    public class CvsModel
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float Bedlevel { get; set; }
        public decimal Timestamp { get; set; }
        public decimal ShipId { get; set; }
        public decimal WaterLevel { get; set; }
        public decimal WaterDepth { get; set; }
        public string Station { get; set; }
        public decimal WaterwayPolygonId { get; set; } //
        public decimal Olr { get; set; }
        public decimal Heigth { get; set; }
        // custom properties
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
    }

    public class ShipData
    {
        public decimal Ship { get; set; }
        public List<CvsModel> Data { get; set; }
        public UnityEngine.Color LayerColor { get; set; }
    }
}
