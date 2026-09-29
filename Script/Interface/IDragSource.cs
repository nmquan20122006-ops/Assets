using UnityEngine;
public interface IDragSource
{
    public bool CanDrag();
    public ItemSO GetDataDrag();
    public void OnDragCompleted(bool isSuccess);
}
