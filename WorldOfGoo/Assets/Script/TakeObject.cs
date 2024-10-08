using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TakeObject : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private GameObject _objectTake;
    private bool _isTake = false;
    [SerializeField] private float _radiusSphere;
    private Collider2D[] node;
    [SerializeField] private GameObject prefabLink;
    void Update()
    {
        if (_isTake)
        {
            Vector3 position = _camera.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            _objectTake.transform.position = position;
        }
    }

    public void LeftClick(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.layer == 6 &&
                hit.collider.GetComponent<MoveMonsters>() != null && hit.collider.GetComponent<MoveMonsters>().enabled)
            {
                hit.collider.GetComponent<MoveMonsters>().enabled = false;
                _objectTake = hit.collider.gameObject;
                _isTake = true;
            }
        }
        else if (ctx.canceled && _objectTake != null)
        {
            AddLink();
            if (!_objectTake.GetComponent<MoveMonsters>().enabled)
            {
                _objectTake.transform.localScale = Vector3.one;
                _objectTake.GetComponent<SpriteRenderer>().color = Color.white;
                _objectTake.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
            _objectTake = null;
            _isTake = false;
        }
    }
    private void AddLink()
    {
        Vector3 position = _camera.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;
        node = Physics2D.OverlapCircleAll(position, _radiusSphere);
        List<Collider2D> closestNode = new();
        for (int i = 0; i < node.Length; i++)
        {
            if (node[i].TryGetComponent(out Link link) && link._links.Count > 0 && closestNode.Count < 2)
            {
                closestNode.Add(node[i]);
            }
        }
        if (closestNode.Count < 2)
        {
            MoveMonsters moveMonsters = _objectTake.GetComponent<MoveMonsters>();
            _objectTake.transform.position = moveMonsters._currentNode.position;
            moveMonsters.enabled = true;
            return;
        }

        for (int i = 0;i < closestNode.Count;i++)
        {
            //Add link
            _objectTake.GetComponent<Link>()._links.Add(closestNode[i].transform);
            closestNode[i].GetComponent<Link>()._links.Add(_objectTake.transform);

            //Set position and scale
            Vector3 newPos = (_objectTake.transform.position - closestNode[i].transform.position) / 2 + closestNode[i].transform.position;
            GameObject link = Instantiate(prefabLink);
            link.transform.position = newPos;
            link.transform.localScale = new Vector3(Vector2.Distance(closestNode[i].transform.position, _objectTake.transform.position),
                                                    link.transform.localScale.y, 1);

            //Rotate
            Vector3 dir = _objectTake.transform.position - link.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            link.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, link.transform.rotation.y, 1));
        }
    }
}
