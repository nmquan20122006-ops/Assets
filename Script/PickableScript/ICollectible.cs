using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectible 
{
    int Collect(ItemSO data, int amount);
}
