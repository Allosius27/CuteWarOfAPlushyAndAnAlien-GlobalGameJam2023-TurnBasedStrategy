using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowHighlight : MonoBehaviour
{
    #region Fields

    private Dictionary<Renderer, Material[]> _glowMaterialDictionary = new Dictionary<Renderer, Material[]>();
    private Dictionary<Renderer, Material[]> _originalMaterialDictionary = new Dictionary<Renderer, Material[]>();
    private Dictionary<Color, Material> _cachedGlowMaterials = new Dictionary<Color, Material>();

    private bool _isGlowing = false;

    #endregion

    #region UnityInspector

    public Material glowMaterial;

    #endregion

    #region Behaviour

    private void Awake()
    {
        PrepareMaterialDictionaries();
    }

    private void PrepareMaterialDictionaries()
    {
        foreach(Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            Material[] originalMaterials = renderer.materials;
            _originalMaterialDictionary.Add(renderer, originalMaterials);

            Material[] newMaterials = new Material[renderer.materials.Length];

            for (int i = 0; i < originalMaterials.Length; i++)
            {
                Material mat = null;
                if (_cachedGlowMaterials.TryGetValue(originalMaterials[i].color, out mat) == false)
                {
                    mat = new Material(glowMaterial);
                    // By default, Unity considers a color with the property name name "_Color" to be the main color
                    mat.color = originalMaterials[i].color;
                }
                newMaterials[i] = mat;
            }
            _glowMaterialDictionary.Add(renderer, newMaterials);
        }
    }

    public void ToggleGlow()
    {
        if(_isGlowing == false)
        {
            foreach(Renderer renderer in _originalMaterialDictionary.Keys)
            {
                renderer.materials = _glowMaterialDictionary[renderer];
            }
        }
        else
        {
            foreach(Renderer renderer in _originalMaterialDictionary.Keys)
            {
                renderer.materials = _originalMaterialDictionary[renderer];
            }
        }
        _isGlowing = !_isGlowing;
    }

    public void ToggleGlow(bool state)
    {
        if(_isGlowing == state)
        {
            return;
        }
        _isGlowing = !state;
        ToggleGlow();
    }

    #endregion
}
