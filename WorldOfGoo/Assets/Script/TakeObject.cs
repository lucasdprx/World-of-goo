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
            FollowMouse();
        }
    }
    public void FollowMouse()
    {
        Vector3 position = _camera.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;
        _objectTake.transform.position = position;
    }

    public void LeftClick(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider == null || hit.collider.gameObject.layer != 6)
                return;

            if (hit.collider.GetComponent<MoveMonsters>() == null)
            {
                return;
            } 
            
            if (hit.collider.GetComponent<MoveMonsters>().enabled)
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

    private void ResetObject()
    {
        MoveMonsters moveMonsters = _objectTake.GetComponent<MoveMonsters>();
        _objectTake.transform.position = moveMonsters._currentNode.position;
        moveMonsters.enabled = true;
    }
    private void AddLink()
    {
        Vector3 position = _camera.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;
        node = Physics2D.OverlapCircleAll(position, _radiusSphere);
        SortCollider(node, _objectTake.transform.position);

        List<Collider2D> closestNode = new();
        for (int i = 0; i < node.Length; i++)
        {
            Collider2D[] verif = Physics2D.OverlapCircleAll(position, 0.5f);
            if (node[i].TryGetComponent(out Link link) && link._links.Count > 0 && closestNode.Count < 2 && verif.Length < 2)
            {
                closestNode.Add(node[i]);
            }
        }

        if (closestNode.Count < 2)
        {
            ResetObject();
            return;
        }

        for (int i = 0;i < closestNode.Count;i++)
        {
            //Add link
            _objectTake.GetComponent<Link>()._links.Add(closestNode[i].transform);
            closestNode[i].GetComponent<Link>()._links.Add(_objectTake.transform);

            CreateLink(closestNode[i]);
        }
    }
    private void CreateLink(Collider2D closestNode)
    {
        //Set position and scale
        Vector3 newPos = (_objectTake.transform.position - closestNode.transform.position) / 2 + closestNode.transform.position;
        GameObject link = Instantiate(prefabLink);
        link.transform.position = newPos;
        float scaleX = Vector2.Distance(closestNode.transform.position, _objectTake.transform.position) - link.transform.localScale.x;
        link.transform.localScale += new Vector3(scaleX,0, 0);

        //Rotate
        Vector3 dir = _objectTake.transform.position - link.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        link.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, link.transform.rotation.y, 1));
    }

    private void SortCollider(Collider2D[] node, Vector2 posObj)
    {
        for (int i = 0; i < node.Length - 1; i++)
        {
            for (int j = 0; j < node.Length - 1; j++)
            {
                if (Vector2.Distance(posObj, node[j].transform.position) >
                    Vector2.Distance(posObj, node[j + 1].transform.position))
                {
                    (node[j + 1], node[j]) = (node[j], node[j + 1]);
                }
            }
        }
    }
}
