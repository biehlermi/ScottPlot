﻿/* Code here extends Plot module with methods to construct plottables.
 *   - Plottables created here are added to the plottables list and returned.
 *   - Long lists of optional arguments (matplotlib style) are permitted.
 *   - Use one line per argument to simplify the tracking of changes.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlot
{
    public partial class Plot
    {

        public PlottableVectorField PlotVectorField(
            Statistics.Vector2[,] vectors,
            double[] xs,
            double[] ys,
            string label = null,
            Color? color = null,
            Drawing.Colormap colormap = null,
            double scaleFactor = 1
            )
        {
            if (!color.HasValue)
            {
                color = settings.GetNextColor();
            }

            var vectorField = new PlottableVectorField(vectors, xs, ys, label, color.Value, colormap, scaleFactor);

            Add(vectorField);
            return vectorField;
        }

        public PlottableScatter PlotArrow(
            double tipX,
            double tipY,
            double baseX,
            double baseY,
            double lineWidth = 5,
            float arrowheadWidth = 3,
            float arrowheadLength = 3,
            Color? color = null,
            string label = null
            )
        {

            var arrow = PlotScatter(
                                        xs: new double[] { baseX, tipX },
                                        ys: new double[] { baseY, tipY },
                                        color: color,
                                        lineWidth: lineWidth,
                                        label: label,
                                        markerSize: 0
                                    );

            var arrowCap = new System.Drawing.Drawing2D.AdjustableArrowCap(arrowheadWidth, arrowheadLength, isFilled: true);
            arrow.penLine.CustomEndCap = arrowCap;
            arrow.penLine.StartCap = System.Drawing.Drawing2D.LineCap.Flat;

            return arrow;
        }

        public PlottableRadar PlotRadar(
            double[,] values,
            string[] categoryNames = null,
            string[] groupNames = null,
            Color[] fillColors = null,
            double fillAlpha = .4,
            Color? webColor = null
            )
        {
            fillColors = fillColors ?? Enumerable.Range(0, values.Length).Select(i => settings.colorset.GetColor(i)).ToArray();
            webColor = webColor ?? Color.Gray;

            var plottable = new PlottableRadar(values, categoryNames, groupNames, fillColors, (byte)(fillAlpha * 256), webColor.Value);
            Add(plottable);
            MatchAxis(this);

            return plottable;
        }

        public PlottableFunction PlotFunction(
            Func<double, double?> function,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 0,
            string label = "f(x)",
            MarkerShape markerShape = MarkerShape.filledCircle,
            LineStyle lineStyle = LineStyle.Solid
        )
        {
            if (color == null)
            {
                color = settings.GetNextColor();
            }

            PlottableFunction functionPlot = new PlottableFunction(function, color.Value, lineWidth, markerSize, label, markerShape, lineStyle);

            Add(functionPlot);
            return functionPlot;
        }

        public PlottableScaleBar PlotScaleBar(
            double sizeX,
            double sizeY,
            string labelX = null,
            string labelY = null,
            double thickness = 2,
            double fontSize = 12,
            Color? color = null,
            double padPx = 10
            )
        {
            color = (color is null) ? Color.Black : color.Value;
            var scalebar = new PlottableScaleBar(sizeX, sizeY, labelX, labelY, thickness, fontSize, color.Value, padPx);
            Add(scalebar);
            return scalebar;
        }

        public PlottablePie PlotPie(
            double[] values,
            string[] sliceLabels = null,
            Color[] colors = null,
            bool explodedChart = false,
            bool showValues = false,
            bool showPercentages = false,
            bool showLabels = true,
            string label = null
            )
        {
            if (colors is null)
                colors = Enumerable.Range(0, values.Length).Select(i => settings.colorset.GetColor(i)).ToArray();

            PlottablePie pie = new PlottablePie(values, sliceLabels, colors, explodedChart, showValues, showPercentages, showLabels, label);

            Add(pie);
            return pie;
        }

    }
}
