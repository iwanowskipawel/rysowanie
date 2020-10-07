using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace rysowanie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double lastX, lastY;
        bool IsNowDrawing;
        DrawingMode drawingMode = DrawingMode.Line;
        public static Random rand = new Random();

        public MainWindow()
        {
            this.MouseLeftButtonDown += new MouseButtonEventHandler(OnMouseDown);
            this.MouseWheel += new MouseWheelEventHandler(MainWindow_OnMouseWheel);
           // CompositionTarget.Rendering += RenderTransformEllipse;
            //this.MouseUp += new MouseButtonEventHandler(OnMouseUp);
            //this.MouseMove += new MouseEventHandler(OnMouseMove);
            IsNowDrawing = false;
                        

            InitializeComponent();
            DrawSlab();

            ZoomViewbox.Width = 300;
            ZoomViewbox.Height = 300;
        }

        public void RenderTransformEllipse(object sender, System.EventArgs e)
        {
            double newLeft = rand.Next(0, 300);
            double newTop = rand.Next(0, 300);

            theEllipse.SetValue(Canvas.LeftProperty, newLeft);
            theEllipse.SetValue(Canvas.TopProperty, newTop);
        }

        private void DrawSlab()
        {
            int[][] points = {
                new int[] { 10,10 },
                new int[] { 100,100 },
                new int[] { 250,110 },
                new int[] { 200, 10}
            };

            Point[] points2 = {
                new Point(10,10),
                new Point( 100,100 ),
                new Point( 250,110 ),
                new Point( 200, 10)
            };
            PointCollection pc = new PointCollection(points2);

            Polygon pl = new Polygon();
            pl.OpacityMask = Brushes.Red;
            pl.Stroke = Brushes.Red;
            pl.Points = pc;
            myGrid.Children.Add(pl);
            //for (int i=0; i<points.Length-1; i++) 
            //    DrawLine(points[i][0], points[i + 1][0],
            //        points[i][1], points[i + 1][1]);

            //DrawLine(points[points.Length - 1][0], points[0][0], points[points.Length - 1][1], points[0][1]);
            
        }

        private void DrawLine(int x1, int x2, int y1, int y2)
        {
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.Red;

            myLine.X1 = x1;
            myLine.X2 = x2;
            myLine.Y1 = y1;
            myLine.Y2 = y2;

            myGrid.Children.Add(myLine);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            myGrid.Children.Clear();
        }

        private void xButton_Click(object sender, RoutedEventArgs e)
        {
            drawingMode = DrawingMode.Cross;
            ClearLineMouseEvents();
        }

        private void oButton_Click(object sender, RoutedEventArgs e)
        {
            drawingMode = DrawingMode.Circle;
            ClearLineMouseEvents();
        }

        private void lineButton_Click(object sender, RoutedEventArgs e)
        {
            drawingMode = DrawingMode.Line;
            AddLineMouseEvents();
        }

        private void ClearLineMouseEvents()
        {
            this.MouseLeftButtonUp -= new MouseButtonEventHandler(OnMouseUpLine);
            this.MouseMove -= new MouseEventHandler(OnMouseMoveLine);
        }
        private void AddLineMouseEvents()
        {
            this.MouseLeftButtonUp += new MouseButtonEventHandler(OnMouseUpLine);
            this.MouseMove += new MouseEventHandler(OnMouseMoveLine);
        }

        public void OnMouseMoveLine(object sender, MouseEventArgs e)
        {
            this.Title = $"Mouse Position: X = {e.GetPosition(this).X}, Y = {e.GetPosition(this).Y}";
            

            if (IsNowDrawing && myGrid.Children.Count > 0)
            {
                ((Line)myGrid.Children[myGrid.Children.Count - 1]).X2 = e.GetPosition(myGrid).X;
                ((Line)myGrid.Children[myGrid.Children.Count - 1]).Y2 = e.GetPosition(myGrid).Y;
            }

        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            
            switch (drawingMode) {
                case DrawingMode.Line:
                    if (!IsNowDrawing)
                    {
                        lastX = e.GetPosition(myGrid).X;
                        lastY = e.GetPosition(myGrid).Y;
                    }
                    break;
                case DrawingMode.Circle:
                    //Shape.Ellipse myEllipse = new Ellipse();
                    //myEllipse.Stroke = System.Windows.Media.Brushes.Black;
                    ////myEllipse.HorizontalAlignment = HorizontalAlignment.Left;
                    ////myEllipse.VerticalAlignment = VerticalAlignment.Center;
                    //myEllipse.Width = 50;
                    //myEllipse.Height = 50;
                    //myGrid.Children.Add(myEllipse);
                    break;
            }
        }

        private void OnMouseUpLine(object sender, MouseEventArgs e)
        {
            if (!IsNowDrawing)
            {
                Line myLine = new Line();
                myLine.Stroke = System.Windows.Media.Brushes.Red;

                myLine.X1 = lastX;
                myLine.X2 = e.GetPosition(myGrid).X;
                myLine.Y1 = lastY;
                myLine.Y2 = e.GetPosition(myGrid).Y;

                myGrid.Children.Add(myLine);

                lastX = e.GetPosition(myGrid).X;
                lastY = e.GetPosition(myGrid).Y;
            }
            if (IsNowDrawing)
            {
                IsNowDrawing = false;
            }
            else
            {
                IsNowDrawing = true;
            }
                
        }

        private void MainWindow_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {

            UpdateViewBox((e.Delta > 0) ? 15 : -15);
        }

        private void UpdateViewBox(int newValue)
        {
            if ((ZoomViewbox.Width >= 0) && ZoomViewbox.Height >= 0)
            {
                ZoomViewbox.Width += newValue;
                ZoomViewbox.Height += newValue;
            }
        }
    }

    public enum DrawingMode
    {
        Line,
        Circle,
        Cross
    }
}
