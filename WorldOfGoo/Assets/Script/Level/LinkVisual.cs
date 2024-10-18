using UnityEngine;

public class LinkVisual : MonoBehaviour
{
    private Transform _transform;
    private SpriteRenderer _spriteRenderer;

    public Transform node1;
    public Transform node2;

    [HideInInspector] public SpringJoint2D _joint;
    private void Start()
    {
        _transform = transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        SetTransformLink();
    }
    private void SetTransformLink()
    {
        //Set position
        Vector3 newPos = (node1.position - node2.position) / 2 + node2.position;
        _transform.position = newPos;

        //Set scale
        float scaleX = Vector2.Distance(node1.position, node2.position) - _transform.localScale.x;
        _transform.localScale += new Vector3(scaleX, 0, 0);

        //Rotate
        Vector3 dir = node1.position - _transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;   //Look at on node 1
        _transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, _transform.rotation.y, 1));

        ////Set color
        //float color = 1 - (_transform.localScale.x / 2.5f - 1);
        //if (_transform.localScale.x > 2.5f)
        //    _spriteRenderer.color = new Color(_spriteRenderer.color.r, color, color);
    }
}
