using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Util;

namespace Data
{
    [Serializable]
    public class ChartValue
    {
        public enum ChartInterpolationType
        {
            Linear,
            Exponential
        }

        [SerializeField] private long[] points = new long[] { };
        [SerializeField] long xmin = 0;
        [SerializeField] long xmax = 1;
        [SerializeField] long ymin;
        [SerializeField] long ymax;
        [SerializeField] ChartInterpolationType interpolationType;


        public void SetPoint(long x, long y)
        {
            if (x < xmin || x > xmax)
            {
                Debug.LogError("X value is out of range");
                return;
            }

            if (y < ymin || y > ymax)
            {
                Debug.LogError("Y value is out of range");
                return;
            }

            points[x - xmin] = y;
        }

        public IReadOnlyList<long> GetPoints()
        {
            return points.ToList();
        }

        public long GetPoint(long x)
        {
            if (x < xmin || x > xmax)
            {
                Debug.LogError("X value is out of range");
                return 0;
            }

            return points[x - xmin];
        }

        public static void Validate(ChartValue chart)
        {
            chart.xmin = 0;
            if (0 == chart.xmax)
            {
                chart.xmax = 1;
            }

            if (chart.points.Length != CalcuatePointsSize(chart.xmin, chart.xmax))
            {
                chart.points = new long[CalcuatePointsSize(chart.xmin, chart.xmax)];
            }

            for (int i = 0; i < chart.points.Length; i++)
            {
                chart.points[i] = ChartValue.CaculateYValue(chart.interpolationType, chart.xmin, chart.xmax, chart.ymin,
                    chart.ymax, i);
            }
        }

        public static void ForceXMax(ChartValue chart, long xmax)
        {
            if (xmax != chart.xmax)
            {
                chart.xmax = xmax;
            }
        }

        public static long CalcuatePointsSize(long xmin, long xmax)
        {
            return (long)Mathf.Clamp(xmax - xmin, 0, Int64.MaxValue);
        }

        public static long CaculateYValue(ChartInterpolationType interpolationType, long xmin, long xmax, long ymin,
            long ymax, int index)
        {
            var xdelta = (long)Mathf.Clamp(xmax - xmin, 0, Int64.MaxValue);
            switch (interpolationType)
            {
                case ChartInterpolationType.Linear:
                {
                    return (long)Interpolation.Linear(ymin, ymax, (float)index / (float)xdelta);
                }
                case ChartInterpolationType.Exponential:
                {
                    return (long)Interpolation.Exponential(ymin, ymax, (float)index / (float)xdelta);
                }
            }

            throw new Exception("Unknown interpolation type");
        }
    }
}