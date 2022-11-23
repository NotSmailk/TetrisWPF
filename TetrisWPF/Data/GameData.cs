using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TetrisWPF
{
    public class GameData
    {
        public static readonly ImageSource[] TileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Sprites/Tiles/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/Tiles/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/Tiles/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/Tiles/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/Tiles/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/Tiles/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/Tiles/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/Tiles/TileRed.png", UriKind.Relative))
        };
        public static readonly ImageSource[] BlockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Sprites/BlockImages/BLock-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/BlockImages/BLock-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/BlockImages/BLock-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/BlockImages/BLock-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/BlockImages/BLock-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/BlockImages/BLock-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/BlockImages/BLock-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/BlockImages/BLock-Z.png", UriKind.Relative))
        };

        public static readonly int MaxDelay = 1000;
        public static readonly int MinDelay = 75;
        public static readonly int DelayDecrease = 25;
    }
}
