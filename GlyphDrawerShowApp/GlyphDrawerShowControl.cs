using CustomControlBaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace GlyphDrawerShowApp{

  public class GlyphDrawerShowControl: Control {

    #region Constructor
    //      -----------

    readonly GlyphDrawer labelGlyphDrawer;
    readonly GlyphDrawer sampleGlyphDrawer;


    public GlyphDrawerShowControl() {
      labelGlyphDrawer = new(FontFamily, FontStyle, FontWeights.Bold, FontStretch, VisualTreeHelper.GetDpi(this).PixelsPerDip);
      sampleGlyphDrawer = new(FontFamily, FontStyle, FontWeights.Normal, FontStretch, VisualTreeHelper.GetDpi(this).PixelsPerDip);
    }
    #endregion


    #region Overrides
    //      ---------

    readonly Brush blueBGBrush = new SolidColorBrush(Color.FromArgb(0x14, 0, 0, 0xC0));
    readonly Brush greenBGBrush = new SolidColorBrush(Color.FromArgb(0x14, 0, 0xC0, 0));

    const int rowsCount = 5;
    const int columnsCount = 5;
    const int fontSize = 12;
    const int stringHeight = fontSize + 3;
    const int labelRowHeight = fontSize + 8;
    const int labelColumnWidth = 120;
    const int labelMarginTop = 5 + fontSize;
    const int labelMarginLeft = 5;
    int rowHeight, rowHeightHalf;
    int columnWidth, columnWidthHalf;


    protected override void OnRender(DrawingContext drawingContext) {
      rowHeight = (int)(RenderSize.Height - labelRowHeight) / rowsCount;
      rowHeightHalf = rowHeight / 2;
      columnWidth = (int)(RenderSize.Width - labelColumnWidth) / columnsCount;
      columnWidthHalf = columnWidth / 2;

      //draw background rectangles
      int y = labelRowHeight;
      drawingContext.DrawRectangle(blueBGBrush, null, new Rect(0, y, RenderSize.Width, rowHeight));
      y += 2 * rowHeight;
      drawingContext.DrawRectangle(blueBGBrush, null, new Rect(0, y, RenderSize.Width, 1.5*rowHeight));

      int x = labelColumnWidth;
      drawingContext.DrawRectangle(greenBGBrush, null, new Rect(x, 0, columnWidth, RenderSize.Height));
      x += 2 * columnWidth;
      drawingContext.DrawRectangle(greenBGBrush, null, new Rect(x, 0, columnWidth, RenderSize.Height));
      x += 2 * columnWidth;
      drawingContext.DrawRectangle(greenBGBrush, null, new Rect(x, 0, columnWidth, RenderSize.Height));

      //write labels
      x = labelColumnWidth - labelMarginLeft;
      y = labelMarginTop;
      labelGlyphDrawer.Write(drawingContext, new Point(x, y), "Angle", fontSize, Foreground, isRightAligned: true);
      x = labelMarginLeft + labelColumnWidth;
      labelGlyphDrawer.Write(drawingContext, new Point(x, y), "0", fontSize, Foreground);
      x += columnWidth;
      labelGlyphDrawer.Write(drawingContext, new Point(x, y), "30", fontSize, Foreground);
      x += columnWidth;
      labelGlyphDrawer.Write(drawingContext, new Point(x, y), "90", fontSize, Foreground);
      x += columnWidth;
      labelGlyphDrawer.Write(drawingContext, new Point(x, y), "180", fontSize, Foreground);
      x += columnWidth;
      labelGlyphDrawer.Write(drawingContext, new Point(x, y), "270", fontSize, Foreground);

      x = labelMarginLeft;
      y = labelRowHeight + labelMarginTop;
      labelGlyphDrawer.Write(drawingContext, new Point(x, y), "RightAligned: false", fontSize, Foreground);
      labelGlyphDrawer.Write(drawingContext, new Point(x, y + stringHeight), "Sideways: false", fontSize, Foreground);
      y += rowHeight;
      labelGlyphDrawer.Write(drawingContext, new Point(x, y), "RightAligned: true", fontSize, Foreground);
      labelGlyphDrawer.Write(drawingContext, new Point(x, y + stringHeight), "Sideways: false", fontSize, Foreground);
      y += rowHeight;
      labelGlyphDrawer.Write(drawingContext, new Point(x, y), "RightAligned: true", fontSize, Foreground);
      labelGlyphDrawer.Write(drawingContext, new Point(x, y + stringHeight), "Sideways: true", fontSize, Foreground);
      y += 2*rowHeight;
      labelGlyphDrawer.Write(drawingContext, new Point(x, y), "RightAligned: false", fontSize, Foreground);
      labelGlyphDrawer.Write(drawingContext, new Point(x, y + stringHeight), "Sideways: true", fontSize, Foreground);

      //draw samples
      x = labelColumnWidth;
      y = labelRowHeight;
      drawSample(drawingContext, x, y, isRightAligned: false, isSideways: false, angle: 0, Brushes.Blue);
      x += columnWidth;
      drawSample(drawingContext, x, y, isRightAligned: false, isSideways: false, angle: 30, Brushes.Blue);
      x += columnWidth;
      drawSample(drawingContext, x, y, isRightAligned: false, isSideways: false, angle: 90, Brushes.Blue);
      x += columnWidth;
      drawSample(drawingContext, x, y, isRightAligned: false, isSideways: false, angle: 180, Brushes.Blue);
      x += columnWidth;
      drawSample(drawingContext, x, y, isRightAligned: false, isSideways: false, angle: 270, Brushes.Blue);

      x = labelColumnWidth;
      y += rowHeight;
      drawSample(drawingContext, x, y, isRightAligned: true, isSideways: false, angle: 0, Brushes.Black);
      x += columnWidth;
      drawSample(drawingContext, x, y, isRightAligned: true, isSideways: false, angle: 30, Brushes.Black);
      x += columnWidth;
      drawSample(drawingContext, x, y, isRightAligned: true, isSideways: false, angle: 90, Brushes.Black);
      x += columnWidth;
      drawSample(drawingContext, x, y, isRightAligned: true, isSideways: false, angle: 180, Brushes.Black);
      x += columnWidth;
      drawSample(drawingContext, x, y, isRightAligned: true, isSideways: false, angle: 270, Brushes.Black);

      x = labelColumnWidth;
      y += rowHeight;
      drawSample(drawingContext, x, y, isRightAligned: true, isSideways: true, angle: 0, Brushes.Blue);
      x += columnWidth;
      drawSample(drawingContext, x, y, isRightAligned: true, isSideways: true, angle: 30, Brushes.Blue);
      x += columnWidth;
      drawSample(drawingContext, x, y, isRightAligned: true, isSideways: true, angle: 90, Brushes.Blue);
      x += columnWidth;
      drawSample(drawingContext, x, y, isRightAligned: true, isSideways: true, angle: 180, Brushes.Blue);
      x += columnWidth;
      drawSample(drawingContext, x, y, isRightAligned: true, isSideways: true, angle: 270, Brushes.Blue);

      x = labelColumnWidth;
      y += 2*rowHeight;
      drawSample(drawingContext, x, y, isRightAligned: false, isSideways: true, angle: 0, Brushes.Black);
      x += columnWidth;
      drawSample(drawingContext, x, y, isRightAligned: false, isSideways: true, angle: 30, Brushes.Black);
      x += columnWidth;
      drawSample(drawingContext, x, y, isRightAligned: false, isSideways: true, angle: 90, Brushes.Black);
      x += columnWidth;
      drawSample(drawingContext, x, y, isRightAligned: false, isSideways: true, angle: 180, Brushes.Black);
      x += columnWidth;
      drawSample(drawingContext, x, y, isRightAligned: false, isSideways: true, angle: 270, Brushes.Black);
    }


    readonly Pen coordinatePen = new Pen(Brushes.LightBlue, 1);
    const int sampleMargin = 5;


    private void drawSample(DrawingContext drawingContext, int xLeft, int yTop, bool isRightAligned, bool isSideways, 
      int angle, Brush fontBrush) {
      var xMiddle = xLeft + columnWidthHalf;
      var yMiddle = yTop + rowHeightHalf;
      var xRight = xLeft + columnWidth - sampleMargin;
      var yBottom = yTop + rowHeight - sampleMargin;
      xLeft += sampleMargin;
      yTop += sampleMargin;
      drawingContext.DrawLine(coordinatePen, new Point(xMiddle, yTop), new Point(xMiddle, yBottom));
      drawingContext.DrawLine(coordinatePen, new Point(xLeft, yMiddle), new Point(xRight, yMiddle));
      var nextPoint = sampleGlyphDrawer.Write(drawingContext, new Point(xMiddle, yMiddle), "Test Text",
        fontSize, fontBrush, isRightAligned, isSideways, angle);
      //var nextPen1 = new Pen(Brushes.DarkBlue, 1);
      //drawingContext.DrawLine(nextPen1, new Point(nextPoint.X-FontSize, nextPoint.Y), new Point(nextPoint.X+FontSize, nextPoint.Y));
      //drawingContext.DrawLine(nextPen1, new Point(nextPoint.X, nextPoint.Y-FontSize), new Point(nextPoint.X, nextPoint.Y+FontSize));
      //mainWindow.NextPoint1 = new Point(nextPoint.X - offsetX, nextPoint.Y - offsetY);
    }
    #endregion
  }
}
