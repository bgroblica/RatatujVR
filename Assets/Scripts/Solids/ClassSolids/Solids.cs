using UnityEngine;

public abstract class Solids : MonoBehaviour
{
    public enum IngredientType
    {
        Butter,
        Sugar,
        Egg
    }

    public IngredientType ingredientType;

    public float amount = 1f;

    private bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        if (used) return;

        Bowl bowl = other.GetComponentInParent<Bowl>();

        if (bowl == null) return;

        switch (ingredientType)
        {
            case IngredientType.Butter:
                bowl.AddButter(amount);
                break;

            case IngredientType.Sugar:
                bowl.AddSugar(amount);
                break;
            case IngredientType.Egg:
                bowl.AddEgg(amount);
                break;
        }

        used = true;

        Destroy(gameObject);
    }
}