using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas parentCanvas;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        // Encontra o Canvas mais próximo
        parentCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        // Torna a imagem semi-transparente enquanto arrasta
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false; // Permite que outros elementos recebam eventos enquanto arrasta
    }

    public void OnDrag(PointerEventData eventData) {
        // Move a imagem com o cursor, levando em conta a escala do Canvas
        if (parentCanvas != null) {
            rectTransform.anchoredPosition += eventData.delta / parentCanvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        // Restaura a opacidade ao soltar
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true; // Volta a bloquear interações
    }
}

