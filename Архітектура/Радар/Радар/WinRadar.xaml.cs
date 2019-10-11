using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Input;

namespace Радар
{
    public partial class WinRadar : Window
    {
        //Оголошуємо змінні
        Label angle, distance;
        List<Rectangle> lines = new List<Rectangle>();
        double ResolWidth, ResolHeight;
        //Конструктор класу
        public WinRadar()
        {
            InitializeComponent();
            ResolHeight = SystemParameters.PrimaryScreenHeight;
            ResolWidth = SystemParameters.PrimaryScreenWidth;

            for (int i = 0; i <= 180; i += 2)
                lines.Add(DrowLine(i));

            DrowRadar();
        }
        //Функція яка малює лінію з заданими параметрами
        Rectangle DrowLine(int angle, int width, int height,
            int right, int bottom, Brush brush)
        {
            Rectangle rectangle = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = brush,
                Opacity = 0.15,
            };

            Canvas.SetBottom(rectangle, bottom);
            Canvas.SetRight(rectangle, right);

            RotateTransform rotate = new RotateTransform(angle)
            {
                CenterX = rectangle.Width,
                CenterY = rectangle.Height / 2
            };
            rectangle.RenderTransform = rotate;
            OnScren(rectangle);

            return rectangle;
        }

        Rectangle DrowLine(int angle)
        {
            return DrowLine(angle, (int)AdaptResol(830, Sides.Width), 
                (int)AdaptResol(10, Sides.Height) , 
                (int)AdaptResol(960, Sides.Width), (int)AdaptResol(70, Sides.Height),
                Brushes.Green);
        }
        //Функція яка малює радар
        void DrowRadar()
        {
            for (int i = 0; i <= 180; i += 45)
            {
                Rectangle rect = DrowLine(i, (int)AdaptResol(900, Sides.Width), 2,
                    (int)AdaptResol(960, Sides.Width), (int)AdaptResol(70, Sides.Height), 
                    Brushes.Yellow);
                rect.Opacity = 1;
            }

            Path arc20, arc40, arc60, arc80, arc100;

            arc20 = CreatePath(new Point(0, (int)AdaptResol(300, Sides.Height)),
                new Point((int)AdaptResol(332, Sides.Width), (int)AdaptResol(300, Sides.Height)), 
                (int)AdaptResol(70, Sides.Height), (int)AdaptResol(794, Sides.Width));
            arc40 = CreatePath(new Point(0, (int)AdaptResol(300, Sides.Height)),
                new Point((int)AdaptResol(664, Sides.Width), (int)AdaptResol(300, Sides.Height)),
                (int)AdaptResol(70, Sides.Height), (int)AdaptResol(628, Sides.Width));
            arc60 = CreatePath(new Point(0, (int)AdaptResol(300, Sides.Height)),
                new Point((int)AdaptResol(996, Sides.Width), (int)AdaptResol(300, Sides.Height)),
                (int)AdaptResol(70, Sides.Height), (int)AdaptResol(462, Sides.Width));
            arc80 = CreatePath(new Point(0, (int)AdaptResol(300, Sides.Height)),
                new Point((int)AdaptResol(1328, Sides.Width), (int)AdaptResol(300, Sides.Height)),
                (int)AdaptResol(70, Sides.Height), (int)AdaptResol(296, Sides.Width));
            arc100 = CreatePath(new Point(0, (int)AdaptResol(300, Sides.Height)),
                new Point((int)AdaptResol(1660, Sides.Width), (int)AdaptResol(300, Sides.Height)),
                (int)AdaptResol(70, Sides.Height), (int)AdaptResol(130, Sides.Width));

            Label label1, label2, label3, label4, label5;

            label1 = CreateLabel("0°", (int)AdaptResol(45, Sides.Width), (int)AdaptResol(70, Sides.Height));
            label2 = CreateLabel("45°", (int)AdaptResol(325, Sides.Width), (int)AdaptResol(700, Sides.Height));
            label3 = CreateLabel("90°", (int)AdaptResol(930, Sides.Width), (int)AdaptResol(960, Sides.Height));
            label4 = CreateLabel("135°", (int)AdaptResol(1500, Sides.Width), (int)AdaptResol(700, Sides.Height));
            label5 = CreateLabel("180°", (int)AdaptResol(1800, Sides.Width), (int)AdaptResol(70, Sides.Height));

            angle = CreateLabel("Angle: ", (int)AdaptResol(320, Sides.Width), 2);
            distance = CreateLabel("Distance: ", (int)AdaptResol(1200, Sides.Width), 2);

            OnScren(arc20, arc40, arc60, arc80, arc100,
                label1, label2, label3, label4, label5, angle, distance);
        }
        //Перерахування яке ширину і висоту
        enum Sides
        {
            Width,
            Height
        }
        //Функція яка обчислює розміри об'єктів відносно розширення системи
        double AdaptResol(Double length, Sides sides)
        {
            if (sides == Sides.Height)
                return length / 1080 * ResolHeight;
            else return length / 1920 * ResolWidth;
        }
        //Функція яка встановлює значення напису Angle
        void SetAngle(int ang)
        {
            string temp = angle.Content.ToString();
            temp = temp.Split(':')[0];
            temp += ": " + (ang * 2);
            angle.Content = temp;
        }
        //Функція яка встановлює значення напису Distance
        void SetDistance(int dis)
        {
            string temp = distance.Content.ToString();
            temp = temp.Split(':')[0];
            if(dis <= 100)
                temp += ": " + dis;
            else temp += ": " + ">100";
            distance.Content = temp;
        }
        //Функція яка виводить всі UIElement на екран
        void OnScren(params UIElement[] element)
        {
            foreach(UIElement el in element)
                plate.Children.Add(el);
        }
        //Функція яка створює напис
        Label CreateLabel(string content, int left, int bottom)
        {
            Label label = new Label
            {
                Content = content,
                FontSize = (int)AdaptResol(48, Sides.Height),
                Foreground = Brushes.Green,
                FontWeight = FontWeights.Bold
            };

            Canvas.SetBottom(label, bottom);
            Canvas.SetLeft(label, left);
            return label;
        }
        //Обробник подій KeyDown для закриття вікна радару
        private void MyWinRadar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) this.Close();
        }
        // Функція для створення форми дуг
        Path CreatePath(Point point1, Point point2, int bottom, int left)
        {
            PathFigure figure = new PathFigure
            {
                StartPoint = point1
            };
            figure.Segments.Add(new ArcSegment(point2,
                new Size(1, 1), 180, false, SweepDirection.Clockwise, true));

            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(figure);

            Path path = new Path
            {
                Stroke = Brushes.Yellow,
                StrokeThickness = 2
            };
            path.Data = geometry;

            Canvas.SetBottom(path, bottom);
            Canvas.SetLeft(path, left);

            return path;
        }
        //Функція яка запускає анімацію вибраної лінії
        public void AnimLine(int num, int time, double from, double to, int distance)
        {
            DoubleAnimation myDoubleAnimation = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = new Duration(TimeSpan.FromMilliseconds(time))
            };

            double myDist = distance > 100 ? 100 : distance;

            LinearGradientBrush myVerticalGradient =
                new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0.5),
                    EndPoint = new Point(1, 0.5)
                };
            myVerticalGradient.GradientStops.Add(
                new GradientStop(Colors.Red, 0.0));
            myVerticalGradient.GradientStops.Add(
                new GradientStop(Colors.Red, (100 - myDist) / 100));
            myVerticalGradient.GradientStops.Add(
                new GradientStop(Colors.Green, (100 - myDist) / 100));
            myVerticalGradient.GradientStops.Add(
                new GradientStop(Colors.Green, 1.0));
            
                lines[num].BeginAnimation(Rectangle.OpacityProperty, myDoubleAnimation);
                lines[num].Fill = myVerticalGradient;

            SetAngle(num);
            SetDistance(distance);
        }
    }
}
