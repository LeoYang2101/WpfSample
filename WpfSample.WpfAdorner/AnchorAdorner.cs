using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfSample.WpfAdorner
{
    /// <summary>
    /// 锚点装饰器
    /// </summary>
    public class AnchorAdorner : Adorner
    {
        public AnchorAdorner(UIElement adornedElement) : base(adornedElement)
        {

        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var size = this.AdornedElement.DesiredSize;//获取需要装饰的UI元素的真实Size
            Rect rect = new Rect(size);//定义一个矩形，从0,0开始，大小为size
            var pen = new Pen(Brushes.Black, 2);
            pen.DashStyle = DashStyles.Solid;
            //drawingContext.DrawRectangle(Brushes.Transparent, pen, rect);//绘制矩形，第1个参数是填充色，第2个参数是边框，第3个参数是矩形大小，位置

            drawingContext.DrawEllipse(Brushes.WhiteSmoke, pen, new Point(0, size.Height / 2), 3, 3);//绘制锚点，左中
            drawingContext.DrawEllipse(Brushes.WhiteSmoke, pen, new Point(size.Width, size.Height / 2), 3, 3);//绘制锚点，右中
            drawingContext.DrawEllipse(Brushes.WhiteSmoke, pen, new Point(size.Width / 2, 0), 3, 3);//绘制锚点，上中
            drawingContext.DrawEllipse(Brushes.WhiteSmoke, pen, new Point(size.Width / 2, size.Height), 3, 3);//绘制锚点，下中
        }
    }
}
