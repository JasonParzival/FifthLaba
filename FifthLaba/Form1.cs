using System;
using FifthLaba.Objects;

namespace FifthLaba
{
    public partial class Form1 : Form
    {
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        GreenCircle greenCircle;
        Random random = new Random();
        int score = 0;

        public Form1()
        {
            InitializeComponent();

            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);

            // добавляю реакцию на пересечение
            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;

                if (obj is GreenCircle circle)
                {
                    objects.Remove(obj);
                    score += 1;
                    label1.Text = "Очки: " + score;
                    SpawnGreenCircle();
                }
            };

            // добавил реакцию на пересечение с маркером
            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };

            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);

            // Инициализация первого зеленого круга
            SpawnGreenCircle();
            SpawnGreenCircle();
            SpawnGreenCircle();

            objects.Add(marker);
            objects.Add(player);
        }

        // Метод для создания зеленого круга в случайном месте
        private void SpawnGreenCircle()
        {
            greenCircle = new GreenCircle(
                random.Next(20, pbMain.Width - 20),
                random.Next(20, pbMain.Height - 20),
                0);

            objects.Add(greenCircle);
        }

        private void updateCircle()
        {
            foreach (var obj in objects.ToList())
            {
                if (obj is GreenCircle circle)
                {
                    circle.Update();
                    if (circle.disappear)
                    {
                        objects.Remove(circle);
                        SpawnGreenCircle();
                    }
                }
            }
        } 

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.Clear(Color.White);

            updatePlayer();

            // пересчитываем пересечения
            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                }
            }

            // рендерим объекты
            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }

        private void updatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;
                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                // по сути мы теперь используем вектор dx, dy
                // как вектор ускорения, точнее даже вектор притяжения
                // который притягивает игрока к маркеру
                // 0.5 просто коэффициент который подобрал на глаз
                // и который дает естественное ощущение движения
                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                // расчитываем угол поворота игрока 
                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }

            // тормозящий момент,
            // нужен чтобы, когда игрок достигнет маркера произошло постепенное замедление
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            // пересчет позиция игрока с помощью вектора скорости
            player.X += player.vX;
            player.Y += player.vY;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //updatePlayer();
            updateCircle();

            pbMain.Invalidate();
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            // тут добавил создание маркера по клику если он еще не создан
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker); // и главное не забыть пололжить в objects
            }

            // а это так и остается
            marker.X = e.X;
            marker.Y = e.Y;
        }
    }
}
