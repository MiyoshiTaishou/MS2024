using UnityEngine;

class animTest : MonoBehaviour{
    public bool animStart;
    public  TextureAnimation textureAnimation;
    private void Start() {
        textureAnimation = GetComponent<TextureAnimation>();
    }
    private void Update() {
        if(animStart == true){
            textureAnimation.StartAnimation();
            animStart = false;
        }
    }
}