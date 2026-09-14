using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerWeaponData", menuName = "Game/Player Weapon Data")] 
public class PlayerWeaponData : ScriptableObject
{
    [field: SerializeField] public string weaponName {  get; set; }
    [field: SerializeField] public Sprite worldSprite {  get; set; }
    [field: SerializeField] public int damage {  get; set; }
    [field: SerializeField] public float attackSpeed {  get; set; }
    [field: SerializeField] public float knockBackForce {  get; set; }
    [field: SerializeField] public float knockBackDuration {  get; set; }
    [field: SerializeField] public float knockBackLift {  get; set; }
    
}
