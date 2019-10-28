namespace ImagenarioCS
{
    public class Terrain
    {
        public int sizex;
        public int sizey;
        public char[,] tiles;
        public char[,] hills;

        void initializeTiles()
        {
            tiles = new char[sizex, sizey];
            hills = new char[sizex, sizey];
        }
    }
}