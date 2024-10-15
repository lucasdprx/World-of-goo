using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveMonsters : MonoBehaviour
{
    public int _speed;
    private Transform _transform;
    public Transform _currentNode;
    [HideInInspector] public bool _endGame;
    public int _indexSkull;
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
        if (_transform.position == _currentNode.position && _currentNode.TryGetComponent(out List_Link links))
        {
            if (_currentNode.TryGetComponent(out MoveMonsters monsters) && monsters._indexSkull == EndLevel.Instance._lastSkull)
            {
                EndLevel.Instance.SetTextScore();
                gameObject.SetActive(false);
                return;
            }
            List<Transform> _Listlinks = links._links;

            if (_endGame)
            {
                List<int> listIndexSkull = new();
                int index = -999;
                for (int i = 0; i < _Listlinks.Count; i++)
                {
                    MoveMonsters moveMonsters = _Listlinks[i].GetComponent<MoveMonsters>();
                    if (moveMonsters == null)
                        continue;
                    listIndexSkull.Add(moveMonsters._indexSkull);
                    if (listIndexSkull.Max() == moveMonsters._indexSkull)
                        index = i;
                }

                if (listIndexSkull.Count() > 0)
                {
                    if (_currentNode.TryGetComponent(out MoveMonsters moveMonsters) && listIndexSkull.Max() < moveMonsters._indexSkull)
                    {
                       moveMonsters._indexSkull = -999;
                    }
                    _currentNode = _Listlinks[index];
                }
                else
                    _currentNode = _Listlinks[Random.Range(0, _Listlinks.Count)];
            }
            else
                _currentNode = _Listlinks[Random.Range(0, _Listlinks.Count)];
            
            _transform.parent = _currentNode.transform;
        }
    }
}
