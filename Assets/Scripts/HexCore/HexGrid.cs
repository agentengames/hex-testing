using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.HexCore
{
    public class HexGrid : MonoBehaviour
    {
        public int width = 6;
        public int height = 6;

        public HexCell cellPrefab;

        public Text cellLabelPrefab;

        private HexMesh hexMesh;

        public HexCell[] cells;

        private Canvas gridCanvas;

        public Color defaultColor = Color.white;

        public Texture2D noiseSource;

        private void Awake()
        {
            HexMetrics.noiseSource = noiseSource;

            gridCanvas = GetComponentInChildren<Canvas>();
            hexMesh = GetComponentInChildren<HexMesh>();

            cells = new HexCell[height * width];

            for (int z = 0, i = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    SetupCell(x, z, i++);
                }
            }
        }

        private void OnEnable()
        {
            HexMetrics.noiseSource = noiseSource;
        }

        private void Start()
        {
            hexMesh.Triangulate(cells);
        }

        public void Refresh()
        {
            hexMesh.Triangulate(cells);
        }

        public HexCell GetCell(Vector3 position)
        {
            position = transform.InverseTransformPoint(position);
            HexCoordinates coordinates = HexCoordinates.FromPosition(position);

            int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
            return cells[index];
        }

        private void SetupCell(int x, int z, int i)
        {
            Vector3 position = CalculateCellPosition(x, z);

            HexCell cell = CreateNewCell(x, z, i, position);

            SetCellNeighbor(x, z, i, cell);

            SetCellLabel(position, cell);

            cell.Elevation = 0;
        }

        private HexCell CreateNewCell(int x, int z, int i, Vector3 position)
        {
            HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
            cell.transform.SetParent(transform, false);
            cell.transform.localPosition = position;
            cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
            cell.cellColor = defaultColor;

            return cell;
        }

        private static Vector3 CalculateCellPosition(int x, int z)
        {
            Vector3 position;
            position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
            position.y = 0.0f;
            position.z = z * (HexMetrics.outerRadius * 1.5f);
            return position;
        }

        private void SetCellLabel(Vector3 position, HexCell cell)
        {
            Text label = Instantiate<Text>(cellLabelPrefab);
            label.rectTransform.SetParent(gridCanvas.transform, false);
            label.rectTransform.anchoredPosition =
                new Vector2(position.x, position.z);
            label.text = cell.coordinates.ToStringOnSeparateLines();

            cell.uiRect = label.rectTransform;
        }

        private void SetCellNeighbor(int x, int z, int i, HexCell cell)
        {
            if (x > 0)
            {
                cell.SetNeighbor(HexDirection.W, cells[i - 1]);
            }
            if (z > 0)
            {
                if ((z & 1) == 0)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - width]);
                    if (x > 0)
                    {
                        cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
                    }
                }
                else
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                    if (x < width - 1)
                    {
                        cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
                    }
                }
            }
        }
    }
}