using System;
using FifthLaba.Objects;

namespace FifthLaba
{
    //Сделать приложение по методичке обработка событий
    //И допилить его.Сделать в обязательном порядке оба зеленых задания и на выбор одно или несколько из 3-6 заданий.
    //1)	Реализовать новый объект, который будет исчезать при пересечении с игроком и появляться на новом месте
    //2)	Реализовать вывод очков.Увеличивать количество очков при пересечении с объектом добавленным в предыдущем пункте.
    //Дополнительно добавить на поле несколько зеленых кругов.
    //3)	Постепенно уменьшать зеленый кружок.Если размер кружка становится нулевым, то перемещать на новую позицию
    //и задавать ему новый начальный размер.Размер хранить в зеленом кружке, событие уменьшения до нуля должен
    //генерировать зеленый кружок

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

            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;

                if (obj is GreenCircle)
                {
                    objects.Remove(obj);
                    score += 1;
                    label1.Text = "Очки: " + score;
                    SpawnGreenCircle();
                }
            };

            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };

            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);

            // Инициализация первых зеленых кругов
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
                    if (circle.Disappear())
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

            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);

                    //obj.Overlap(player);
                }
            }

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

                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }

            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            player.X += player.vX;
            player.Y += player.vY;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            updateCircle();

            pbMain.Invalidate();
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker);
            }

            marker.X = e.X;
            marker.Y = e.Y;
        }
    }
}
