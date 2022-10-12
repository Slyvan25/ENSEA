using UnityEngine;
using UnityEditor;
using System.IO;
using TMPro;
using System.Linq;
using Models;

public class ImportChart : MonoBehaviour
{
    public TextMeshProUGUI debugText;

    // Start is called before the first frame update
    void Start()
    {
        string path = EditorUtility.OpenFilePanel("Import chart data file", "", "csv");
        if (path.Length != 0)
        {
            debugText.text = "loading...";
            var data = (from line in File.ReadAllLines(path).Skip(1)
                        let columns = line.Split(',')
                        select new CvsModel
                            {
                                Latitude = float.Parse(columns[0]),
                                Longitude = float.Parse(columns[1]),
                                Bedlevel = float.Parse(columns[2]),
                                Timestamp = decimal.Parse(columns[3]),
                                ShipId = decimal.Parse(columns[4]),
                                WaterLevel = decimal.Parse(columns[5]),
                                WaterDepth = decimal.Parse(columns[6]),
                                Station = columns[7],
                                WaterwayPolygonId = decimal.Parse(columns[8]),
                                Olr = decimal.Parse(columns[9]),
                                Heigth = decimal.Parse(columns[10]),
                                x = float.Parse(columns[0]),
                                y = float.Parse(columns[10]),
                                z = float.Parse(columns[1])
                            }
                        )
                        .GroupBy(ship => ship.ShipId)
                        .Select(groupedShipScans => new ShipData { Ship = groupedShipScans.Key, Data = groupedShipScans.ToList(), LayerColor = Random.ColorHSV() })
                        .ToList();

            debugText.text = "Parsed the file to a readable list..";
            GameObject.Find("Visual Effect").GetComponent<PointCloudRenderer>().ApplyToParticleSystem(data);
            debugText.text = "Generating the point cloud(s)";

        }
    }
}
