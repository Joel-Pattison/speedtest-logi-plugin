namespace Loupedeck.SpeedTestPlugin.Helpers
{
    using System;
    using System.IO;

    using SkiaSharp;

    /// <summary>SkiaSharp-based image builder with text measurement and rendering.</summary>
    public class ImageBuilder : IDisposable
    {
        private readonly SKBitmap _bitmap;
        private readonly SKCanvas _canvas;
        private static SKTypeface _defaultTypeface;
        private static readonly Object _typefaceLock = new();

        public Int32 Width { get; }
        public Int32 Height { get; }

        public ImageBuilder(Int32 width, Int32 height)
        {
            this.Width = width;
            this.Height = height;
            this._bitmap = new SKBitmap(width, height);
            this._canvas = new SKCanvas(this._bitmap);
            InitializeFont();
        }

        private static void InitializeFont()
        {
            if (_defaultTypeface != null)
            {
                return;
            }

            lock (_typefaceLock)
            {
                if (_defaultTypeface != null)
                {
                    return;
                }

                _defaultTypeface = TryLoadEmbeddedFont(PluginConstants.FontFileName)
                    ?? TryGetTypeface("Segoe UI", SKFontStyleWeight.Medium)
                    ?? TryGetTypeface("Helvetica Neue", SKFontStyleWeight.Medium)
                    ?? SKTypeface.Default;

                PluginLog.Info($"ImageBuilder: Using font: {_defaultTypeface.FamilyName}");
            }
        }

        /// <summary>Loads a font from embedded resources.</summary>
        private static SKTypeface TryLoadEmbeddedFont(String fileName)
        {
            try
            {
                using (var stream = PluginResources.GetStream(fileName))
                {
                    if (stream == null)
                    {
                        return null;
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        using (var skData = SKData.CreateCopy(memoryStream.ToArray()))
                        {
                            return SKTypeface.FromData(skData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PluginLog.Warning(ex, $"ImageBuilder: Failed to load embedded font '{fileName}'");
                return null;
            }
        }

        /// <summary>Loads a typeface by family name, returns null if unavailable.</summary>
        private static SKTypeface TryGetTypeface(String familyName, SKFontStyleWeight weight)
        {
            var typeface = SKTypeface.FromFamilyName(familyName, weight, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
            if (typeface != null && typeface.FamilyName.Equals(familyName, StringComparison.OrdinalIgnoreCase))
            {
                return typeface;
            }

            typeface?.Dispose();
            return null;
        }

        /// <summary>Clears the canvas.</summary>
        public void Clear(SKColor color) => this._canvas.Clear(color);

        /// <summary>Draws text at the specified position.</summary>
        public void DrawText(String text, Int32 x, Int32 y, SKColor color, Int32 fontSize)
        {
            if (String.IsNullOrEmpty(text))
            {
                return;
            }

            using (var paint = new SKPaint())
            {
                paint.Typeface = _defaultTypeface;
                paint.TextSize = fontSize;
                paint.Color = color;
                paint.IsAntialias = true;
                paint.TextAlign = SKTextAlign.Left;

                var baseline = GetBaselineFromTop(fontSize, y);
                this._canvas.DrawText(text, x, baseline, paint);
            }
        }

        /// <summary>Draws a filled rectangle.</summary>
        public void FillRectangle(Int32 x, Int32 y, Int32 width, Int32 height, SKColor color)
        {
            using (var paint = new SKPaint())
            {
                paint.Color = color;
                paint.Style = SKPaintStyle.Fill;
                this._canvas.DrawRect(x, y, width, height, paint);
            }
        }

        /// <summary>Draws a rectangle outline.</summary>
        public void DrawRectangle(Int32 x, Int32 y, Int32 width, Int32 height, SKColor color, Single strokeWidth = 1f)
        {
            using (var paint = new SKPaint())
            {
                paint.Color = color;
                paint.Style = SKPaintStyle.Stroke;
                paint.StrokeWidth = strokeWidth;
                this._canvas.DrawRect(x, y, width, height, paint);
            }
        }

        /// <summary>Draws a filled circle.</summary>
        public void FillCircle(Single centerX, Single centerY, Single radius, SKColor color)
        {
            using (var paint = new SKPaint())
            {
                paint.Color = color;
                paint.Style = SKPaintStyle.Fill;
                this._canvas.DrawCircle(centerX, centerY, radius, paint);
            }
        }

        /// <summary>Draws a circle outline.</summary>
        public void DrawCircle(Single centerX, Single centerY, Single radius, SKColor color, Single strokeWidth = 1f)
        {
            using (var paint = new SKPaint())
            {
                paint.Color = color;
                paint.Style = SKPaintStyle.Stroke;
                paint.StrokeWidth = strokeWidth;
                this._canvas.DrawCircle(centerX, centerY, radius, paint);
            }
        }

        /// <summary>Draws a line.</summary>
        public void DrawLine(Single x1, Single y1, Single x2, Single y2, SKColor color, Single strokeWidth = 1f)
        {
            using (var paint = new SKPaint())
            {
                paint.Color = color;
                paint.StrokeWidth = strokeWidth;
                this._canvas.DrawLine(x1, y1, x2, y2, paint);
            }
        }

        /// <summary>Draws an image at the specified position.</summary>
        public void DrawImage(Byte[] imageBytes, Int32 x, Int32 y)
        {
            if (imageBytes == null)
            {
                return;
            }

            using (var bitmap = SKBitmap.Decode(imageBytes))
            {
                if (bitmap != null)
                {
                    this._canvas.DrawBitmap(bitmap, x, y);
                }
            }
        }

        /// <summary>Draws an image scaled to the specified size.</summary>
        public void DrawImage(Byte[] imageBytes, Int32 x, Int32 y, Int32 width, Int32 height)
        {
            if (imageBytes == null)
            {
                return;
            }

            using (var bitmap = SKBitmap.Decode(imageBytes))
            {
                if (bitmap != null)
                {
                    var destRect = new SKRect(x, y, x + width, y + height);
                    this._canvas.DrawBitmap(bitmap, destRect);
                }
            }
        }

        /// <summary>Draws text centered on both axes.</summary>
        public void DrawCenteredText(String text, Int32 fontSize, SKColor color, Int32 width, Int32 height)
        {
            var textWidth = MeasureTextWidth(text, fontSize);
            var textHeight = MeasureTextHeight(fontSize, text);
            var x = (width - textWidth) / 2;
            var y = (height - textHeight) / 2;
            this.DrawText(text, x, y, color, fontSize);
        }

        /// <summary>Draws text centered horizontally.</summary>
        public void DrawHorizontallyCenteredText(String text, Int32 fontSize, SKColor color, Int32 y, Int32 width)
        {
            var textWidth = MeasureTextWidth(text, fontSize);
            var x = (width - textWidth) / 2;
            this.DrawText(text, x, y, color, fontSize);
        }

        /// <summary>Draws text centered vertically.</summary>
        public void DrawVerticallyCenteredText(String text, Int32 fontSize, SKColor color, Int32 x, Int32 height)
        {
            var textHeight = MeasureTextHeight(fontSize, text);
            var y = (height - textHeight) / 2;
            this.DrawText(text, x, y, color, fontSize);
        }

        /// <summary>Draws an image centered with padding.</summary>
        public void DrawCenteredImage(Byte[] imageBytes, Int32 width, Int32 height, Int32 padding)
        {
            var imageSize = Math.Min(width, height) - padding;
            var x = (width - imageSize) / 2;
            var y = (height - imageSize) / 2;
            this.DrawImage(imageBytes, x, y, imageSize, imageSize);
        }

        /// <summary>Measures text width.</summary>
        public static Int32 MeasureTextWidth(String text, Int32 fontSize)
        {
            InitializeFont();

            using (var paint = new SKPaint())
            {
                paint.Typeface = _defaultTypeface;
                paint.TextSize = fontSize;
                return (Int32)Math.Ceiling(paint.MeasureText(text));
            }
        }

        /// <summary>Measures text height.</summary>
        public static Int32 MeasureTextHeight(Int32 fontSize, String text)
        {
            InitializeFont();

            using (var paint = new SKPaint())
            {
                paint.Typeface = _defaultTypeface;
                paint.TextSize = fontSize;

                var bounds = new SKRect();
                paint.MeasureText(text, ref bounds);
                return (Int32)Math.Ceiling(bounds.Height);
            }
        }

        /// <summary>Converts a top Y position to a baseline Y position.</summary>
        public static Int32 GetBaselineFromTop(Int32 fontSize, Int32 topY)
        {
            InitializeFont();

            using (var paint = new SKPaint())
            {
                paint.Typeface = _defaultTypeface;
                paint.TextSize = fontSize;

                return (Int32)(topY - paint.FontMetrics.Ascent);
            }
        }

        /// <summary>Gets the full line height for a font size.</summary>
        public static Int32 GetFontMaxHeight(Int32 fontSize)
        {
            InitializeFont();

            using (var paint = new SKPaint())
            {
                paint.Typeface = _defaultTypeface;
                paint.TextSize = fontSize;

                var metrics = paint.FontMetrics;
                return (Int32)Math.Ceiling(metrics.Descent - metrics.Ascent);
            }
        }

        /// <summary>Exports as a BitmapImage.</summary>
        public BitmapImage ToBitmapImage()
        {
            this._canvas.Flush();
            using (var image = SKImage.FromBitmap(this._bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            {
                return BitmapImage.FromArray(data.ToArray());
            }
        }

        /// <summary>Exports as a byte array.</summary>
        public Byte[] ToArray()
        {
            this._canvas.Flush();
            using (var image = SKImage.FromBitmap(this._bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            {
                return data.ToArray();
            }
        }

        public void Dispose()
        {
            this._canvas?.Dispose();
            this._bitmap?.Dispose();
        }
    }
}