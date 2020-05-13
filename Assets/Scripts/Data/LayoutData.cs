using Data;
using UnityEngine;

namespace Drillings.Data
{
    [CreateAssetMenu(fileName = "New LayoutData", menuName = "Layout Data", order = 52)]
    public class LayoutData : ScriptableObject
    {
        public ObjectInfoUnitData CurrentUnit => units[CurrentUnitId];
        public int UnitsCount => units.Length;
        public bool IsLastUnit => CurrentUnitId == units.Length - 1;
        public int CurrentUnitId { get; private set; }

        public string objectName;
        public ObjectInfoUnitData[] units;

        public bool NextUnit()
        {
            if (CurrentUnitId >= units.Length - 1)
                return false;

            CurrentUnitId++;
            return true;
        }

        public bool PrevUnit()
        {
            if (CurrentUnitId <= 0)
                return false;

            CurrentUnitId--;
            return true;
        }

        public bool OpenUnit(int unitId)
        {
            if (unitId >= units.Length || unitId < 0)
                return false;

            CurrentUnitId = unitId;
            return true;
        }

        public void ResetUnit()
        {
            CurrentUnitId = 0;
        }
    }
}