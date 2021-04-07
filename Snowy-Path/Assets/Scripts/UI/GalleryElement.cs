using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gallery/Element")]
public class GalleryElement : ScriptableObject
{
    public string title;

    [Multiline]
    public string description;

    public Texture texture;
}
