using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
#endif

/// <summary>
/// 明确类型的接口、一些业务相关的接口
/// </summary>
public partial class AddressModel
{
    public static GameObject LoadGameObject(string address, Transform parent = null, bool track = true, bool active = true)
    {
        GameObject template = Instance.LoadAsset<GameObject>(address);
        if(template == null)
        {
            return template;
        }
        GameObject instance = GameObjectInstanceFromTemplate(address, template, parent, track, active);
        return instance;
    }

    public static T LoadComponent<T>(string address, Transform parent = null, bool addComponent = false, bool track = true, bool active = true)
        where T: MonoBehaviour
    {
        GameObject template = Instance.LoadAsset<GameObject>(address);
        if (template == null)
        {
            return null;
        }
        GameObject obj = GameObjectInstanceFromTemplate(address, template, parent, track, active);
        if (addComponent)
        {
            return obj.GetOrAddComponent<T>();
        }
        else
        {
            return obj.GetComponent<T>();
        }
    }

    public static bool Has(string address)
    {
        return Instance.HasAddress(address);
    }

    static GameObject GameObjectInstanceFromTemplate(string address, GameObject template, Transform parent, bool track = true, bool active = true)
    {
        bool templateChanged = false;
        if (template.activeSelf != active)
        {
            template.SetActive(active);
            templateChanged = true;
        }

        GameObject instance = GameObject.Instantiate(template, parent, false);

#if UNITY_EDITOR
        if (templateChanged)
        {
            template.SetActive(!active);
            EditorUtility.ClearDirty(template);
        }
#endif
        if (track)
        {
            AddressTrack addressTrack = instance.GetOrAddComponent<AddressTrack>();
            addressTrack.Address(address);
        }

#if UNITY_EDITOR  // Editor Bundle Shader err
        if (AddressableAssetSettingsDefaultObject.Settings.ActivePlayModeDataBuilderIndex == 2)
        {
            TextMeshProUGUI[] textMeshs = instance.GetComponentsInChildren<TextMeshProUGUI>();

            for (int i = 0; i < textMeshs.Length; i++)
            {
                TextMeshProUGUI textMeshProUGUI = textMeshs[i];
                textMeshProUGUI.fontSharedMaterial.shader = Shader.Find(textMeshProUGUI.fontSharedMaterial.shader.name);
            }

            ParticleSystem[] particleSystems = instance.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < particleSystems.Length; i++)
            {
                ParticleSystem particleSystem = particleSystems[i];
                Renderer render = particleSystem.GetComponent<Renderer>();

                if (render != null)
                {
                    render.sharedMaterial.shader = Shader.Find(render.sharedMaterial.shader.name);
                }
            }

            MeshRenderer[] meshRenderers = instance.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                MeshRenderer meshRenderer = meshRenderers[i];
                meshRenderer.sharedMaterial.shader = Shader.Find(meshRenderer.sharedMaterial.shader.name);
            }
        }
#endif
        return instance;
    }

    public static void LoadGameObjectAsync(string address, Transform parent, Action<GameObject> callBack, bool track = true, bool active = true)
    {
        Instance.LoadAssetAsync<GameObject>(address, (_result) =>
        {
            GameObject template = (GameObject)_result;
			if(template == null)
			{
				callBack?.Invoke(template);
				return;
			}
            GameObject instance = GameObjectInstanceFromTemplate(address, template, parent, track, active);
            callBack?.Invoke(instance);
        });
    }

    static Sprite spriteDefault;
    public static void LoadSpriteAsync(string address, Action<Sprite> callBack)
    {
        Instance.LoadAssetAsync<Sprite>(address, (_result) =>
        {
            Sprite result = (Sprite)_result;

			if (result == null)
			{
				if (spriteDefault == null)
				{
					var rect = new Rect(0, 0, Texture2D.whiteTexture.width, Texture2D.whiteTexture.height);
					var pivot = new Vector2(0.5f, 0.5f);
					spriteDefault = Sprite.Create(Texture2D.redTexture, rect, pivot);
					spriteDefault.name = "Asset_SpriteDefault";
				}
				result = spriteDefault;
			}
            callBack?.Invoke(result);
        });
    }

    public static void SetSpriteAsync(Image image, string address, bool setNativeSize = false, Action callBack = null)
    {
        if (image == null)
        {
            Debug.LogError("image is null,when set sprite:" + address);
            callBack?.Invoke();
            return;
        }

        LoadSpriteAsync(address, (_result) =>
        {
            if (image != null)
            {
                image.sprite = _result;

                if (setNativeSize)
                {
                    image.SetNativeSize();
                }
            }
            else
            {
                Debug.LogError("image is destroyed, when set sprite:" + address);
            }

            callBack?.Invoke();
        });
    }

    public static Texture LoadTexture(string address)
    {
        return Instance.LoadAsset<Texture>(address);
    }

    public static Material LoadMaterial(string address)
    {
        return Instance.LoadAsset<Material>(address);
    }
}