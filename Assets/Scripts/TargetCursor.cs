using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TargetCursor : MonoBehaviour
{
    public Transform tf;
    public SpriteRenderer sr;

    public float transparencyFactor = 1f;
    public float scaleFactor = 1f;

    public Vector3 scaleTarget = Vector3.zero;

    Color defColor;
    Vector3 defScale;

    // Start is called before the first frame update
    void Start()
    {
        if (tf == null)
            tf = base.transform;

        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        defColor = sr.color;
        Color c = sr.color;
        c.a = 0;
        sr.color = c;

        defScale = tf.localScale;

    }

    public void Update()
    {
        if(GameManager.instance.gameState != GameState.Pause)
        {
            //Moves the target cursor to your mouse when you click the mouse button as well as sets the transparency to 1.
            if (Input.GetMouseButton(0))
            {
                tf.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                sr.color = defColor;
                tf.localScale = defScale;
            }
        }


        //Fades out the target cursor after clicking.
        if (sr.color.a > 0 && !Input.GetMouseButton(0))
        {
            Color c = sr.color;
            c.a -= Time.deltaTime * transparencyFactor;
            sr.color = c;
        }

        //Scales the cursor after clicking.
        if (tf.localScale.x > 0 && !Input.GetMouseButton(0))
        {
            Vector3 result = tf.localScale;
            result.x -= Time.deltaTime * scaleFactor;
            result.y -= Time.deltaTime * scaleFactor;
            tf.localScale = result;
        }

        if(GameManager.instance.gameState == GameState.Fail)
        {
            this.gameObject.SetActive(false);
        }
    }
}
