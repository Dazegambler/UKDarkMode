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
        GameObject Light = null, Cam = null, Ray = null, Particle = null, point = null, Spot = null;
        GameObject[] Objects;

        public void Start()
        {
        }
        public void Update()
        {
            Scene ACTIVE = SceneManager.GetActiveScene();
            if (Time.timeSinceLevelLoad > 1.5f && Time.timeSinceLevelLoad < 1.95f && ACTIVE.name != "Main Menu" && ACTIVE.name != "Intro")
            {
                Objects = GameObject.FindObjectsOfType<GameObject>();
                Cam = null;
                Light = null;
                DarkMode();
            }
            if (Time.timeSinceLevelLoad > 1.5f && ACTIVE.name != "Main Menu" && ACTIVE.name != "Intro")
            {
                LightUpdate();
            }
        }
        private void FindLight()
        {
            if(Cam == null && Light == null)
            {
                Cam = GameObject.Find("Main Camera");
                Light = ObjFind("LightShaft", Light);
            }
            else
            {
            }
        }
        private void DarkMode()
        {
            FindLight();
            switch (Cam && Light)
            {
                case false:
                    break;
                default:
                    InstFlashLight();
                    break;
            }
        }
        private void InstFlashLight()
        {
            Vector3 spawnpos = new Vector3(Cam.transform.position.x - 2, Cam.transform.position.y + 1.25f, Cam.transform.position.z);
            Quaternion Rotation = new Quaternion(Cam.transform.rotation.x + 0.75f, 0, 0, 0);
            Ray = GameObject.Find("Main Camera/LightShaft(Clone)");
            switch (Ray) 
            {
                case null:
                    Instantiate(Light, spawnpos, Rotation, Cam.transform);
                    Ray.transform.forward *= -1;
                    Ray.transform.localPosition = new Vector3(-2, 1.25f, 0);
                    break;
                default:
                    StartSpotlight();
                    switch (Spot)
                    {
                        case null:
                            break;
                        default:
                            SetSpotLight();
                            break;
                    }
                    Deactivate("Main Camera/LightShaft(Clone)/Particle System", Particle);
                    Deactivate("Main Camera/LightShaft(Clone)/Point Light",point);
                    break;
            }
        }
        private void StartSpotlight()
        {
            //Ray.SetActive(true);
            Ray.layer = 22;
            var aud1 = Ray.GetComponent<AudioSource>().enabled = false;
            Spot = GameObject.Find("Main Camera/LightShaft(Clone)/Spot Light");
            switch (Spot)
            {
                case null:
                    break;
                default:
                    Spot.layer = 22;
                    Spot.SetActive(true);
                    var light = Spot.GetComponent<Light>();
                    switch (light)
                    {
                        case null:
                            break;
                        default:
                            light.spotAngle = 40;
                            light.intensity = 10;
                            break;
                    }
                    break;
            }
        }
        private void SetSpotLight()
        {
            Spot.layer = 22;
            Spot.SetActive(true);
            var light = Spot.GetComponent<Light>();
            switch (light)
            {
                case null:
                    break;
                default:
                    light.spotAngle = 40;
                    light.intensity = 7.5f;
                    break;
            }
        }
        private void Deactivate(string txt,GameObject obj)
        {
            obj = GameObject.Find(txt);
            switch (obj)
            {
                case null:
                    break;
                default:
                    obj.SetActive(false);
                    break;

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
            Objects = GameObject.FindObjectsOfType<GameObject>();
            if (Objects != null)
            {
                foreach (GameObject obj in Objects)
                {
                    switch (obj.name)
                    {
                        case "LightShaft(Clone)":
                            break;
                        default:
                            switch (obj.activeSelf)
                            {
                                case true:
                                    lightsoff(obj);
                                    break;
                                default:
                                    break;
                            }
                            break;
                    }
                    matChange(obj);
                }
            }
        }
        private void lightsoff(GameObject obj)
        {
            switch (obj.layer)
            {
                case 14:
                    break;
                case 2:
                    break;
                case 22:
                    break;
                default:
                    var lght = obj.GetComponentInChildren<Light>();
                    lght = obj.GetComponent<Light>();
                    switch (lght)
                    {
                        case null:
                            break;
                        default:
                            switch (lght.enabled)
                            {
                                case true:
                                    lght.enabled = false;
                                    break;
                                default:
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }
        private void matChange(GameObject obj)
        {
            Shader rep = Shader.Find("psx/vertexlit/vertexlit");
                switch (obj.tag)
                {
                    case "Enemy":
                        var type = obj.GetComponent<EnemyIdentifier>().enemyType;
                        switch (type)
                        {
                            case EnemyType.Virtue:
                                MeshRenderer[] meshes = obj.GetComponentsInChildren<MeshRenderer>();
                                foreach (MeshRenderer mesh in meshes)
                                {
                                    switch (mesh)
                                    {
                                        case null:
                                            break;
                                        default:
                                            switch (mesh.gameObject.name)
                                            {
                                                case "Sphere":
                                                    break;
                                                default:
                                                    mesh.material.shader = rep;
                                                    break;
                                            }
                                            break;
                                    }
                                }
                                break;
                            case EnemyType.MinosPrime:
                                SkinnedMeshRenderer[] skinnedmeshes = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
                                foreach (SkinnedMeshRenderer skin in skinnedmeshes)
                                {
                                    switch (skin)
                                    {
                                        case null:
                                            break;
                                        default:
                                            switch (skin.gameObject.name)
                                            {
                                                case "MinosPrime_Body.001":
                                                    break;
                                                default:
                                                    skin.material.shader = rep;
                                                    break;
                                            }
                                            break;
                                    }
                                }
                                break;
                            case EnemyType.Mindflayer:
                                break;
                            case EnemyType.Swordsmachine:
                                SkinnedMeshRenderer[] _skinnedmeshes = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
                                foreach (SkinnedMeshRenderer skin in _skinnedmeshes)
                                {
                                    switch (skin)
                                    {
                                        case null:
                                            break;
                                        default:
                                            skin.material.shader = rep;
                                            break;
                                    }
                                }
                                break;
                        case EnemyType.Drone:
                            MeshRenderer[] rends = obj.GetComponentsInChildren<MeshRenderer>();
                            foreach(MeshRenderer rend in rends)
                            {
                                switch (rend.gameObject.name)
                                {
                                    case "Body":
                                        rend.material.shader = rep;
                                        break;
                                    case "Gib_Eyeball":
                                        rend.material.shader = rep;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case EnemyType.Sisyphus:
                            SkinnedMeshRenderer[] _sisy = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
                            foreach (SkinnedMeshRenderer skin in _sisy)
                            {
                                skin.material.shader = rep;
                            }
                                break;
                        case EnemyType.Minos:
                            MeshRenderer[] _rends = obj.GetComponentsInChildren<MeshRenderer>();
                            foreach (MeshRenderer rend in _rends)
                            {
                                switch (rend.gameObject.name)
                                {
                                    case "Cube (3)":
                                        rend.gameObject.SetActive(false);
                                        break;
                                    case "Cube (2)":
                                        rend.gameObject.SetActive(false);
                                        break;
                                    default:
                                        Material _Mat = obj.GetComponentInChildren<SkinnedMeshRenderer>().material;
                                        switch (_Mat)
                                        {
                                            case null:
                                                _Mat = obj.GetComponentInChildren<MeshRenderer>().material;
                                                break;
                                            default:
                                                if (_Mat.shader.name != rep.name)
                                                {
                                                    _Mat.shader = rep;
                                                }
                                                break;
                                        }
                                        break;
                                }
                            }
                            break;
                            default:
                                Material Mat = obj.GetComponentInChildren<SkinnedMeshRenderer>().material;
                                switch (Mat)
                                {
                                    case null:
                                        Mat = obj.GetComponentInChildren<MeshRenderer>().material;
                                        break;
                                    default:
                                        if (Mat.shader.name != rep.name)
                                        {
                                            Mat.shader = rep;
                                        }
                                        break;
                                }
                                break;
                        }
                        break;
                    default:
                        break;
                }
        }
        private void flick(GameObject obj)
        {
            GameObject light = null;
            if(light == null)
            {
                light = obj.GetComponentInChildren<Light>().gameObject;
                light = obj.GetComponent<Light>().gameObject;
            }
            else if(light != null)
            {
                var lght = light.GetComponent<Light>();
                light.AddComponent<Flicker>();
                var flick = light.GetComponent<Flicker>();
                if (flick != null)
                {
                    int n = UnityEngine.Random.Range(0, 1);
                    switch (n)
                    {
                        case 0:
                            flick.quickFlicker = false;
                            break;
                        case 1:
                            flick.quickFlicker = true;
                            break;
                    }
                    flick.delay = UnityEngine.Random.Range(1,7);
                    flick.intensityRandomizer = lght.intensity;
                    flick.timeRandomizer = UnityEngine.Random.Range(1,3);
                    flick.enabled = true;
                }
            }
        }
        private void doorcheck()
        {
            GameObject[] pool = GameObject.FindObjectsOfType<GameObject>();
            foreach(GameObject obj in pool)
            {
                var door = obj.GetComponent<Door>();
                door = obj.GetComponentInChildren<Door>();
                if (door)
                {

                }
            }
        }
    }
}
