using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TextHighlight
    : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI textoBoton;
    public Color colorNormal = Color.white; 
    public Color colorIluminado = Color.yellow; 

    private void Start()
    {
        // Asegúrate de que tienes la referencia al componente Text
        if (textoBoton == null)
        {
            textoBoton = GetComponentInChildren<TextMeshProUGUI>(); // Obtener el Text si no está asignado
        }

        // Establecer el color inicial
        textoBoton.color = colorNormal;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Cambiar el color del texto cuando el ratón pasa por encima
        textoBoton.color = colorIluminado;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Volver al color normal cuando el ratón sale
        textoBoton.color = colorNormal;
    }
}
