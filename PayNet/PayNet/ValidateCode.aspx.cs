using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace PayNet
{
    public partial class ValidateCode : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        private const Int32 CodeCount = 4;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            String radom = CommonUntils.CreateRandomCode(CodeCount);
            HttpContext.Current.Session["CheckCode"] = radom;
            ResponseHandler.AddCookie(this.Page, "CheckCode", radom);

            CreateCodeImg(radom, HttpContext.Current);
        }

        /// <summary>
        /// 根据验证码生成图片
        /// </summary>
        /// <param name="checkCode"></param>
        private void CreateCodeImg(string code, HttpContext context)
        {
            //定义图片
            Bitmap image = new Bitmap((int)Math.Ceiling(code.Length * 15.0), 30);
            Graphics graphics = Graphics.FromImage(image);
            //生成随机字体颜色
            try
            {
                Random r = new Random();
                graphics.Clear(Color.White);

                //绘制线条
                for (int i = 0; i < 5; i++)
                {
                    int x1 = r.Next(image.Width);
                    int x2 = r.Next(image.Width);
                    int y1 = r.Next(image.Height);
                    int y2 = r.Next(image.Height);
                    graphics.DrawLine(new Pen(Color.Silver), x1, x2, y1, y2);
                }

                //绘制文本
                Font font = new Font("Arial", 16, (FontStyle.Bold | FontStyle.Italic));
                for (int i = 0; i < code.Length; i++)
                {
                    LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0,
                        image.Width, image.Height),
                        Color.FromArgb(r.Next(255),
                        r.Next(255),
                        r.Next(255)),
                        Color.FromArgb(r.Next(255),
                        r.Next(255),
                        r.Next(255)),
                        0, true);
                    graphics.DrawString(code[i].ToString(), font, brush, 1 + i * 13, 2);
                }

                //绘制前景
                for (int i = 0; i < 30; i++)
                {
                    image.SetPixel(r.Next(image.Width),
                        r.Next(image.Height),
                        Color.FromArgb(r.Next(255),
                        r.Next(255),
                        r.Next(255)));
                }

                //将绘制的验证码图片保存为jpg，并写入到页面
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                context.Response.Clear();
                context.Response.ContentType = "Image/jpeg";
                context.Response.BinaryWrite(stream.ToArray());
            }
            catch (Exception)
            {
            }
            finally
            {
                image.Dispose();
                graphics.Dispose();
            }
        }

    }
}