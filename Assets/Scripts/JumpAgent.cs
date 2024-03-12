using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.UI;

public class JumpAgent : Agent
{

    public Animator animator;
    public GameObject scenario;



    bool isGrounded = false;
    bool isDead = false;
    int score = 0;

    private Rigidbody2D rbody;
   
    public Text textScore;

    public GameObject GenObs;
    private GenerateObs GenObsScript;

    public List<AudioClip> audio; //list of audio for coin, death & jump

    public ParticleSystem particelleCoin;

     
    public override void Initialize()
    {
        rbody = this.GetComponent<Rigidbody2D>();

        textScore.text = "0";

        GenObsScript = GenObs.GetComponent<GenerateObs>();
        GenObsScript.T = 0.0f;
        GenObsScript.T1 = 0.0f;

        particelleCoin.Stop();

    }

  
    public override void OnActionReceived(ActionBuffers actions)
    {
        if (actions.DiscreteActions[0] == 1)
        {
            jumpPlayer();
        }

        if (actions.DiscreteActions[0] == 2)
        {
            goBack();
        }

        if (actions.DiscreteActions[0] == 3)
        {
            goForward();
        }
    }

    public override void OnEpisodeBegin()
    {
        Reset();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteAction = actionsOut.DiscreteActions;
        discreteAction[0] = 0;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) )
        {
            
            discreteAction[0] = 1;

        }

        if( Input.GetKeyDown(KeyCode.LeftArrow))
        {
            discreteAction[0] = 2;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            discreteAction[0] = 3;
        }

    }

    private void jumpPlayer()
    {
        if (isGrounded==true && isDead==false)
        {

            AudioSource.PlayClipAtPoint(audio[2], transform.localPosition);
            isGrounded = false;
            //animator.SetBool("jump", true);
            rbody.AddForce(new Vector2(7.0f, 150.0f));
           

            AddReward(-0.35f);
        }   
    }

    private void goBack()
    {
        if (isGrounded == true)
        {
            rbody.AddForce(new Vector2(-30.0f, 0f));

            AddReward(+0.0055f);
        }

    }

    private void goForward()
    {
        if (isGrounded == true)
        {
            rbody.AddForce(new Vector2(+18.0f, 0f));

            AddReward(-0.0040f);

        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // checking if the player is on the ground.
        if (col.gameObject.tag == "ground")
        {
            //animator.SetBool("jump", false);
            isGrounded = true;
            
        }

        // checking if the player collide with the walls
        if (col.gameObject.tag == "wall")
        {
            AddReward(-0.30f);

        }

        // checking if the player has collided with any obstacles
        if (col.gameObject.tag == "obs" )
        {

            AddReward(-1f);
            isDead = true;

            AudioSource.PlayClipAtPoint(audio[1], transform.localPosition);

            Debug.Log("REWARD: "+ GetCumulativeReward());
           
            EndEpisode();
        }

    }

    // Collects the coin and adds 100 to the player score as well as plays the sound for coin collection.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "coin")
        {

            //AddReward(+0.40f);
            AddReward(+0.30f);
            score += 1;

            particelleCoin.Play();

            AudioSource.PlayClipAtPoint(audio[0], transform.localPosition);


            Destroy(other.gameObject);
        }
    }

    private void Reset()
    {
        
      score = 0;
      isGrounded = false;
      isDead = false;

      GenObsScript = GenObs.GetComponent<GenerateObs>();
      GenObsScript.T = 0.0f;
      GenObsScript.T1 = 0.0f;

      cleanObs();

     
      transform.localPosition = new Vector3(-3.43000007f, -2.998209f, -1.20992661f);


      particelleCoin.Stop();

    }
    void Update()
    {
        textScore.text = " " + score.ToString();
    }

    private void cleanObs()
    {
        //foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        foreach (Transform o in scenario.transform)
            {
            if (o.gameObject.tag == "obs0" || o.gameObject.tag == "obs1" || o.gameObject.tag == "obs2" || o.gameObject.tag == "obs3")
            {
                Destroy(o.gameObject);
            }

        }
    }
}
