using System.Collections.Generic;
using UnityEngine;

public class MoveMonsters : MonoBehaviour
{
    [SerializeField] private int _speed;
    private Transform _transform;
    public Transform _currentNode;
    private void Start()
    {
        _transform = transform;
    }
    void FixedUpdate()
    {
        MoveToNextBase();
    }
    private void MoveToNextBase()
    {
        _transform.position = Vector2.MoveTowards(_transform.position, _currentNode.position, _speed * Time.fixedDeltaTime);
        if (_transform.position == _currentNode.position)
        {
            List<Transform> _links = _currentNode.GetComponent<List_Link>()._links;
            _currentNode = _links[Random.Range(0, _links.Count)];
        }
    }
}
