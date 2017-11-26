using UnityEngine;

namespace Assets.Scripts.HexCore
{
    public class HexCell : MonoBehaviour
    {
        public HexCoordinates coordinates;

        public Color cellColor;

        public RectTransform uiRect;

        public int Elevation
        {
            get { return elevation; }
            set
            {
                elevation = value;
                Vector3 position = transform.localPosition;
                position.y += (HexMetrics.SampleNoise(position).y * 2f - 1f) * HexMetrics.elevationPerturbStrength;
                transform.localPosition = position;

                Vector3 uiPosition = uiRect.localPosition;
                uiPosition.z = -position.y;
                uiRect.localPosition = uiPosition;
            }
        }

        public Vector3 Position
        {
            get { return transform.localPosition; }
        }

        private int elevation;

        [SerializeField]
        private HexCell[] neighborCells;

        public HexCell GetNeighbor(HexDirection direction)
        {
            return neighborCells[(int)direction];
        }

        public void SetNeighbor(HexDirection direction, HexCell neighborCell)
        {
            neighborCells[(int)direction] = neighborCell;
            neighborCell.neighborCells[(int)direction.Opposite()] = this;
        }

        public HexEdgeType GetEdgeType(HexDirection direction)
        {
            return HexMetrics.GetEdgeType(elevation, neighborCells[(int)direction].elevation);
        }

        public HexEdgeType GetEdgeType(HexCell otherCell)
        {
            return HexMetrics.GetEdgeType(elevation, otherCell.elevation);
        }
    }
}