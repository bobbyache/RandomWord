using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gui
{
    class WordButton : Button
    {
        public WordButton()
        {
            Id = Guid.NewGuid().ToString();
            FontSize = 24;
            Canvas.SetZIndex(this, 0);
            ToolTip = Canvas.GetZIndex(this).ToString();
            this.GotFocus += (s, e) =>
            {
                FontWeight = FontWeights.Bold;
                this.Foreground = new SolidColorBrush(Colors.Blue);
                BringToFront((Canvas)this.Parent, this);
            };
            this.LostFocus += (s, e) =>
            {
                FontWeight = FontWeights.Normal;
                this.Foreground = new SolidColorBrush(Colors.Black);
                Canvas.SetZIndex(this, 99);
                ToolTip = Canvas.GetZIndex(this).ToString();
            };
        }
        public string Id { get; private set; }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            Size size = MeasureTextSize(this.Content.ToString(), this.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch, this.FontSize);
            this.Width = size.Width + 50;
            this.Height = size.Height + 50;
        }

        

        /// <summary>
        /// Get the required height and width of the specified text. Uses FortammedText
        /// </summary>
        public static Size MeasureTextSize(string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize)
        {
            FormattedText ft = new FormattedText(text,
                                                 CultureInfo.CurrentCulture,
                                                 FlowDirection.LeftToRight,
                                                 new Typeface(fontFamily, fontStyle, fontWeight, fontStretch),
                                                 fontSize,
                                                 Brushes.Black);
            return new Size(ft.Width, ft.Height);
        }

        /// <summary>
        /// Get the required height and width of the specified text. Uses Glyph's
        /// </summary>
        public static Size MeasureText(string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize)
        {
            Typeface typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
            GlyphTypeface glyphTypeface;

            if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
            {
                return MeasureTextSize(text, fontFamily, fontStyle, fontWeight, fontStretch, fontSize);
            }

            double totalWidth = 0;
            double height = 0;

            for (int n = 0; n < text.Length; n++)
            {
                ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[n]];

                double width = glyphTypeface.AdvanceWidths[glyphIndex] * fontSize;

                double glyphHeight = glyphTypeface.AdvanceHeights[glyphIndex] * fontSize;

                if (glyphHeight > height)
                {
                    height = glyphHeight;
                }
                totalWidth += width;
            }

            return new Size(totalWidth, height);
        }

        static public void BringToFront(Canvas pParent, WordButton pToMove)
        {
            try
            {
                int currentIndex = Canvas.GetZIndex(pToMove);
                int zIndex = 0;
                int maxZ = 0;
                WordButton child;
                for (int i = 0; i < pParent.Children.Count; i++)
                {
                    if (pParent.Children[i] is WordButton &&
                        pParent.Children[i] != pToMove)
                    {
                        child = pParent.Children[i] as WordButton;
                        zIndex = Canvas.GetZIndex(child);
                        maxZ = Math.Max(maxZ, zIndex);
                        if (zIndex > currentIndex)
                        {
                            Canvas.SetZIndex(child, zIndex - 1);
                        }
                    }
                }
                Canvas.SetZIndex(pToMove, maxZ);
            }
            catch (Exception ex)
            {
            }
        }

    }
        }
