using UnityEngine;

public class Swipe : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private Movement movement;

    private Vector3 _startPoint = Vector3.zero;
    private Vector3 _endPoint = Vector3.zero;
    private Vector3 _difference = Vector3.zero;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.timeScale > 0f)
        {
            // Uložení si vektoru startovní pozice myši
            _startPoint = Input.mousePosition;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0) && Time.timeScale > 0f)
        {
            // Uložení si vektoru koncové pozice myši
            _endPoint = Input.mousePosition;

            _difference = _startPoint - _endPoint;

            float yDirection = Vector3.Dot(Vector3.up, _difference);
            float xDirection = Vector3.Dot(Vector3.right, _difference);

            if (Mathf.Abs(xDirection) > Mathf.Abs(yDirection))
            {
                yDirection = 0f;
            }
            else if (Mathf.Abs(xDirection) < Mathf.Abs(yDirection))
            {
                xDirection = 0f;
            }

            // Porovnávám to v mínusu, protože by mi to vracelo obráceně hodnoty for ex.: xDirection == 20f; a místo right by to bylo left
            if (-yDirection >= 1f)
            {
                // aka up
                movement.CurrentInstruction = Movement.Instruction.Jump;
            }
            else if (-yDirection <= -1f)
            {
                // aka down
                movement.CurrentInstruction = Movement.Instruction.SlideDown;
            }
            else if (-xDirection >= 1f)
            {
                // aka right
                movement.CurrentInstruction = Movement.Instruction.SlideRight;
            }
            else if (-xDirection <= -1f)
            {
                // aka left
                movement.CurrentInstruction = Movement.Instruction.SlideLeft;
            }
        }
        else
        {
            movement.CurrentInstruction = Movement.Instruction.Run;
        }
    }
}
