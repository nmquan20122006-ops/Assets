using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ComboConfig", menuName = "Game/ComboConfig")]
public class ComboConfig : ScriptableObject
{
    [Header("Timing")]
    [Tooltip("AttackInterval = clipLength / attackSpeed * margin")]
    public float margin = 1.3f;
    [Tooltip("Buffer window")]
    public float bufferWindow = 0.2f;
    [Header("Combo")]
    public int maxComboStep = 3;
    [Header("Is Queue Allow")]
    public bool isQueueAllow;
    [Header("Queue size")]
    public int queueSize;
}
