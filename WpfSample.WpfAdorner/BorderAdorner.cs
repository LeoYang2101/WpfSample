using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfSample.WpfAdorner
{
    //1.创建一个自定义的Adorner类，继承Adorner抽象类
    public class BorderAdorner: Adorner
    {
        public BorderAdorner(UIElement adornerUIElement):base(adornerUIElement) { }

        //2.在实现的Adorner类中重写OnRender方法，以定义如何绘制额外的图形或内容
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            //创建一个红色边框
            Rect rect = new Rect(this.AdornedElement.DesiredSize);
            Pen pen = new Pen(Brushes.Red, 2);
            //绘制矩形框
            drawingContext.DrawRectangle(null, pen, rect);
        }
    }
}
