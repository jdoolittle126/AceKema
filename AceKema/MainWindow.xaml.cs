using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AceKema
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TransformGroup box_group = new TransformGroup();

        ScaleTransform scale = new ScaleTransform();
        TranslateTransform movement = new TranslateTransform();

        float canvas_scale = 1;
        int backdrop_size = 5;
        bool mouse_down = false;
        double lastX = 0, lastY = 0;

        public MainWindow()
        {
            box_group.Children.Add(scale);
            box_group.Children.Add(movement);

            InitializeComponent();
            Viewbox_Boxy.RenderTransform = box_group;
            
        }


        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            canvas_scale = (float) Math.Clamp(canvas_scale + ((float)e.Delta / 200), 1, 10);
            scale.ScaleX = canvas_scale;
            scale.ScaleY = canvas_scale;

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouse_down = true;
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouse_down = false;
            lastX = 0;
            lastY = 0;
        }

        private void Window_PreviewMouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            //Fix on scale
            if(mouse_down)
            {
                if(lastX == 0)
                {
                    lastX = e.GetPosition(Viewbox_Boxy).X;
                    lastY = e.GetPosition(Viewbox_Boxy).Y;
                }
                movement.X += e.GetPosition(Viewbox_Boxy).X - lastX;
                movement.Y += e.GetPosition(Viewbox_Boxy).Y - lastY;
                lastX = e.GetPosition(Viewbox_Boxy).X;
                lastY = e.GetPosition(Viewbox_Boxy).Y;

                //movement.X = Math.Clamp(movement.X, 0, (Panel_Flow.ActualWidth * scale.ScaleX) - Panel_Flow.ActualWidth);
                //movement.Y = Math.Clamp(movement.Y, -(Panel_Flow.ActualHeight * scale.ScaleY), (Panel_Flow.ActualHeight * scale.ScaleY));
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.R)
            {
                DrawCanvasBackdrop();
            }
        }

        private void DrawCanvasBackdrop()
        {
            Panel_Flow.Children.Clear();
            bool doneDrawingBackground = false;
            float nextX = 0, nextY = 0;
            int rowCounter = 0;
            bool nextIsOdd = false;

            while (doneDrawingBackground == false)
            {
                Rectangle rect = new Rectangle
                {
                    Width = backdrop_size,
                    Height = backdrop_size,
                    Fill = nextIsOdd ? Brushes.LightGray : Brushes.WhiteSmoke
                };
                Panel_Flow.Children.Add(rect);
                Canvas.SetTop(rect, nextY);
                Canvas.SetLeft(rect, nextX);

                nextIsOdd = !nextIsOdd;
                nextX += backdrop_size;
                if (nextX >= Panel_Flow.ActualWidth)
                {
                    nextX = 0;
                    nextY += backdrop_size;
                    rowCounter++;
                    nextIsOdd = (rowCounter % 2 != 0);
                }

                if (nextY >= Panel_Flow.ActualHeight)
                    doneDrawingBackground = true;
            }
        }


    }
}
