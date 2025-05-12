using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Utilities/Icon Maker")]
public class IconMakerAsset : ScriptableObject
{
    public GameObject iconMakerGameObject;
    public SpriteMeshType spriteMeshType;
    public int renderLayer = 26;
    public RenderTexture renderTexture;

    public void RequestIconForList(List<IICon> l, IconMaker.OnIconComplete callback = null)
    {
        GameObject go = Instantiate(iconMakerGameObject);
        IconMakerActual iconMakerActual = go.GetComponentInChildren<IconMakerActual>();
        iconMakerActual.CreateIconsForList(l, this, callback);
    }

    public void RequestIcon(IICon targetObject, IconMaker.OnIconComplete callback = null)
    { 
        GameObject go = Instantiate(iconMakerGameObject);
        IconMakerActual iconMakerActual = go.GetComponentInChildren<IconMakerActual>();
        iconMakerActual.CreateIcon(targetObject, this, callback);
    }
}

public static class IconMaker
{
    static IconMakerAsset _iconMakerAsset;
    public delegate void OnIconComplete();

    public static void RequestIconForList(List<IICon> targetList, OnIconComplete iconCompleteCallback)
    {
        if (_iconMakerAsset == null)
        {
            _iconMakerAsset = Resources.Load("IconMakerAsset") as IconMakerAsset;
        }

        _iconMakerAsset.RequestIconForList(targetList, iconCompleteCallback);
    }

    public static void RequestIcon(IICon targetObject, OnIconComplete iconCompleteCallback)
    {
        if (_iconMakerAsset == null)
        {
            _iconMakerAsset = Resources.Load("IconMakerAsset") as IconMakerAsset;
        }

        _iconMakerAsset.RequestIcon(targetObject, iconCompleteCallback);
    }

    public static void RequestIcon(IICon targetObject)
    {
        if (_iconMakerAsset == null)
        { 
            _iconMakerAsset = Resources.Load("IconMakerAsset") as IconMakerAsset;   
        }

        _iconMakerAsset.RequestIcon(targetObject);
    }
}  