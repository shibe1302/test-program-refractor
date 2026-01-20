using System.Drawing;

namespace CPEI_MFG
{
    public partial class ImageCompare
    {
        public string image1;
        public string image2;

        public ImageCompare(string file1,string file2)
        {
            image1 = file1;
            image2 = file2;
        }

        public float CompareBitmap()
        {
            int diffPixel = 0;
            Bitmap map1 = new Bitmap(image1);
            Bitmap map2 = new Bitmap(image2);
            Bitmap container = new Bitmap(map1.Width, map1.Height);
            
            for (int i = 0; i < map1.Width; i++ )
            {
                for (int j = 0; j < map1.Height;j++ )
                {
                    Color firstColor = map1.GetPixel(i, j);
                    Color secColor = map2.GetPixel(i, j);

                    if (firstColor != secColor)
                    {
                        diffPixel++;
                        container.SetPixel(i, j, Color.Red);
                    }
                    else
                        container.SetPixel(i, j, firstColor);
                }
            }
            int totalPixel = map1.Width * map2.Height;
            float fDiff = (float)((float)diffPixel / (float)totalPixel);
            float precentage = fDiff * 100;
            return precentage;
        }
    }
}
