using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace WinUI3.NovaUI.Helpers
{
    public static class UiElementHelpers
    {
        /// <summary>
        /// Helper method to calculate the width of a grid column.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static double GetGridColumnWidth(Grid grid, int columnIndex)
        {
            var columnDefinitions = grid.ColumnDefinitions;
            double totalGridWidth = grid.ActualWidth;

            // Calculate the actual width of the column
            if (columnDefinitions[columnIndex].Width.IsAuto)
            {
                // Auto width: Use the actual size of the content in the column
                var child = grid.Children.Cast<FrameworkElement>()
                    .FirstOrDefault(e => Grid.GetColumn(e) == columnIndex);

                return child?.DesiredSize.Width ?? 0;
            }
            else if (columnDefinitions[columnIndex].Width.IsStar)
            {
                // Star width: Proportionally distribute the width
                double totalStar = columnDefinitions.Sum(c => c.Width.IsStar ? c.Width.Value : 0);
                double starRatio = columnDefinitions[columnIndex].Width.Value / totalStar;
                return totalGridWidth * starRatio;
            }
            else
            {
                // Fixed width
                return columnDefinitions[columnIndex].Width.Value;
            }
        }
    }
}
