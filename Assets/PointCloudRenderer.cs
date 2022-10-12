using Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class PointCloudRenderer : MonoBehaviour
{
    Texture2D textureColor;
    Texture2D texturePositionScale;
    VisualEffect effect;
    uint resolution = 2048;

    public float particleSize = 0.1f;
    bool toUpdate= false;
    uint particleCount = 0;
    //public Color color;
    public float kRadiusOfEarth = 6000;
    public List<CvsModel> positions = new List<CvsModel>();

    // Start is called before the first frame update
    void Start()
    {
        effect = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (toUpdate)
        {
            toUpdate = false;
            effect.Reinit();
            effect.SetUInt(Shader.PropertyToID("ParticleCount"), particleCount);
            effect.SetTexture(Shader.PropertyToID("TextureColor"), textureColor);
            effect.SetTexture(Shader.PropertyToID("TexturePositionScale"), texturePositionScale);
            effect.SetUInt(Shader.PropertyToID("Resolution"), resolution);
        }
    }

    public void SetParticles(List<Vector3> positions, List<Color> colors)
    {
        textureColor = new Texture2D(positions.Count > (int)resolution ? (int)resolution : positions.Count, Mathf.Clamp(positions.Count / (int)resolution, 1, (int)resolution), TextureFormat.RGBAFloat, false);
        texturePositionScale = new Texture2D(positions.Count > (int)resolution ? (int)resolution : positions.Count, Mathf.Clamp(positions.Count / (int)resolution, 1, (int)resolution), TextureFormat.RGBAFloat, false);
        int textureWidth = textureColor.width;
        int textureHeight = textureColor.height;

        for (int y = 0; y < textureHeight; y++)
        {
            for (int x = 0; x < textureWidth; x++)
            {
                int index = x + y * textureWidth;
                textureColor.SetPixel(x, y, colors[index]);
                var data = new Color(positions[index].x, positions[index].y, positions[index].z, particleSize);
                texturePositionScale.SetPixel(x, y, data);
            }
        }

        textureColor.Apply();
        texturePositionScale.Apply();
        particleCount = (uint)positions.Count;
        toUpdate = true;
    }
 
    public void ApplyToParticleSystem(List<ShipData> ships)
    {
        List<Color> colors = new List<Color>();
        List<Vector3> pointPositions = new List<Vector3>();

        for (int ship = 0; ship < ships.Count; ship++)
        {
            for (int i = 0; i < ships[ship].Data.Count; i++)
            {
                positions.Add(ships[ship].Data[i]);
                colors.Add(ships[ship].LayerColor);
            }
        }

        Vector3[] particles = new Vector3[positions.Count];

        for (int i = 0; i < particles.Length; ++i)
        {
            Vector3 xyzCalculated = Quaternion.AngleAxis(positions[1].Longitude, -Vector3.up) * Quaternion.AngleAxis(positions[1].Latitude, -Vector3.right) * new Vector3(0, 0, 1);
            pointPositions.Add(xyzCalculated);
            //pointPositions.Add(new Vector3(positions[i].Longitude, positions[i].Bedlevel, positions[i].Latitude));
        }

        SetParticles(pointPositions, colors);
    }

}
