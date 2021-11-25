using UnityEngine;

public class ImageRandomizer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _randomSpriteRenderer;
    [SerializeField] private Sprite[] _sprites;

    [SerializeField] private float _timeBetweenChange;
    [SerializeField] private float _timeUntilStop;

    private int _currentSpriteIndex;
    private float _changeTimer;

    private void Start()
    {
        _changeTimer = _timeBetweenChange;
    }

    private void Update()
    {
        _timeUntilStop -= Time.deltaTime;
        _changeTimer -= Time.deltaTime;

        if (_timeUntilStop >= 0f)
        {
            _randomSpriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
            DestroyImmediate(this);
            return;
        }

        if (_changeTimer <= 0f)
        {
            _currentSpriteIndex++;
            if (_currentSpriteIndex >= _sprites.Length)
            {
                _currentSpriteIndex = 0;
            }

            _randomSpriteRenderer.sprite = _sprites[_currentSpriteIndex];
            _changeTimer = _timeBetweenChange;
        }
    }
}
