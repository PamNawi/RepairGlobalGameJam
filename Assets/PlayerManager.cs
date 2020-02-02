using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    ParticleSystem ps;
    public int collectedOrbs = 0;
    public float orbsMultiplier = 1.0f;
    float initialParticleEmissionCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        initialParticleEmissionCount = ps.emission.rateOverTime.constant;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectOrb()
    {
        collectedOrbs++;
        ps.emissionRate = collectedOrbs * orbsMultiplier + initialParticleEmissionCount;
    }
}
