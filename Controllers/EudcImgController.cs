using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.IO;

namespace Turbo.MVC.Base3.Controllers
{
    public class EudcImgController : Controller
    {
        // GET: EudcImg
        public ActionResult PNG(string text, string color, string bkColor, string fontSize)
        {
            Color textColor, backColor;
            int textFontSize = 16;
            FontFamily fontFamily = new FontFamily("MINGLIU");
            
            if (string.IsNullOrEmpty(color))
            {
                textColor = Color.Black;
            }
            else
            {
                if (!color.StartsWith("#"))
                {
                    color = "#" + color;
                }
                textColor = ColorTranslator.FromHtml(color);
            }
            if (string.IsNullOrEmpty(bkColor))
            {
                backColor = Color.Transparent;
            }
            else
            {
                if (!bkColor.StartsWith("#"))
                {
                    bkColor = "#" + bkColor;
                }
                backColor = ColorTranslator.FromHtml(bkColor);
            }

            if(!string.IsNullOrEmpty(fontSize))
            {
                int iFontSize;
                if (int.TryParse(fontSize, out iFontSize))
                {
                    textFontSize = iFontSize;
                }
            }

            if (string.IsNullOrEmpty(text))
            {
                text = "null";
            }

            MemoryStream ms = new MemoryStream();
            using (Font font = new Font(
               fontFamily,
               textFontSize,
               FontStyle.Regular,
               GraphicsUnit.Pixel))
            {
                using (Image img = DrawTextImage(text, font, textColor, backColor, Size.Empty))
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            return File(ms.ToArray(), "image/png");
        }

        private Image DrawTextImage(String text, Font font, Color textColor, Color backColor, Size minSize)
        {
            //first, create a dummy bitmap just to get a graphics object
            SizeF textSize;
            using (Image img = new Bitmap(1, 1))
            {
                using (Graphics drawing = Graphics.FromImage(img))
                {
                    //measure the string to see how big the image needs to be
                    textSize = drawing.MeasureString(text, font);
                    textSize.Width = (float)(textSize.Width * 0.74);  // MeasureString結果左右空間會多一些空間,不用那麼大
                    if (!minSize.IsEmpty)
                    {
                        textSize.Width = textSize.Width > minSize.Width ? textSize.Width : minSize.Width;
                        textSize.Height = textSize.Height > minSize.Height ? textSize.Height : minSize.Height;
                    }
                }
            }

            //create a new image of the right size
            Image retImg = new Bitmap((int)textSize.Width, (int)textSize.Height);
            using (var drawing = Graphics.FromImage(retImg))
            {
                //paint the background
                drawing.Clear(backColor);

                // 避免文字變粗
                drawing.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                //drawing.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;

                // 避免文字左側留白
                StringFormat format = StringFormat.GenericTypographic;

                //create a brush for the text
                using (Brush textBrush = new SolidBrush(textColor))
                {
                    //float y = (float)((retImg.Height / 10)); // 文字上方留白 height * 10%
                    //drawing.DrawString(text, font, textBrush, 0, y, format);
                    drawing.DrawString(text, font, textBrush, 0, 0, format);
                    drawing.Save();
                }
            }
            return retImg;
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}