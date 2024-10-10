using UnityEngine;

public class LinkVisual : MonoBehaviour
{
    private Transform _transform;

    public Transform node1;
    public Transform node2;
    private void Start()
    {
        _transform = transform;
    }
    private void Update()
    {
        SetTransformLink();
    }
    private void SetTransformLink()
    {
        //Set position and scale
        Vector3 newPos = (node1.position - node2.position) / 2 + node2.position;
        _transform.position = newPos;

        float scaleX = Vector2.Distance(node1.position, node2.position) - _transform.localScale.x;
        _transform.localScale += new Vector3(scaleX, 0, 0);

        //Rotate
        Vector3 dir = node1.position - _transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, _transform.rotation.y, 1));
    }
}
