using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery : MonoBehaviour
{
    public List<Color> imageList;

    GameObject m_galleryContent;

    void Start()
    {
        m_galleryContent = GetComponentInChildren<GridLayoutGroup>().gameObject;

        foreach (var image in imageList) {
            GameObject galleryImage = new GameObject("GalleryImage");
            galleryImage.transform.parent = m_galleryContent.transform;

            Image img = galleryImage.AddComponent<Image>();
            img.color = Random.ColorHSV(); // FIXME: image unused
        }
    }
}
