using UnityEngine;
using UnityEngine.UI;

namespace CrossProject.Core
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class DynamicGrid : MonoBehaviour
    {
        [SerializeField] private float _aspectRatio = 1f;

        private GridLayoutGroup _grid;
        private RectTransform _rect;

        private void OnEnable()
        {
            Canvas.willRenderCanvases += UpdateCellSize;
        }

        private void OnDisable()
        {
            Canvas.willRenderCanvases -= UpdateCellSize;
        }

        private void UpdateCellSize()
        {
            _grid = GetComponent<GridLayoutGroup>();
            _rect = GetComponent<RectTransform>();

            if (_grid.constraint == GridLayoutGroup.Constraint.Flexible)
            {
                return;
            }

            var size = GetSize(_rect);

            var totalWidth = size.x;
            var totalHeight = size.y;

            switch (_grid.constraint)
            {
                case GridLayoutGroup.Constraint.FixedColumnCount:
                {
                    var columns = _grid.constraintCount;

                    var availableWidth = totalWidth - _grid.padding.left - _grid.padding.right - (columns - 1) * _grid.spacing.x;

                    var cellWidth = availableWidth / columns;
                    var cellHeight = cellWidth / _aspectRatio;

                    _grid.cellSize = new Vector2(cellWidth, cellHeight);

                    break;
                }
                case GridLayoutGroup.Constraint.FixedRowCount:
                {
                    var rows = _grid.constraintCount;

                    var availableHeight = totalHeight - _grid.padding.top - _grid.padding.bottom - (rows - 1) * _grid.spacing.y;

                    var cellHeight = availableHeight / rows;
                    var cellWidth = cellHeight * _aspectRatio;

                    _grid.cellSize = new Vector2(cellWidth, cellHeight);

                    break;
                }
            }
        }

        private Vector2 GetSize(RectTransform rt)
        {
            if (rt.parent == null)
                return rt.rect.size; // Canvas или верхний уровень

            var parent = rt.parent as RectTransform;
            var parentSize = parent.rect.size;

            var width = parentSize.x * (rt.anchorMax.x - rt.anchorMin.x) - rt.offsetMin.x + rt.offsetMax.x;
            var height = parentSize.y * (rt.anchorMax.y - rt.anchorMin.y) - rt.offsetMin.y + rt.offsetMax.y;

            return new Vector2(width, height);
        }
    }
}
