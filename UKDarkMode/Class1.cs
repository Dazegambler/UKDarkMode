using System;
using System.Collections;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UKDarkmode
{
    [BepInPlugin("UK.Dark", "UKDark", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        GameObject Light, Cam,Ray, Particle,point,Spot;
        GameObject[] Objects;

        public void Start()
        {
        }
        public void Update()
        {
            Scene ACTIVE = SceneManager.GetActiveScene();
            if (Time.timeSinceLevelLoad > UnityEngine.Random.Range(4, 6) && ACTIVE.name != "Main Menu" && ACTIVE.name != "Intro")
            {
                DarkMode();
            }
        }
        private void DarkMode()
        {
            if (Cam == null)
            {
                Cam = GameObject.Find("Main Camera");
            }
            if (Light == null)
            {
                Light = ObjFind("LightShaft", Light);
            }
            if (Cam != null && Light != null)
            {
                InstFlashLight();
                LightUpdate();
            }
        }
        private void InstFlashLight()
        {
            Ray = GameObject.Find("LightShaft(Clone)");
            if (Ray == null)
            {
                Instantiate(Light, new Vector3(Cam.transform.position.x - 2, Cam.transform.position.y + 1, Cam.transform.position.z),new Quaternion(181,0,0,0));
            }
            else if(Ray != null)
            {
                Ray.SetActive(true);
                Ray.layer = 22;
                Ray.transform.SetParent(Cam.transform);
                Spot = GameObject.Find("LightShaft(Clone)/Spot Light");
                GameObject Light = GameObject.Find("FirstRoom/Player/Main Camera/LightShaft(Clone)");
                if(Light != null)
                {
                    var aud1 = Light.GetComponent<AudioSource>().enabled = false;
                    var aud2 = Light.GetComponent<AudioSource>().enabled = false;
                }
                if (Spot != null)
                {
                    Spot.layer = 22;
                    Spot.SetActive(true);
                    var light = Spot.GetComponent<Light>();
                    if (light != null)
                    {
                        light.spotAngle = 40;
                        light.intensity = 20;
                    }
                }
                Particle = GameObject.Find("FirstRoom/Player/Main Camera/LightShaft(Clone)/Particle System");
                if (Particle != null)
                {
                    Particle.SetActive(false);
                }
                point = GameObject.Find("FirstRoom/Player/Main Camera/LightShaft(Clone)/Point Light");
                if (point != null)
                {
                    point.SetActive(false);
                }
            }
        }
        private GameObject ObjFind(string name, GameObject Original)
        {
            //Find set Object in the prefabs
            GameObject[] pool = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject obj in pool)
            {
                if (obj.gameObject.name == name)
                {
                    Original = obj;
                }
            }
            return Original;
        }
        private void LightUpdate()
        {
            LayerMask mask = LayerMask.GetMask("Projectile","Ignore Raycast");
            Objects = GameObject.FindObjectsOfType<GameObject>();
            if (Objects != null)
            {
                foreach (GameObject obj in Objects)
                {
                    if (obj.name != "LightShaft(Clone)" && obj.activeSelf == true && obj.layer != 14 && obj.layer != 2 && obj.layer != 22)
                    {
                        var lght = obj.GetComponentInChildren<Light>();
                        lght = obj.GetComponent<Light>();
                        if (lght != null && lght.gameObject.activeSelf == true && lght.intensity > 0)
                        {
                           lght.intensity = 0f;
                        }
                    }
                }
            }

        }
    }
}
