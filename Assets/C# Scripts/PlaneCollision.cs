using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlaneCollision : MonoBehaviour
{
    Color color;

    // Start is called before the first frame update
    void OnCollisionEnter(Collision collision)
    {
        //Check to see if the Collider's name is "Chest"
        if (collision.collider.tag == "Cube")
        {
            //Output the message
            Debug.Log("Box hit Plane");
            // color
            color = collision.gameObject.GetComponent<MeshRenderer>().material.color;

            Destroy(collision.gameObject);
            Vector3 poc = collision.transform.position;
            Explode(poc, color);
        }
    }

// -------------------------------------------------------------------

    public float cubeSize = 0.2f;
    public int cubesInRow = 5;

    float cubesPivotDistance;
    Vector3 cubesPivot;

    public float explosionForce = 50f;
    public float explosionRadius = 4f;
    public float explosionUpward = 0.4f;



    // Use this for initialization
    void Start() 
    {
        //calculate pivot distance
        cubesPivotDistance = cubeSize * cubesInRow / 2;
        //use this value to create pivot vector)
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
    }

    void Explode(Vector3 pos, Color objectColor)
    {
        Debug.Log("2");
        //loop 3 times to create 5x5x5 pieces in x,y,z coordinates
        for (int x = 0; x < cubesInRow; x++) {
            for (int y = 0; y < cubesInRow; y++) {
                for (int z = 0; z < cubesInRow; z++) {
                    createPiece(pos.x + (x * cubeSize), pos.y + (y * cubeSize), pos.z + (z * cubeSize), objectColor);
                }
            }
        }

        //get colliders in that position and radius
        Collider[] colliders = Physics.OverlapSphere(pos, explosionRadius);
        //add explosion force to all colliders in that overlap sphere
        Debug.Log("4");
        foreach (Collider hit in colliders) 
        {
            //get rigidbody from collider object
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null) {
                //add explosion force to this body with given parameters
                rb.AddExplosionForce(explosionForce, pos, explosionRadius, explosionUpward);
            }
        }
        Debug.Log("5");
    }

    void createPiece(float x, float y, float z, Color objectColor) {
        //create piece
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // colour
        piece.GetComponent<Renderer>().material.color = objectColor;

        //set piece position and scale
        piece.transform.position = new Vector3(x, y, z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        //add rigidbody and set mass
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;
        Destroy(piece, 2);
    }

    
}
