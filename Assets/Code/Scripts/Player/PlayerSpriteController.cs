using UnityEngine;

public class PlayerSpriteController : MonoBehaviour
{
    public Transform pSprite;
    public Transform cam;
    public Transform spriteTarget;
    SpriteRenderer sp;

    private void Start() {
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
