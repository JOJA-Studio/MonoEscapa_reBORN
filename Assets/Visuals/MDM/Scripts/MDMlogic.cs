using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MDMlogic : MonoBehaviour
{
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;
    public KeyCode throwKey = KeyCode.Mouse0;
    public int totalThrows;
    public float throwCooldown;
    public float throwForce;
    public float throwthrowUpwardForce;
    bool ready;

    // Start is called before the first frame update
    void Start()
    {
        ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(throwKey) && ready && totalThrows > 0)
        {
            Throw();
        }
    }
    private void Throw()
    {
        ready = false;
        //Instainiciar objeto a lanzar
        GameObject projectil = Instantiate(objectToThrow, attackPoint.position, cam.rotation);
        //agarramos rigidbody para el projectil
        Rigidbody rigidocuerpo = projectil.GetComponent<Rigidbody>();
        //calculamos la direccion donde está apuntando el player
        Vector3 forceDirection = cam.transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }
        //añadimos fuerza para que salga disparado
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwthrowUpwardForce;
        rigidocuerpo.AddForce(forceToAdd,ForceMode.Impulse);
        totalThrows--;
        Invoke(nameof(ResetThrow),throwCooldown);
    }
    private void ResetThrow()
    {
        ready = true;
    }
}
