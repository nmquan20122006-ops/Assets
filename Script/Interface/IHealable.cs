using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealable
{
    int currentHealth { get; }
    int maxHealth { get; }
    void Heal(int amount);
}
