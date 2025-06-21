using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTesting : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootForce = 50000f;

    [SerializeField] private RectTransform imageTransform;

    void Update() {
        if (Input.GetMouseButtonDown(0)) { // Left click
            ShootTowardMouse();
        }
    }

    void ShootTowardMouse() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 direction = ray.direction.normalized;

        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null) {
            rb.AddForce(direction * shootForce);
        }


        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imageTransform.parent as RectTransform,
            Input.mousePosition,
            null,
            out mousePos
        );

        imageTransform.anchoredPosition = mousePos;
    }

}
