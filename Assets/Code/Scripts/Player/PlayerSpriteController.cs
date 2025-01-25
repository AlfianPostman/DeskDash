using UnityEngine;
using System.Collections;

public class PlayerSpriteController : MonoBehaviour
{
    public Transform pSprite;
    public Transform spriteTarget;
    GameObject cam;
    SpriteRenderer sp;

    private void Start() 
    {
        cam = GameObject.FindWithTag("Camera");
        sp = pSprite.GetComponent<SpriteRenderer>();
    }
    
    private void Update() {
        pSprite.transform.position = spriteTarget.transform.position;

        pSprite.transform.LookAt(cam.transform);
        pSprite.transform.rotation = Quaternion.Euler(0, pSprite.transform.rotation.eulerAngles.y,0);
    }

    public void Flip()
    {
        Vector3 theScale = pSprite.transform.localScale;
        theScale.x *= -1;
        pSprite.transform.localScale = theScale;
    }
}
