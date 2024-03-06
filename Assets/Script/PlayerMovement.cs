// lets make him move
// using __ imports namespace
// Namespaces are collection of classes, data types
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

// MonoBehavior is the base class from which every Unity Script Derives
public class PlayerMovement : NetworkBehaviour
{
    public float speed = 25.0f;
    public float rotationSpeed = 90;
    public float force = 700f;

    private Animator animator;

    Rigidbody rb;
    Transform t;

    bool isInMotion = false;

    public Transform holdPoint; // Assign a point where picked objects will be parented

    [SerializeField]
    public GameObject heldObject;
    public Transform cameratransform;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        if(!IsOwner){
            transform.Find("Camera").GetComponent<Camera>().enabled = false;
        }
    }

    

    void Update()
    {
        if(!IsOwner) return;
        
        if (Input.GetKey(KeyCode.W))
            rb.velocity += this.transform.forward * speed * Time.deltaTime;
   
        else if (Input.GetKey(KeyCode.S))
            rb.velocity -= this.transform.forward * speed * Time.deltaTime;

        // Quaternion returns a rotation that rotates x degrees around the x axis and so on
        if (Input.GetKey(KeyCode.D))
            t.rotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);

        else if (Input.GetKey(KeyCode.A))
            t.rotation *= Quaternion.Euler(0, - rotationSpeed * Time.deltaTime, 0);

        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            isInMotion = true;

        animator.SetBool("IsMoving",false);
        if (Input.GetKeyDown(KeyCode.Space))
            rb.AddForce(t.up * force);
        // animator.SetBool("IsMoving",false);
        if (isInMotion == true)
            animator.SetBool("IsMoving", true);
        else
            animator.SetBool("IsMoving",false);


        if (Input.GetButtonDown("Fire1"))
        {
            if (heldObject == null)
                TryPickupObject();
            else
                DropObject();
        }
    }


    /*void TryPickupObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameratransform.position, cameratransform.forward, out hit, 2000f)) // 2f is the pickup distance
        {
            //Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.CompareTag("PickupObject")) // Make sure your prefabs have this tag
            {
                heldObject = hit.collider.gameObject;
                heldObject.transform.SetParent(holdPoint);
                heldObject.transform.position = holdPoint.position;
                heldObject.GetComponent<Rigidbody>().isKinematic = true;
                heldObject.GetComponent<Collider>().enabled = false;
            }
        }
    }

    void DropObject()
    {
        heldObject.transform.SetParent(null);
        heldObject.GetComponent<Rigidbody>().isKinematic = false;
        heldObject.GetComponent<Collider>().enabled = true;
        heldObject = null;
    }*/  


    [ServerRpc]
    private void PickupObjectServerRpc(ulong clientId){
        if(NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(clientId, out NetworkObject networkObject)){
            networkObject.transform.SetParent(holdPoint);
            networkObject.GetComponent<Rigidbody>().isKinematic = true;
            networkObject.GetComponent<Collider>().enabled = false;
        }
    }


    [ServerRpc]
    void DropObjectServerRpc(ulong clientId)
    {
        if(NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(clientId, out NetworkObject networkObject)){
            networkObject.transform.SetParent(null);
            networkObject.GetComponent<Rigidbody>().isKinematic = false;
            networkObject.GetComponent<Collider>().enabled = true;
        }
    }

    void TryPickupObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameratransform.position, cameratransform.forward, out hit, 2000f)) // Adjusted distance for realism
        {
            if (hit.collider.CompareTag("PickupObject"))
            {
                ulong objectId = hit.collider.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
                heldObject = hit.collider.gameObject;
                PickupObjectServerRpc(objectId); // Call ServerRpc instead of direct reparenting
            }
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            ulong objectId = heldObject.GetComponent<NetworkObject>().NetworkObjectId;
            heldObject = null;
            DropObjectServerRpc(objectId); // Call ServerRpc for dropping the object
        }
    }
}