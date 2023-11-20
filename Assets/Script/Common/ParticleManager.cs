using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : CSingel<ParticleManager>
{
    GameObject parent;
    private Dictionary<E_Particle, GameObject> particleResDic;
    private Dictionary<E_Particle, GameObject> particleDic;
    string PARTICLE_PATH = "ui/particle/";
    public void init()
    {
        particleResDic = new Dictionary<E_Particle, GameObject>();
        particleDic = new Dictionary<E_Particle, GameObject>();
    }
    public GameObject getEffect(E_Particle particle)
    {
        GameObject result;
        if (!particleResDic.ContainsKey(particle))
        {
            GameObject obj = AssetManager.loadAsset<GameObject>(PARTICLE_PATH + particle.ToString());
            particleResDic.Add(particle, obj);
        }
        result = UnityEngine.Object.Instantiate(particleResDic[particle]);
        result.SetActive(false);
        return result;
    }
    //粒子类播放
    public void playEffect(E_Particle particle,Vector3 transfer)
    {
        checkParticle(particle);
        particleResDic[particle].transform.position = transfer;
        particleResDic[particle].GetComponent<ParticleSystem>().Play();
    }
    public GameObject getPlayEffect(E_Particle particle, Vector3 transfer)
    {
        checkParticle(particle);
        particleResDic[particle].transform.position = transfer;
        particleResDic[particle].SetActive(true);
        particleResDic[particle].GetComponent<ParticleSystem>().Play();
        return particleResDic[particle];
    }
    void checkParticle(E_Particle particle)
    {
        if (!particleResDic.ContainsKey(particle))
        {
            GameObject res = AssetManager.loadAsset<GameObject>(PARTICLE_PATH + particle.ToString());
            var obj = UnityEngine.Object.Instantiate(res);
            if (parent == null)
            {
                parent = new GameObject("Particles");
                parent.transform.SetParent(PanelManager.Instance.maincanvas.transform);
                parent.transform.localScale = Vector3.one;
            }
            obj.transform.SetParent(parent.transform);
            obj.transform.localScale = Vector3.one;
            particleResDic.Add(particle, obj);
        }
    }
    //带动画类播放
    public void playEffect_special(E_Particle particle, Vector3 transfer,string text,Action callback=null)
    {
        callback?.Invoke();
        checkParticle(particle);
        particleResDic[particle].transform.position = transfer;
        particleResDic[particle].GetComponent<ParticleBase>().play(text, particle.ToString(), callback);
    }
}
