using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopterMovement : MonoBehaviour
{

    //references
    [SerializeField] private Camera mainCam;
    [SerializeField] private UIManager uiMan;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject helibackPrefab;
    [SerializeField] private GameObject smokePrefab;
    [SerializeField] private PolygonCollider2D frontCollider;
    private float smokeCounter = 0;

    //variables we use
    public float currentUpSpeed = 0;
    public float upSpeed;
    public bool gameGoing = true;
    [SerializeField] private float currentTilt;
    [SerializeField] private float maxTilt;
    [SerializeField] private float minTilt;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        smokeCounter = 1;
        //gameGoing = true;
        mainCam = Camera.main;
        StartCoroutine(BeginningPause());
        currentTilt = 0;
        maxTilt = 400;
        minTilt = -400;
    }

    private void Update()
    {
        if (gameGoing)
        {
            mainCam.gameObject.transform.position = new Vector3(gameObject.transform.position.x + 6, 0, -10);
            if (Input.GetMouseButton(0))
            {
                smokeCounter += 1 * Time.deltaTime;
            }
            if (smokeCounter >= .1f)
            {
                smokeCounter = 0;
                GameObject go = Instantiate(smokePrefab, gameObject.transform.position + new Vector3(-0.7f, -0.1f, 0), Quaternion.identity);
                Destroy(go, 0.5f);
            }
        } 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameGoing)
        {
            if (Input.GetMouseButton(0))
            {
                //add tilt
                if (currentTilt < maxTilt)
                {
                    gameObject.transform.Rotate(new Vector3(0, 0, 0.4f));
                    currentTilt += 1;
                }
                currentUpSpeed += upSpeed * Time.deltaTime;
            }
            else
            {
                //reduce tilt
                if (currentTilt > minTilt)
                {
                    gameObject.transform.Rotate(new Vector3(0, 0, -0.4f));
                    currentTilt -= 1;
                }
                currentUpSpeed -= upSpeed * Time.deltaTime;
            }
            transform.position += new Vector3(8 * Time.deltaTime, currentUpSpeed, 0);
        }
    }

    public void AddPoints (int points)
    {
        score += points;
        uiMan.UpdateScore(score);
    }

    IEnumerator BeginningPause ()
    {
        gameGoing = false;
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        gameGoing = true;
        uiMan.HideTapToStart();
    }

    bool deathHappened = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!deathHappened)
        {
            deathHappened = true;
            Death();
        }
            
    }
    
    private void Death()
    {
        //stop game's progression
        gameGoing = false;
        //trigger game over
        uiMan.GameOver(score);
        Debug.Log("game over");

        //change sprite
        Animator anim = gameObject.GetComponentInChildren<Animator>();
        anim.SetTrigger("broken");

        //set colliders
        frontCollider.isTrigger = false;
        //add backside
        GameObject go = Instantiate(helibackPrefab, gameObject.transform.position + new Vector3(-1f, 0.15f, 0), Quaternion.identity);
        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(-50,50));
        rb.AddTorque(-50);

        //add gravity and random force
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 1;
        rb.AddForce(new Vector2(Random.Range(21f, 50f), Random.Range(21f, 50f)));
        rb.AddTorque(50);

        //create explosions
        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(Explosions(i * 0.25f));
        }
    }

    IEnumerator Explosions(float timer)
    {
        yield return new WaitForSeconds(timer);
        GameObject go = Instantiate(explosionPrefab, gameObject.transform.position + new Vector3(Random.Range(-.5f,.5f), Random.Range(-.5f, .5f), 0), Quaternion.identity);
        Destroy(go,2);
    }

}
