using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    ParticleSystem ps;
    public int collectedOrbs = 0;
    public float orbsMultiplier = 1.0f;
    float initialParticleEmissionCount = 0;

    public Material[] materialsToUpdate;
    public int Radius;
    public List<Vector3> points;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        initialParticleEmissionCount = ps.emission.rateOverTime.constant;

        foreach (Material m in materialsToUpdate)
        {
            m.SetInt("_NPoints", 0);
            m.SetFloat("_Dist", Radius);
        }

        StartCoroutine("CreateColorPoints");
    }
    
    void UpdateMaterials()
    {
        Texture2D texture = new Texture2D(points.Count, 1);
        Color c = new Color();
        for (int x = 0; x < texture.width; x++)
        {
            c.r = points[x].x/ 255f ;
            c.g = points[x].y/ 255f ;
            c.b = points[x].z/ 255f ;
            c.a = 1.0f;
            texture.SetPixel(x, 1, c);
        }
        foreach (Material m in materialsToUpdate){
            m.SetTexture("_PosTex", texture);
            m.SetInt("_NPoints", points.Count);
            m.SetFloat("_Dist", Radius);
        }
        texture.Apply();
    }

    void Update()
    {
        foreach(Material m in materialsToUpdate) {
            // Set the player position in the shader file
            m.SetVector("_PlayerPos", this.transform.position);
            // Set the distance or radius
            m.SetFloat("_Dist", Radius);
        }
    }

    public void CollectOrb()
    {
        collectedOrbs++;
        ps.emissionRate = collectedOrbs * orbsMultiplier + initialParticleEmissionCount;
    }

    IEnumerator CreateColorPoints()
    {
        for (; ; )
        {
            points.Add(this.transform.position);
            UpdateMaterials();
            yield return new WaitForSeconds(.1f);
        }
    }
}
