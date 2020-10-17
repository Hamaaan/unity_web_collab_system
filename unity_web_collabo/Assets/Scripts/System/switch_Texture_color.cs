using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switch_Texture_color : MonoBehaviour
{
    Renderer rend;

    MeshRenderer mrend;

    /*
    [SerializeField] Material BaseColor;

    [SerializeField] Material Texture;
    */

    //public Color BaseColor = new Color(1, 1, 1, 1);

    //public Color TextureColor = new Color(1, 1, 1, 1);

    public int Texture_ID ;

    [SerializeField] Texture2D[] Textures;

    public int BaseColor_ID;

    public int TextureColor_ID;

    [SerializeField] Color[] Colors;

    // Start is called before the first frame update
    void Start()
    {
        ColorChanger();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ColorChanger()
    {
        rend = this.gameObject.GetComponent<Renderer>();

        rend.materials[0].color = Colors[BaseColor_ID];

        mrend = this.gameObject.GetComponent<MeshRenderer>();

        rend.materials[1].color = Colors[TextureColor_ID];

        rend.materials[1].mainTexture = Textures[Texture_ID];
    }
}
