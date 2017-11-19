using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.HexCore
{
    public class HexMapEditor : MonoBehaviour
    {
        public Color[] colors;

        public HexGrid hexGrid;

        private Color activeColor;

        private int activeElevation;

        private void Awake()
        {
            SelectColor(0);
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                HandleInput();
            }
        }

        private void HandleInput()
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                EditCell(hexGrid.GetCell(hit.point));
            }
        }

        private void EditCell(HexCell cell)
        {
            cell.cellColor = activeColor;
            cell.Elevation = activeElevation;
            hexGrid.Refresh();
        }

        public void SelectColor(int colorIndex)
        {
            activeColor = colors[colorIndex];
        }

        public void SetElevation(float elevation)
        {
            activeElevation = (int)elevation;
        }
    }
}