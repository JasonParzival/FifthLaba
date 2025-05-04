using FifthLaba.Objects;

namespace FifthLaba
{
    public partial class Form1 : Form
    {
        //MyRectangle myRect; // создадим поле под наш прямоугольник
        List<BaseObject> objects = new();
        Player player;
        Marker marker;

        public Form1()
        {
            InitializeComponent();

            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);
            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);


            objects.Add(marker);
            objects.Add(player);
            //myRect = new MyRectangle(100, 100, 45); // создать экземпляр класса
            objects.Add(new MyRectangle(50, 50, 0));
            objects.Add(new MyRectangle(100, 100, 45));
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.Clear(Color.White);

            foreach (var obj in objects)
            {
                // проверяю было ли пересечение с игроком
                if (obj != player && player.Overlaps(obj, g))
                {
                    // и если было вывожу информацию на форму
                    txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
                }

                g.Transform = obj.GetTransform();

                obj.Render(g);
            }

            //g.Transform = myRect.GetTransform(); // устанавливаем новую матрицу

            //myRect.Render(g); // рендерим как обычно
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // расчитываем вектор между игроком и маркером
            float dx = marker.X - player.X;
            float dy = marker.Y - player.Y;

            // находим его длину
            float length = MathF.Sqrt(dx * dx + dy * dy);
            dx /= length; // нормализуем координаты
            dy /= length;

            // пересчитываем координаты игрока
            player.X += dx * 2;
            player.Y += dy * 2;

            // запрашиваем обновление pbMain
            // это вызовет метод pbMain_Paint по новой
            pbMain.Invalidate();
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            marker.X = e.X;
            marker.Y = e.Y;
        }
    }
}
