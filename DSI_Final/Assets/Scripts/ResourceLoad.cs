using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class ResourceLoad : MonoBehaviour
{
    private static Dictionary<string, Sprite> personajeAssets = new Dictionary<string, Sprite>();
    List<string> listaPersonajeAssets;

    private static Dictionary<string, Sprite> iconosCasco = new Dictionary<string, Sprite>();
    private static List<string> listaIconosCasco;

    private static Dictionary<string, Sprite> iconosOjos = new Dictionary<string, Sprite>();
    private static List<string> listaIconosOjos;

    private static Dictionary<string, Sprite> iconosArma = new Dictionary<string, Sprite>();
    private static List<string> listaIconosArma;

    private static Dictionary<string, Sprite> iconosBotas = new Dictionary<string, Sprite>();
    private static List<string> listaIconosBotas;

    void Awake()
    {
        string jsonText = Resources.Load<TextAsset>("json/resources").ToString();
        if (jsonText == null)
        {
            Debug.LogError("No se encontró archivo en Resources.");
            return;
        }

        listaIconosCasco = JsonUtility.FromJson<List<string>>(jsonText);
        listaIconosOjos = JsonUtility.FromJson<List<string>>(jsonText);
        listaIconosArma = JsonUtility.FromJson<List<string>>(jsonText);
        listaIconosBotas = JsonUtility.FromJson<List<string>>(jsonText);

        foreach (var name in listaPersonajeAssets)
        {
            Sprite sprite = Resources.Load<Sprite>(name);
            if (sprite != null)
            {
                personajeAssets[name] = sprite;
            }
            else
            {
                Debug.LogWarning($"Sprite no encontrado: {name}");
            }
        }

        foreach (var name in listaIconosCasco)
        {
            Sprite sprite = Resources.Load<Sprite>(name);
            if (sprite != null)
            {
                iconosCasco[name] = sprite;
            }
            else
            {
                Debug.LogWarning($"Sprite no encontrado: {name}");
            }
        }

        foreach (var name in listaIconosOjos)
        {
            Sprite sprite = Resources.Load<Sprite>(name);
            if (sprite != null)
            {
                iconosOjos[name] = sprite;
            }
            else
            {
                Debug.LogWarning($"Sprite no encontrado: {name}");
            }
        }

        foreach (var name in listaIconosArma)
        {
            Sprite sprite = Resources.Load<Sprite>(name);
            if (sprite != null)
            {
                iconosArma[name] = sprite;
            }
            else
            {
                Debug.LogWarning($"Sprite no encontrado: {name}");
            }
        }

        foreach (var name in listaIconosBotas)
        {
            Sprite sprite = Resources.Load<Sprite>(name);
            if (sprite != null)
            {
                iconosBotas[name] = sprite;
            }
            else
            {
                Debug.LogWarning($"Sprite no encontrado: {name}");
            }
        }
    }

    public static Sprite GetPersonajeAsset(string name)
    {
        if (personajeAssets.TryGetValue("Images/Personaje/" + name, out var sprite))
        {
            return sprite;
        }
        Debug.LogError($"Sprite '{name}' no cargado.");
        return null;
    }

    public static Sprite GetIcono(string name, int type)
    {
        switch(type)
        {
            case 0:
                if (iconosCasco.TryGetValue("Images/Iconos/Accesorios/Casco" + name, out var spriteC))
                {
                    return spriteC;
                }
                break;
            case 1:
                if (iconosCasco.TryGetValue("Images/Iconos/Accesorios/Ojos" + name, out var spriteO))
                {
                    return spriteO;
                }
                break;
            case 2:
                if (iconosCasco.TryGetValue("Images/Iconos/Accesorios/Arma" + name, out var spriteA))
                {
                    return spriteA;
                }
                break;
            case 3:
                if (iconosCasco.TryGetValue("Images/Iconos/Accesorios/Botas" + name, out var spriteB))
                {
                    return spriteB;
                }
                break;
        }
        
        Debug.LogError($"Sprite '{name}' no cargado.");
        return null;
    }
}