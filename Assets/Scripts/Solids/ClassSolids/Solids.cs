using UnityEngine;

public abstract class Solids : MonoBehaviour
{
    public enum IngredientType
    {
        Butter,
        Sugar
    }

    public IngredientType ingredientType;

    public float amount = 1f;

    private bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        if (used) return;

        Bowl bowl = other.GetComponentInParent<Bowl>();

        if (bowl == null) return;

        Debug.Log("Entered Bowl");
        switch (ingredientType)
        {
            case IngredientType.Butter:
                bowl.AddButter(amount);
                break;

            case IngredientType.Sugar:
                bowl.AddSugar(amount);
                break;
        }

        used = true;

        Destroy(gameObject);
    }
}