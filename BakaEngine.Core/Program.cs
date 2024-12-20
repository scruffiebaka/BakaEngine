namespace BakaEngine.Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(800, 600, "BakaEngine");
            game.Run();
        }
    }
}
