using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : CSingel<ParticleManager>
{
    GameObject parent;
    private Dictionary<E_Particle, GameObject> particleResDic = new Dictionary<E_Particle, GameObject>();
    private Dictionary<E_Particle, GameObject> particleDic = new Dictionary<E_Particle, GameObject>();
    string PARTICLE_PATH = "Assets/ui/particle/";

    public GameObject getEffect(E_Particle particle)
    {
        GameObject result;
        if (!particleResDic.ContainsKey(particle))
        {
            GameObject obj = AssetLoad.Instance.loadUIPrefab<GameObject>(PARTICLE_PATH, particle.ToString());
            particleResDic.Add(particle, obj);
        }
        result = UnityEngine.Object.Instantiate(particleResDic[particle]);
        result.SetActive(false);
        return result;
    }

    public void playEffect(E_Particle particle,Vector3 transfer)
    {
        if (!particleResDic.ContainsKey(particle))
        {
            GameObject res = AssetLoad.Instance.loadUIPrefab<GameObject>(PARTICLE_PATH, particle.ToString());
            var obj = UnityEngine.Object.Instantiate(res);
            if (parent == null)
            {
                parent = new GameObject("Particles");
            }
            obj.transform.SetParent(parent.transform);
            particleResDic.Add(particle, obj);
        }
        particleResDic[particle].transform.position = transfer;
        particleResDic[particle].GetComponent<ParticleSystem>().Play();
    }
}
