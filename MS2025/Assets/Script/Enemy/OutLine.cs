using Fusion;
using UnityEngine;

public class OutLine : NetworkBehaviour
{
    SpriteRenderer renderer;
    Sprite sprite;
    GameObject parent;
    SpriteRenderer parentrenderer;
    Sprite parentsprite;
    public override void Spawned()
    {
        renderer = GetComponent<SpriteRenderer>();
        sprite = renderer.sprite;
        parent= transform.parent.gameObject;
        parentrenderer= parent.GetComponent<SpriteRenderer>();
        parentsprite= parentrenderer.sprite;
    }
    public override void FixedUpdateNetwork()
    {
        parentsprite = parentrenderer.sprite;
        sprite=parentsprite;
        Color color = Color.green;
        renderer.color = color;

        renderer.sprite=sprite;
        Debug.Log("‚Ú‚·‚·‚Õ‚ç‚¢‚Æ" + sprite.name);
    }
    public override void Render()
    {
         
    }
}
