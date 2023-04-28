using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class UnitTargetsBehaviour : MonoBehaviour
    {
        [Header("Targets")]
        public List<UnitController> targetUnits;

        public void AddTargetUnits(List<UnitController> addedUnits)
        {

            targetUnits.Clear();

            for(int i = 0; i < addedUnits.Count; i++)
            {
                targetUnits.Add(addedUnits[i]);
            }
        }

        public void RemoveTargetUnit(UnitController removedUnit)
        {
            targetUnits.Remove(removedUnit);
        }

        public List<UnitController> FilterTargetUnits(TargetType targetType)
        {
            
            List<UnitController> filteredUnits = new List<UnitController>();

            if(targetUnits.Count <= 0)
            {
                return filteredUnits;
            }
            
            switch (targetType)
            {
                case TargetType.RandomTarget:
                    int randomUnit = Random.Range(0, targetUnits.Count);
                    filteredUnits.Add(targetUnits[randomUnit]);
                    break;
                    
                case TargetType.AllTargets:
                    filteredUnits = targetUnits;
                    break;

            }
                
                return filteredUnits;
            
        }

        public UnitController GetRandomTargetUnit()
        {
            int randomUnit = Random.Range(0, targetUnits.Count);
            return targetUnits[randomUnit];
        }

        public List<UnitController> GetAllTargetUnits()
        {
            return targetUnits;
        }

    }


