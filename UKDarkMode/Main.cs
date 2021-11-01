using System;
using System.Collections;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.SceneManagement;
using General_Unity_Tools;

namespace UKDarkmode
{
    [BepInPlugin("UK.Dark", "UKDark", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {

        ConfigEntry<bool> SpookyMode;

        List<EnemyType> 
            AshBlackList = new List<EnemyType>{
            EnemyType.Minos,
            EnemyType.V2,
            EnemyType.FleshPrison,
            EnemyType.MinosPrime,
            EnemyType.Drone,
            EnemyType.Mindflayer,
            };

        GameObject
            Ash,
            Guns,
            Light;

        List<GameObject> NoNo;

        EnemyIdentifier[]
            eids;

        Shader rep;

        Light[]
            lights;

        public void Start()
        {
            SpookyMode = Config.Bind("", "Spooky Mode",false);
            rep = Shader.Find("psx/vertexlit/vertexlit");
        }
        public void Update()
        {
            Scene ACTIVE = SceneManager.GetActiveScene();
            if (Time.timeSinceLevelLoad > 1.5f && Time.timeSinceLevelLoad < 2.5f && ACTIVE.name != "Main Menu" && ACTIVE.name != "Intro")
            {
                if (Light == null)
                {
                    InstFlashLight();
                    Guns = GameObject.Find("Guns");
                    Ash = DazeExtensions.PrefabFind("AshParticle");
                }
            }
            if (Time.timeSinceLevelLoad > 1.65f && ACTIVE.name != "Main Menu" && ACTIVE.name != "Intro")
            {
                lights = FindObjectsOfType<Light>();
                eids = FindObjectsOfType<EnemyIdentifier>();
                foreach (var eid in eids)
                {
                    matChange(eid);
                }
                EnemyAsher();
                LightUpdate(lights);
                Light.transform.forward = -Guns.transform.forward;
                if (SpookyMode.Value == true)
                {
                    ToggleFlashlight();
                }
            }
        }

        public void LateUpdate()
        {
            var scn = SceneManager.GetActiveScene();
            if(scn.name != "Main Menu" && scn.name != "Intro")
            {
            }
        }

        public void InstFlashLight()
        {
            NoNo = new List<GameObject>();
            var Cam = GameObject.Find("Main Camera").transform;
            GameObject LightShaft = DazeExtensions.PrefabFind("LightShaft");
            Light = Instantiate(LightShaft,new Vector3(0,0,0), new Quaternion(0, 0, 0, 0), Cam);
            Light.transform.localPosition = new Vector3(-2,.75f,0);

            NoNo.Add(Light);
            foreach(var a in Light.ListChildren())
            {
                NoNo.Add(a.gameObject);
            }

            Light.transform.FindInChildren("Particle System").gameObject.SetActive(false);
            Light.transform.FindInChildren("Point Light").gameObject.SetActive(false);
            Light.GetComponent<AudioSource>().enabled = false;

            var b = Light.transform.FindInChildren("Spot Light");
            b.gameObject.SetActive(true);
            b.GetComponent<Light>().spotAngle = 40;
            b.GetComponent<Light>().intensity = 7.5f;

        }

        private void LightUpdate(Light[] lights)
        {
            foreach (var obj in lights)
            {
                lightsoff(obj);
            }
        }
        private void lightsoff(Light obj)
        {
            var a = obj.gameObject.layer;
            if(a != 14 && a != 2 && a != 22 && !NoNo.Contains(obj.gameObject))
            {
                obj.enabled = false;
            }
        }
        private void matChange(EnemyIdentifier obj)
        {
            var type = obj.enemyType;
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
                    foreach (MeshRenderer rend in rends)
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
        }

        void EnemyAsher()
        {
            foreach(var eid in eids)
            {
                if (eid.dead && !AshBlackList.Contains(eid.enemyType))
                {
                    foreach(var part in eid.gameObject.ListChildren())
                    {
                        Instantiate(Ash,part.position,Quaternion.identity);
                        Destroy(part.gameObject);
                    }
                }
            }
        }

        void ToggleFlashlight()
        {
            var En = Light.activeSelf;
            var sure = UnityEngine.Random.Range(1,250);
            Light.GetComponents<AudioSource>()[1].enabled = false;
            if(sure == 1)
            {
                Light.SetActive(!En);
            }
        }
    }
}
