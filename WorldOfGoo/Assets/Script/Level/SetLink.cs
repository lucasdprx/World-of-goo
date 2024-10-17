using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLink : MonoBehaviour
{
    private Camera _camera;

    [SerializeField] private float _radiusSphere;
    [SerializeField] private float _radiusSphereVerif;

    [SerializeField] private int _numberLink;
    [SerializeField] private float _frequencyLink;

    [SerializeField] private GameObject _linkVisual;
    [SerializeField] private GameObject _linkVisualPreview;
    private List<GameObject> _listLinkPreview = new();
    private int _numberSkull;

    [SerializeField] private Sprite _spriteOs;
    private void Start()
    {
        for (int i = 0; i < _numberLink; i++)
        {
            GameObject linkPreview = Instantiate(_linkVisualPreview);
            _listLinkPreview.Add(linkPreview);
        }
        EnableLinkPreview(false);
        _camera = Camera.main;
    }

    public void AddLink(GameObject objTake)
    { 
        Vector2 position = _camera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] node = Physics2D.OverlapCircleAll(position, _radiusSphere, 1 << LayerMask.NameToLayer("Node"));
        SortCollider(node, objTake.transform.position);

        if (node.Length < 2 || Physics2D.OverlapCircleAll(position, _radiusSphereVerif, LayerMask.GetMask("Node", "Default", "Object")).Length > 1)
        {
            ResetObject(objTake);
            return;
        }

        for (int i = 0; i < node.Length; i++)
        {
            if (objTake.GetComponent<List_Link>()._links.Count >= _numberLink)
                break;

            //Add link
            objTake.GetComponent<List_Link>()._links.Add(node[i].transform);
            node[i].GetComponent<List_Link>()._links.Add(objTake.transform);

            //Add joint
            SpringJoint2D joint = objTake.AddComponent<SpringJoint2D>();
            joint.connectedBody = node[i].GetComponent<Rigidbody2D>();
            joint.frequency = _frequencyLink;
            joint.autoConfigureDistance = false;

            //Add LinkVisual
            GameObject linkVisual = Instantiate(_linkVisual);
            linkVisual.GetComponent<LinkVisual>().node1 = objTake.transform;
            linkVisual.GetComponent<LinkVisual>().node2 = node[i].transform;
        }

        SpriteRenderer spriteRenderer = objTake.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = _spriteOs;
        spriteRenderer.sortingOrder -= 1; 
        objTake.transform.GetChild(0).localScale /= 1.5f;
        objTake.layer = 7;
        objTake.GetComponent<Collider2D>().isTrigger = false;
        _numberSkull++;
        objTake.GetComponent<MoveMonsters>()._indexSkull = _numberSkull;
        objTake.GetComponent<Rigidbody2D>().freezeRotation = true;
        EnableLinkPreview(false);
    }
    private void ResetObject(GameObject obj)
    {
        MoveMonsters moveMonsters = obj.GetComponent<MoveMonsters>();
        obj.transform.position = moveMonsters._currentNode.position;
        moveMonsters.enabled = true;
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
    public void AddLinkPreview(GameObject objTake)
    {
        Vector2 position = _camera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] node = Physics2D.OverlapCircleAll(position, _radiusSphere, 1 << LayerMask.NameToLayer("Node"));
        SortCollider(node, objTake.transform.position);
        if (Physics2D.OverlapCircleAll(position, _radiusSphereVerif, LayerMask.GetMask("Node", "Default", "Object")).Length > 1 || node.Length < 2)
        {
            EnableLinkPreview(false);
            return;
        }
        for (int i = _numberLink; i > node.Length; i--)
        {
            _listLinkPreview[i - 1].SetActive(false);
        }
        for (int i = 0; i < node.Length; i++)
        {
            if (i >= _numberLink)
                break;

            _listLinkPreview[i].GetComponent<LinkVisual>().node1 = node[i].transform;
            _listLinkPreview[i].GetComponent<LinkVisual>().node2 = objTake.transform;
            StartCoroutine(DisableLinkOneFrame(_listLinkPreview[i]));
        }
    }
    private IEnumerator DisableLinkOneFrame(GameObject link)
    {
        yield return new WaitForEndOfFrame();
        link.SetActive(true);
    }
    public void EnableLinkPreview(bool enable)
    {
        for (int i = 0; i < _listLinkPreview.Count; i++)
        {
            _listLinkPreview[i].SetActive(enable);
        }
    }
}