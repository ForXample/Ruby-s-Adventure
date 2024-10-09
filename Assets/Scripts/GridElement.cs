using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Properties to store the grid indices
    public int _currentRow;
    public int _currentColumn;

    // a mathematical XZ plane we will use for the dragging input
    private static Plane _dragPlane = new Plane(Vector3.up, 1f);

    // Reference to the grid to forward invoke methods
    private GridManager _grid;

    // The world position where the current dragging was started
    private Vector3 _startDragPoint;

    // Camera used to convert screen space mouse position to ray
    [SerializeField]
    private Camera _camera;

    // Properties to expose the current row and column
    public int CurrentRow => _currentRow;
    public int CurrentColumn => _currentColumn;

    public void Initialize(GridManager grid)
    {
        // Assign the camera
        if (!_camera) _camera = Camera.main;

        // Store the grid reference to later forward the input calls
        _grid = grid;
    }

    // Set the indices to the new values
    public void UpdateIndices(int row, int column)
    {
        _currentRow = row;
        _currentColumn = column;
    }

    // Called by the EventSystem when starting to drag this object
    public void OnBeginDrag(PointerEventData eventData)
    {
        var ray = _camera.ScreenPointToRay(eventData.position);

        if (_dragPlane.Raycast(ray, out var distance))
        {
            _startDragPoint = ray.GetPoint(distance);
        }
    }

    // Called by the EventSystem while dragging this object
    public void OnDrag(PointerEventData eventData)
    {
        var ray = _camera.ScreenPointToRay(eventData.position);

        if (_dragPlane.Raycast(ray, out var distance))
        {
            var currentDragPoint = ray.GetPoint(distance);
            var delta = currentDragPoint - _startDragPoint;

            // Only drag vertically or horizontally
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.z))
            {
                delta.x = Mathf.Clamp(delta.x, -1f, 1f);
                _grid.MoveRow(_currentRow, delta.x);
            }
            else
            {
                delta.z = Mathf.Clamp(delta.z, -1f, 1f);
                _grid.MoveColumn(_currentColumn, delta.z);
            }
        }
    }

    // Called by the EventSystem when stopping dragging this object
    public void OnEndDrag(PointerEventData eventData)
    {
        var ray = _camera.ScreenPointToRay(eventData.position);

        if (_dragPlane.Raycast(ray, out var distance))
        {
            var currentDragPoint = ray.GetPoint(distance);
            var delta = currentDragPoint - _startDragPoint;

            // Check against a threshold to shift the grid
            if (delta.x > 0.5f)
            {
                _grid.ShiftRowRight(_currentRow);
            }
            else if (delta.x < -0.5f)
            {
                _grid.ShiftRowLeft(_currentRow);
            }
            else if (delta.z > 0.5f)
            {
                _grid.ShiftColumnUp(_currentColumn);
            }
            else if (delta.z < -0.5f)
            {
                _grid.ShiftColumnDown(_currentColumn);
            }
            else
            {
                _grid.RefreshIndices();
            }
        }
    }
}
