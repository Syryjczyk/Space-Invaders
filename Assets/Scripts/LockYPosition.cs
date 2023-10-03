using UnityEngine;

public class LockYPosition : MonoBehaviour
{
    [SerializeField] RectTransform buttonTransform;

    private RectTransform _parentTransform;
    private Vector2 _stablePosition;

    private void OnEnable()
    {
        _parentTransform = buttonTransform.gameObject.GetComponentInParent<RectTransform>();
        _stablePosition = new Vector2(0.0f , _parentTransform.position.y);
    }

    private void Update()
    {
        buttonTransform.position = new Vector2(buttonTransform.position.x, _stablePosition.y);
    }
}
