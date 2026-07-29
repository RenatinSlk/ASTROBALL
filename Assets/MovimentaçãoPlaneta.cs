using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentaçãoPlano : MonoBehaviour
{
    [SerializeField] float maxTiltDegrees = 35f; // quanto o plano pode inclinar no máximo
    [SerializeField] float smoothSpeed = 10f; // quão rápido a inclinação acompanha o mouse
    [SerializeField] bool invertX; // se true, inverte a inclinação esquerda/direita
    [SerializeField] bool invertY; // se true, inverte a inclinação cima/baixo

    void Update() // roda todo frame
    {
        Mouse mouse = Mouse.current; // pega o mouse atual
        if (mouse == null) return; // se não tem mouse, para aqui

        Vector2 mousePos = mouse.position.ReadValue(); // posição do mouse na tela (pixels)

        float nx = (mousePos.x / Screen.width) * 2f - 1f; // X virando número de -1 a 1
        float ny = (mousePos.y / Screen.height) * 2f - 1f; // Y virando número de -1 a 1

        nx = Mathf.Clamp(nx, -1f, 1f); // trava X entre -1 e 1
        ny = Mathf.Clamp(ny, -1f, 1f); // trava Y entre -1 e 1

        if (invertX) nx = -nx; // inverte X se marcado no Inspector
        if (invertY) ny = -ny; // inverte Y se marcado no Inspector

        float pitch = ny * maxTiltDegrees; // inclinação frente/trás
        float roll = -nx * maxTiltDegrees; // inclinação esquerda/direita

        Quaternion target = Quaternion.Euler(pitch, 0f, roll); // rotação final desejada

        transform.rotation = Quaternion.Slerp( // aplica a rotação suavemente
            transform.rotation, // rotação atual
            target, // rotação alvo
            1f - Mathf.Exp(-smoothSpeed * Time.deltaTime) // quanto misturar neste frame
        );
    }
}
