using PSC.QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSC.Shorturl.Web.Controllers
{
    public class QRCodeController : Controller
    {
        private Bitmap getIconBitmap(string iconPath)
        {
            Bitmap img = null;
            if (iconPath.Length > 0)
            {
                try
                {
                    img = new Bitmap(iconPath);
                }
                catch (Exception)
                {
                }
            }
            return img;
        }

        private Bitmap renderQRCode(string text)
        {
            string level = "L";
            QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, eccLevel);
            QRCode qrCode = new QRCode(qrCodeData);

            return qrCode.GetGraphic(20, Color.Black, Color.White, getIconBitmap(""), 16);
        }

        // GET: QRCode
        public ActionResult Index(string text)
        {
            //Return Image
            MemoryStream ms = new MemoryStream();

            Image img = new Bitmap(100, 50);
            img = renderQRCode(text);
            img.Save(ms, ImageFormat.Png);

            ms.Position = 0;

            return new FileStreamResult(ms, "image/png");
        }
    }
}