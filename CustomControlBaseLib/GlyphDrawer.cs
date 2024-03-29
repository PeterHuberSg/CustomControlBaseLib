﻿/**************************************************************************************

CustomControlBaseLib.GlyphDrawer
================================

Writes text to a DrawingContext. Can also be used to calculate the length of text.

Written 2014 - 2020 by Jürgpeter Huber by Jürgpeter Huber 
Contact: PeterCode at Peterbox dot com

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Media;


namespace CustomControlBaseLib {

  /// <summary>
  /// Draws glyphs to a DrawingContext. From the font information in the constructor, GlyphDrawer creates and stores 
  /// the GlyphTypeface, which is used every time for the drawing of the string. Can also be used to calculate the
  /// length of text.
  /// </summary>
  public class GlyphDrawer {

    readonly Typeface typeface;


    /// <summary>
    /// Contains the measurement information of one particular font and the specified font properties
    /// </summary>
    public GlyphTypeface GlyphTypeface {
      get { return glyphTypeface; }
    }
    readonly GlyphTypeface glyphTypeface;


    /// <summary>
    /// Screen resolution. 
    /// </summary>
    public float PixelsPerDip { get; }


    /// <summary>
    /// Construct a GlyphTypeface with the specified font properties
    /// </summary>
    public GlyphDrawer(FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight,
      FontStretch fontStretch, double pixelsPerDip) 
    {
      typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
      if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
        throw new InvalidOperationException("No GlyphTypeface found");

      PixelsPerDip = (float)pixelsPerDip;
    }


    /// <summary>
    /// Writes a string to a DrawingContext, using the GlyphTypeface stored in the GlyphDrawer.
    /// </summary>
    /// <param name="drawingContext"></param>
    /// <param name="origin"></param>
    /// <param name="text"></param>
    /// <param name="size">same unit like FontSize: (em)</param>
    /// <param name="brush"></param>
    public void Write(DrawingContext drawingContext, Point origin, string text, double size, Brush brush) {
      if (string.IsNullOrEmpty(text)) return;

      double totalWidth = 0;
      ushort[] glyphIndexes = new ushort[text.Length];
      double[] advanceWidths = new double[text.Length];

      var glyphIndexesIdex = 0;
      for (int charIndex = 0; charIndex<text.Length; charIndex++) {
        var codePoint = (int)text[charIndex];
        //https://docs.microsoft.com/en-us/dotnet/standard/base-types/character-encoding-introduction#surrogate-pairs
        if (codePoint<0xd800) {
          // codePoint consists of only 1 integer, nothing to do
        } else if (codePoint<0xdc00) {
          //high surrogate code point
          if (charIndex>=text.Length) {
            //low surrogate code pointmissing
            System.Diagnostics.Debugger.Break();
            codePoint = (int)'?';
          } else {
            var lowCodPoint = (int)text[++charIndex];
            if (lowCodPoint<0xdc00 || lowCodPoint>=0xe000) {
              //illeagel second surrogate code point
              System.Diagnostics.Debugger.Break();
              codePoint = (int)'?';
            } else {
              codePoint = 0x10000 + ((codePoint - 0xD800) *0x0400) + (lowCodPoint - 0xDC00);
            }
          }
        } else if (codePoint<0xe000) {
          //illeagel low surrogate code point, high should come first
          System.Diagnostics.Debugger.Break();
          codePoint = (int)'?';
        } else {
          // codePoint consists of only 1 integer, nothing to do
        }
        //ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[codePoint];
        if (!glyphTypeface.CharacterToGlyphMap.TryGetValue(codePoint, out var glyphIndex)) {
          glyphIndex = glyphTypeface.CharacterToGlyphMap[(int)'?'];
        };
        glyphIndexes[glyphIndexesIdex] = glyphIndex;
        double width = glyphTypeface.AdvanceWidths[glyphIndex] * size;
        advanceWidths[glyphIndexesIdex++] = width;
        totalWidth += width;
      }

      if (glyphIndexes.Length!=glyphIndexesIdex) {
        glyphIndexes = glyphIndexes[0..glyphIndexes.Length];
        advanceWidths = advanceWidths[0..glyphIndexes.Length];
      }
      GlyphRun glyphRun = new GlyphRun(glyphTypeface, 0, false, size, PixelsPerDip, glyphIndexes, origin, advanceWidths, null, null, null, null, null, null);

      drawingContext.DrawGlyphRun(brush, glyphRun);
      /*
                                  dxUnderline,
                                  penThickness
                              );

                          // Apply the pair of guidelines: one for baseline and another
                          // for top edge of undelining line. Both will be snapped to pixel grid.
                          // Guideline pairing algorithm detects the case when these two
                          // guidelines happen to be close to one another and provides
                          // synchronous snapping, so that the gap between baseline and
                          // undelining line does not depend on the position of text line.
                          drawingContext.PushGuidelineY2(y, lineOrigin.Y - penThickness * 0.5 - y);

                          try
                          {
                              drawingContext.DrawRectangle(
                                  foregroundBrush,
                                  null,               // pen
                                  underlineRect
                                  );
                          }
                          finally
                          {
                              drawingContext.Pop();
                          }
       */
    }


    /// <summary>
    /// Writes a string to a DrawingContext, using the GlyphTypeface stored in the GlyphDrawer. The text will be right aligned. The
    /// last character will be at Origin, all other characters in front.
    /// </summary>
    /// <param name="drawingContext"></param>
    /// <param name="origin"></param>
    /// <param name="text"></param>
    /// <param name="size">same unit like FontSize: (em)</param>
    /// <param name="brush"></param>
    public void WriteRightAligned(DrawingContext drawingContext, Point origin, string text, double size, Brush brush) {
      if (string.IsNullOrEmpty(text)) return;

      ushort[] glyphIndexes = new ushort[text.Length];
      double[] advanceWidths = new double[text.Length];

      double totalWidth = 0;

      for (int charIndex = 0; charIndex<text.Length; charIndex++) {
        ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[charIndex]];
        glyphIndexes[charIndex] = glyphIndex;

        double width = glyphTypeface.AdvanceWidths[glyphIndex] * size;
        advanceWidths[charIndex] = width;

        totalWidth += width;
      }

      Point newOrigin = new Point(origin.X - totalWidth, origin.Y);
      GlyphRun glyphRun = new GlyphRun(glyphTypeface, 0, false, size, PixelsPerDip, glyphIndexes, newOrigin, advanceWidths, null, null, null, null, null, null);

      drawingContext.DrawGlyphRun(brush, glyphRun);
    }


    /// <summary>
    /// Returns the length of the text using the GlyphTypeface stored in the GlyphDrawer. 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="size">same unit like FontSize: (em)</param>
    /// <returns></returns>
    public double GetLength(string text, double size) {
      double length = 0;

      for (int charIndex = 0; charIndex<text.Length; charIndex++) {
        ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[charIndex]];
        double width = glyphTypeface.AdvanceWidths[glyphIndex] * size;
        length += width;
      }
      return length;
    }


    /// <summary>
    /// calculates the width of each string in strings and returns the longest length.
    /// </summary>
    public double GetMaxLength(IEnumerable<string> strings, double size) {
      var maxLength = 0.0;
      foreach (var text in strings) {
        double length = 0;
        for (int charIndex = 0; charIndex<text.Length; charIndex++) {
          ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[charIndex]];
          double width = glyphTypeface.AdvanceWidths[glyphIndex] * size;
          length += width;
        }
        maxLength = Math.Max(maxLength, length);
      }
      return maxLength;
    }


    /// <summary>
    /// Returns width and height of text
    /// </summary>
    public Size MeasureString(string text, double size) {
      var formattedText = new FormattedText(text, CultureInfo.CurrentUICulture,
                                              FlowDirection.LeftToRight,
                                              typeface,
                                              size,
                                              Brushes.Black,
                                              PixelsPerDip);

      return new Size(formattedText.Width, formattedText.Height);
    }
  }
}