using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;

public class Scale : MonoBehaviour
{
    public XRSocketInteractor socket;

    public TMP_Text milkText;
    public TMP_Text flourText;
    public TMP_Text eggText;
    public TMP_Text sugarText;
    public TMP_Text butterText;
    public TMP_Text flavourText;

    private void Start()
    {
        ClearDisplay();
    }
    private void OnEnable()
    {
        socket.selectEntered.AddListener(OnObjectPlaced);
        socket.selectExited.AddListener(OnObjectRemoved);
    }

    private void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnObjectPlaced);
        socket.selectExited.RemoveListener(OnObjectRemoved);
    }

    private void OnObjectPlaced(SelectEnterEventArgs args)
    {
        Transform obj =
            args.interactableObject.transform;

        Debug.Log("Placed on scale: " + obj.name);

        ReadObject(obj);
    }

    private void OnObjectRemoved(SelectExitEventArgs args)
    {
        Debug.Log("Removed from scale");

        ClearDisplay();
    }

    private void ReadObject(Transform obj)
    {
        Bowl bowl =
            obj.GetComponent<Bowl>();

        if (bowl != null)
        {
            if (bowl.IsFullyMixed())
            {
                ShowData(
                    bowl.batterMilk,
                    bowl.batterFlour,
                    bowl.batterEgg,
                    bowl.batterSugar,
                    bowl.batterButter,
                    bowl.batterFlavour.ToString()
                );
            }
            else
            {
                ShowData(
                    bowl.milkAmount,
                    bowl.flourAmount,
                    bowl.eggAmount,
                    bowl.sugarAmount,
                    bowl.butterAmount,
                    bowl.flavour.ToString()
                );
            }

            return;
        }

        Mold mold =
            obj.GetComponent<Mold>();

        if (mold != null)
        {
            ShowData(
                mold.milkAmount,
                mold.flourAmount,
                mold.eggAmount,
                mold.sugarAmount,
                mold.butterAmount,
                mold.flavour.ToString()
            );

            return;
        }

        Cake cake =
            obj.GetComponent<Cake>();

        if (cake != null)
        {
            ShowData(
                cake.milkAmount,
                cake.flourAmount,
                cake.eggAmount,
                cake.sugarAmount,
                cake.butterAmount,
                cake.flavour.ToString()
            );

            return;
        }

        ClearDisplay();
    }

    private void ShowData(
        float milk,
        float flour,
        float egg,
        float sugar,
        float butter,
        string flavour)
    {
        milkText.text = milk.ToString("F2");
        flourText.text = flour.ToString("F2");
        eggText.text = egg.ToString("F2");
        sugarText.text = sugar.ToString("F2");
        butterText.text = butter.ToString("F2");
        flavourText.text = flavour;
    }

    private void ClearDisplay()
    {
        milkText.text = "-";
        flourText.text = "-";
        eggText.text = "-";
        sugarText.text = "-";
        butterText.text = "-";
        flavourText.text = "-";
    }
}