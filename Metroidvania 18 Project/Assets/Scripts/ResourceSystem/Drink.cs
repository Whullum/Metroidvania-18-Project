using UnityEngine;

public class Drink : ScriptableObject
{
    public DrinkType DrinkType;
    public Sprite DrinkImage;
    public int Cost;

    public virtual void UseDrink() { }
}
