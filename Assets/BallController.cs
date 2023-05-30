using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class BallController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float force;
    [SerializeField] LineRenderer aimLine;
    [SerializeField] Transform aimWorld;
    bool shoot;
    bool shootingMode;
    float forceFactor;
    Vector3 forceDirection;
    Ray ray;
    Plane plane;
    public bool ShootingMode { get => shootingMode; }
    int shootCount;
    public int ShootCount { get => shootCount;}
    public UnityEvent<int> onBallShooted = new UnityEvent<int>();
    private void Update()
    {
        if(shootingMode)
        {
            if (Input.GetMouseButtonDown(button: 0))
            {
                aimLine.gameObject.SetActive(value: true);
                aimWorld.gameObject.SetActive(value: true);
                plane = new Plane(inNormal: Vector3.up, inPoint: this.transform.position);
            }
            else if (Input.GetMouseButton(button: 0))
            {
                //draw aim
                //aimLine.transform.position = ballScreenPos;

                //aimWorld.transform.position = this.transform.position;
                //var aimDirection = new Vector3(x: pointerDirection.x, y: 0, z: pointerDirection.y);
                //aimDirection = Camera.main.transform.localToWorldMatrix * aimDirection;
                //aimWorld.transform.forward = aimDirection;
                //aimWorld.transform.forward = aimDirection.normalized;

                //force direction
                ray = Camera.main.ScreenPointToRay(pos: Input.mousePosition);
                plane.Raycast(ray: ray, enter: out var distance);
                forceDirection = (this.transform.position - ray.GetPoint(distance: distance));
                forceDirection.Normalize();

                //force factor
                var mouseViewportPos = Camera.main.ScreenToViewportPoint(position: Input.mousePosition);
                var ballViewportPos = Camera.main.WorldToViewportPoint(position: this.transform.position);
                var pointerDirection = ballViewportPos - mouseViewportPos;
                pointerDirection.z = 0;
                pointerDirection.z *= Camera.main.aspect;
                pointerDirection.z = Mathf.Clamp(value: pointerDirection.z, min: -0.5f, max: 0.5f);
                forceFactor = pointerDirection.magnitude * 2;

                //aim visuals
                aimWorld.transform.position = this.transform.position;
                aimWorld.forward = forceDirection;
                aimWorld.localScale = new Vector3(x: 1, y: 1, z: 0.5f + forceFactor);

                var ballScreenPos = Camera.main.WorldToScreenPoint(position: this.transform.position);
                var mouseScreenPos = Input.mousePosition;
                ballScreenPos.z = 1f;
                mouseScreenPos.z = 1f;
                var positions = new Vector3[] {
                    Camera.main.ScreenToWorldPoint(position: ballScreenPos),
                    Camera.main.ScreenToWorldPoint(position: mouseScreenPos)};
                aimLine.SetPositions(positions: positions);
                aimLine.endColor = Color.Lerp(a: Color.blue, b: Color.red, t: forceFactor);
            }
            else if (Input.GetMouseButtonUp(button: 0))
            {
                shoot = true;
                shootingMode = false;
                //aimLine.gameObject.SetActive(value: false);
                aimWorld.gameObject.SetActive(value: false);
            }
        }
    }


    private void FixedUpdate()
    {
        if (shoot)
        {
            shoot = false;
            rb.AddForce(force: forceDirection * force * forceFactor, mode: ForceMode.Impulse);
            shootCount += 1;
            onBallShooted.Invoke(arg0: shootCount);
        }

        if (rb.velocity.sqrMagnitude < 0.01f && rb.velocity.sqrMagnitude > 0)
        {
            rb.velocity = Vector3.zero;
            //rb.useGravity = false;
        }
    }

    public bool IsMove()
    {
        return rb.velocity != Vector3.zero;
    }

    //public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Impulse)
    //{
        //rb.useGravity = true;
        //rb.AddForce(force: force, mode: forceMode);
    //}

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (this.IsMove())
            return;

        shootingMode = true;
    }
}
