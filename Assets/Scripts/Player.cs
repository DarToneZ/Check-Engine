using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    [SerializeField] private Rigidbody2D frontTireRB;
    [SerializeField] private Rigidbody2D backTireRB;

    [SerializeField] WheelJoint2D frontWheelJoint;
    [SerializeField] WheelJoint2D backWheelJoint;

    [SerializeField] float defaultFrequency;
    [SerializeField] float jumpingFrequency;

    float backJumpMultiplier;
    float frontJumpMultiplier;
    [SerializeField] float minJumpTime;
    [SerializeField] float timeToChargeFullJump;

    [SerializeField] private float speed;
    [SerializeField] float jumpForce;

    private float moveInput;

    [SerializeField] float maxSpeed;
    [SerializeField] AudioSource engineSound;
    [SerializeField] float maxSpeedSound;
    [SerializeField] float minPitchEngine;
    [SerializeField] float maxPitchEngine;

    [SerializeField] GameObject bloodEffect;

    public Vector2 VelocityAdd;
    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        //if(Input.GetKeyDown(KeyCode.R))
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //}

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            backJumpMultiplier += Time.deltaTime;
            if(backWheelJoint != null)
            {
            JointSuspension2D bwjs = backWheelJoint.suspension;
            bwjs.frequency = jumpingFrequency;
            backWheelJoint.suspension = bwjs;
            }
        }
        else
        {
            if(backWheelJoint != null)
            {
            JointSuspension2D bwjs = backWheelJoint.suspension;
            bwjs.frequency = defaultFrequency;
            backWheelJoint.suspension = bwjs;
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            frontJumpMultiplier += Time.deltaTime;
            JointSuspension2D fwjs = frontWheelJoint.suspension;
            fwjs.frequency = jumpingFrequency;
            frontWheelJoint.suspension = fwjs;
        }
        else
        {
            JointSuspension2D fwjs = frontWheelJoint.suspension;
            fwjs.frequency = defaultFrequency;
            frontWheelJoint.suspension = fwjs;
        }

        if(Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if(backTireRB != null)
            {
            rb.AddForceAtPosition(Vector2.up * jumpForce * Mathf.Clamp(backJumpMultiplier - minJumpTime, 0f, timeToChargeFullJump) / timeToChargeFullJump, backTireRB.position, ForceMode2D.Impulse);
            backJumpMultiplier = 0f;
            }
            //rb.AddForceAtPosition(new Vector2(0f, jumpForce * Mathf.Clamp(backJumpMultiplier - minJumpForce, 0f, timeToChargeFullJump) / timeToChargeFullJump), backTireRB.position, ForceMode2D.Impulse);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            //rb.AddForceAtPosition(new Vector2(0f, jumpForce * Mathf.Clamp(frontJumpMultiplier - minJumpForce, 0f, timeToChargeFullJump) / timeToChargeFullJump), frontTireRB.position, ForceMode2D.Impulse);
            rb.AddForceAtPosition(Vector2.up * jumpForce * Mathf.Clamp(frontJumpMultiplier - minJumpTime, 0f, timeToChargeFullJump) / timeToChargeFullJump, frontTireRB.position, ForceMode2D.Impulse);
            if(frontJumpMultiplier > minJumpTime && bloodEffect != null)
            {
                Instantiate(bloodEffect, transform.position, transform.rotation);
            }
            frontJumpMultiplier = 0f;
        }
        //if (Input.GetKey(KeyCode.Space))
        //{
            //rb.MovePosition(rb.position + new Vector2(3f, 0f) * Time.deltaTime);
            //rb.AddRelativeForce(new Vector2(2f, 0f));
            //rb.GetRelativePointVelocity(new Vector2(3f, 0f));
            //rb.GetPointVelocity(new Vector2(3f, 0f));
            //rb.position += (new Vector2(3f, 0f) * Time.deltaTime);
        //}
    }

    private void FixedUpdate()
    {
        frontTireRB.AddTorque(-moveInput * speed * Time.fixedDeltaTime / Mathf.Clamp(frontTireRB.angularVelocity - maxSpeed, 1, 999));  //little bit smoother speed limit, idk if it really work well
        if(backTireRB != null)
        {
        backTireRB.AddTorque(-moveInput * speed * Time.fixedDeltaTime / Mathf.Clamp(backTireRB.angularVelocity - maxSpeed, 1, 999));
        }

        frontTireRB.angularVelocity = Mathf.Clamp(frontTireRB.angularVelocity, - maxSpeed, maxSpeed);
        if(backTireRB != null)
        {
        backTireRB.angularVelocity = Mathf.Clamp(backTireRB.angularVelocity, -maxSpeed, maxSpeed);
        }

        float currentSpeed = Mathf.Abs((frontTireRB.angularVelocity + backTireRB.angularVelocity) / 2);
        //print(currentSpeed);

        bool isThrottling = Mathf.Abs(moveInput) > 0.1f;

        float targetPitch;

        if (isThrottling)
        {
            targetPitch = maxPitchEngine;
        }
        else
        {
            targetPitch = Mathf.Lerp(minPitchEngine, maxPitchEngine, currentSpeed / maxSpeedSound);
        }

        engineSound.pitch = Mathf.Lerp(engineSound.pitch, targetPitch, Time.fixedDeltaTime * 10f);
        //rb.linearVelocity = rb.linearVelocity + ;
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    //rb.MovePosition(rb.position + new Vector2(3f, 0f) * Time.deltaTime);
        //    //rb.AddRelativeForce(new Vector2(2f, 0f));
        //    //rb.GetRelativePointVelocity(new Vector2(3f, 0f));
        //    //rb.GetPointVelocity(new Vector2(3f, 0f));
        //    //rb.position += (VelocityAdd * Time.fixedDeltaTime);
        //    //frontTireRB.position += (VelocityAdd * Time.fixedDeltaTime);
        //    //backTireRB.position += (VelocityAdd * Time.fixedDeltaTime);
        //}
    }

    public void ResetPosition()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = 0;
        backTireRB.linearVelocity = Vector3.zero;
        backTireRB.angularVelocity = 0;
        //backTireRB.position = backWheelJoint.anchor;
        backTireRB.gameObject.transform.position = transform.position;
        frontTireRB.linearVelocity = Vector3.zero;
        frontTireRB.angularVelocity = 0;
        frontTireRB.gameObject.transform.position = transform.position;
        //frontTireRB.position = frontWheelJoint.anchor;
    }
}
