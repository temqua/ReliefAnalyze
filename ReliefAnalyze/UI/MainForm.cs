﻿using System;
using System.Numerics;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using ReliefAnalyze.UI;
using Accord.Imaging;
using Image = System.Drawing.Image;
using Accord.Imaging.Filters;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using ReliefAnalyze.Logic.ColorDetect;
using System.Reflection;
using System.Drawing.Imaging;
using ReliefAnalyze.Logic;
using MaterialSkin.Controls;
using MaterialSkin;

namespace ReliefAnalyze
{
    public partial class MainForm : MaterialForm
    {
        private System.Drawing.Point coordinatesAnalyze { get; set; }
        private int RectSide { get; set; }

        private int PointSide { get; set; }

        private MapObjects mapObjects;
        private string ProjectDir { get; set; }
        public MainForm()
        {
            InitializeComponent();
        }

        private class MyRenderer : ToolStripProfessionalRenderer
        {
            public MyRenderer() : base(new MyColors()) { }
        }

        /// <summary>
        /// Изменение стандартных цветов компонентов формы windows
        /// </summary>
        private class MyColors : ProfessionalColorTable
        {
            private MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;

            public override Color MenuItemSelectedGradientBegin
            {
                get
                {
                    return materialSkinManager.ColorScheme.LightPrimaryColor;
                }
            }
            public override Color MenuItemSelectedGradientEnd
            {

                get
                {
                    return materialSkinManager.ColorScheme.LightPrimaryColor;
                }
            }

            public override Color MenuItemBorder
            {
                get
                {
                    return materialSkinManager.ColorScheme.DarkPrimaryColor;
                }
            }

            public override Color MenuItemPressedGradientBegin
            {
                get { return materialSkinManager.ColorScheme.LightPrimaryColor; }
            }

            public override Color MenuItemPressedGradientEnd
            {
                get { return materialSkinManager.ColorScheme.DarkPrimaryColor; }
            }
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OpenFileMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var myImageFile = ImageFile.GetInstance();
                myImageFile.FileInfo = new FileInfo(openFileDialog1.FileName);
                myImageFile.Image = Image.FromFile(openFileDialog1.FileName);
                mainPictureBox.Image = myImageFile.Image;
            }
        }

        private void AnalyzeButton_Click(object sender, EventArgs e)
        {
            if (mainPictureBox.Image != null)
            {
                Analyze();
            }
            else
            {
                MessageBox.Show("Вы не открыли изображение!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Bitmap HoughTransform(Bitmap originalImage)
        {
            var imgBitmap = new Bitmap(originalImage);
            var lineTransform = new Accord.Imaging.HoughLineTransformation();
            // Convert it to binary and mark the possible lines 
            // in white so it can be processed by the transform
            var sequence = new FiltersSequence(
                Grayscale.CommonAlgorithms.BT709,
                new NiblackThreshold(),
                new Invert()
            );
            var contoursBitmap = ImageFilter.PrewittFilter(imgBitmap, true);
            // Apply the sequence of filters above:
            Bitmap binaryImage = sequence.Apply(contoursBitmap);
            lineTransform.ProcessImage(binaryImage);
            // Now, let's say we would like to retrieve the lines and use them
            // for further processing. First, the lines can be ordered by their
            // relative intensity using
            HoughLine[] lines = lineTransform.GetLinesByRelativeIntensity(1);

            // Then, let's plot them on top of the input image. Since we will
            // apply many operations to a single image, it is better to first
            // convert it to an UnmanagedImage object to avoid having to lock
            // the image into memory multiple times.

            UnmanagedImage unmanagedImage = UnmanagedImage.FromManagedImage(binaryImage);

            // Finally, plot them in order:
            foreach (HoughLine line in lines)
            {
                line.Draw(unmanagedImage, color: Color.Red);
            }

            return unmanagedImage.ToManagedImage();
        }

        private void Hough()
        {
            var houghBitmap = HoughTransform(new Bitmap(ImageFile.GetInstance().Image));
            ContoursForm contoursForm = new ContoursForm();
            contoursForm.Image = houghBitmap;
            contoursForm.Show();
        }

        private Bitmap CannyFilter(Bitmap originalBitmap)
        {
            var originalMat = BitmapConverter.ToMat(originalBitmap);
            var cannyMat = new Mat();
            Cv2.Canny(originalMat, cannyMat, 50, 100);
            return BitmapConverter.ToBitmap(cannyMat);
        }

        private void FragmentContours()
        {
            var imageBitmap = new Bitmap(ImageFile.GetInstance().Image);
            if (!coordinatesAnalyze.IsEmpty)
            {
                var contoursBitmap = FragmentBitmap(imageBitmap);

                contoursBitmap = ImageFilter.PrewittFilter(contoursBitmap, false);
                FilteredImageForm filteredImageForm = new FilteredImageForm();
                filteredImageForm.Image = contoursBitmap;
                filteredImageForm.Show();
            }
            else
            {
                MessageBox.Show("Выберите участок карты!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Contours()
        {
            var originalBitmap = new Bitmap(ImageFile.GetInstance().Image);
            var contoursBitmap = ImageFilter.PrewittFilter(originalBitmap, false);
            ContoursForm contoursForm = new ContoursForm();
            contoursForm.Image = contoursBitmap;
            contoursForm.Show();
        }

        private void Analyze()
        {
            var imageBitmap = new Bitmap(ImageFile.GetInstance().Image);
            var colorDictionary = new Dictionary<string, ColorInfo>();
            var Width = ImageFile.GetInstance().Image.Width;
            var Height = ImageFile.GetInstance().Image.Height;
            if (!coordinatesAnalyze.IsEmpty)
            {
                var xstart = coordinatesAnalyze.X - RectSide / 2;
                var ystart = coordinatesAnalyze.Y - RectSide / 2;
                var xend = coordinatesAnalyze.X + RectSide / 2;
                var yend = coordinatesAnalyze.Y + RectSide / 2;
                var xmin = xstart > 0 ? xstart : 0;
                var ymin = ystart > 0 ? ystart : 0;
                var xmax = xend < Width ? xend : Width;
                var ymax = yend < Height ? yend : Height;


                for (int i = xmin; i < xmax; i++)
                {
                    for (int j = ymin; j < ymax; j++)
                    {
                        if (!(i == 0 || j == 0 || i == Width - 1 || j == Height - 1))
                        {
                            var medium = imageBitmap.GetPixel(i - 1, j - 1);
                            var colorValue = medium.ToArgb().ToString();
                            var near = ColorHelper.GetNearestColorName(ColorHelper.GetSystemDrawingColorFromHexString("#" + medium.Name.Substring(2)));

                            if (!colorDictionary.ContainsKey(colorValue))
                            {
                                colorDictionary.Add(colorValue, new ColorInfo { Color = medium, ColorCount = 1, NearColor = near });
                            }
                            else
                            {
                                colorDictionary[colorValue].ColorCount += 1;
                            }

                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали фрагмент!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            var allColors = colorDictionary.OrderByDescending(elem => elem.Value.ColorCount).ToList();
            var mainColors = allColors.Take(20).ToList();



            using (StreamWriter writer = new StreamWriter($@"{ProjectDir}\imageinfo.txt"))
            {
                foreach (KeyValuePair<string, ColorInfo> keyValue in colorDictionary.OrderByDescending(elem => elem.Value.ColorCount))
                {
                    Color col = Color.FromArgb(Convert.ToInt32(keyValue.Key));
                    var near = ColorHelper.GetNearestColorName(ColorHelper.GetSystemDrawingColorFromHexString("#" + col.Name.Substring(2)));

                    writer.WriteLine($"{keyValue.Key} : {keyValue.Value.ColorCount} , near color = {keyValue.Value.NearColor}");
                    writer.WriteLine(near);
                }
            }

            MessageBox.Show("Анализ проведён!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            AnalyzeForm analyzeForm = new AnalyzeForm();
            //analyzeForm.SetDataGridView(mainColors);

            analyzeForm.Show();
        }

        private string AnalyzeColors(List<KeyValuePair<string, ColorInfo>> allColors)
        {
            StringBuilder analyze = new StringBuilder("");
            var midBlue = allColors.Take(30).Where(clr => clr.Value.NearColor == Color.MidnightBlue.ToString());

            if (midBlue != null)
            {
                analyze.Append("Есть река");
            }
            //switch (mainColors.First().Value.NearColor)
            //{

            //}
            return analyze.ToString();
        }

        private void HistogramMenuItem_Click(object sender, EventArgs e)
        {
            if (this.mainPictureBox.Image != null)
            {
                HistogramForm form = new HistogramForm();
                form.MyImage = new Bitmap(this.mainPictureBox.Image);
                form.Show();
            }
            else
            {
                MessageBox.Show("Вы не открыли изображение!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void MainPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.mainPictureBox.Image != null)
            {
                var coordinates = e.Location;
                coordinatesAnalyze = coordinates;
                var imageBitmap = new Bitmap(ImageFile.GetInstance().Image);
                var rectSide = imageBitmap.Height > imageBitmap.Width ? Convert.ToInt32(imageBitmap.Width * 0.2) : Convert.ToInt32((int)imageBitmap.Height * 0.2);

                var gr = Graphics.FromImage(imageBitmap);
                var rect = new Rectangle(e.Location.X - rectSide / 2, e.Location.Y - rectSide / 2, rectSide, rectSide);
                var point = new Rectangle(e.Location.X - 5, e.Location.Y - 5, 10, 10);

                gr.DrawRectangle(new Pen(Color.Gray, 3), rect);
                gr.DrawEllipse(new Pen(Color.Gray, 3), point);
                gr.Save();
                mainPictureBox.Image = imageBitmap;
                RectSide = rectSide;
                PointSide = 10;
            }
            else
            {
                MessageBox.Show("Вы не открыли изображение!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Material_Load()
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
            MainMenu.BackColor = materialSkinManager.ColorScheme.DarkPrimaryColor;
            MainMenu.ForeColor = materialSkinManager.ColorScheme.TextColor;
            var ffamily = materialSkinManager.ROBOTO_REGULAR_11.FontFamily;
            var menuFont = new Font(ffamily, 9);
            MainMenu.Font = menuFont;
            MainMenu.Renderer = new MyRenderer();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Material_Load();
            detectSizeRadio.Select();
            ProjectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            mapObjects = new MapObjects();
            if (File.Exists($@"{ProjectDir}\map.png"))
            {
                var imageFile = ImageFile.GetInstance();
                imageFile.Image = Image.FromFile($@"{ProjectDir}\map.png");
                imageFile.FileInfo = new FileInfo($@"{ProjectDir}\map.png");
                scaleBox.SelectedIndex = 1;
                mainPictureBox.Image = imageFile.Image;
            }

            var mapColors = MapObjectsColors.GetInstance();
            mapColors.SetDefaultColors();
        }

        private void ContoursButton_Click(object sender, EventArgs e)
        {
            Contours();
        }

        private void FragmentControursButton_Click(object sender, EventArgs e)
        {
            FragmentContours();

        }

        private void HoughButton_Click(object sender, EventArgs e)
        {
            Hough();
        }

        private void Opencvbutton_Click(object sender, EventArgs e)
        {
            FindContours();
        }

        private void FindContours()
        {
            var imageFile = ImageFile.GetInstance();
            var originalBitmap = new Bitmap(imageFile.Image);
            //var houghBitmap = HoughTransform(originalBitmap);
            //var invertedHoughBitmap = InvertImage(houghBitmap);
            var originalMat = FindContoursAndDraw(originalBitmap, "Ponds");
            var contoursBitmap = BitmapConverter.ToBitmap(originalMat);
            ContoursForm contoursForm = new ContoursForm();
            contoursForm.Image = contoursBitmap;
            contoursForm.Show();
        }

        private Bitmap FragmentBitmap(Bitmap imageBitmap)
        {
            var fragmentBitmap = new Bitmap(RectSide, RectSide);
            var xstart = coordinatesAnalyze.X - RectSide / 2;
            var ystart = coordinatesAnalyze.Y - RectSide / 2;
            var xend = coordinatesAnalyze.X + RectSide / 2;
            var yend = coordinatesAnalyze.Y + RectSide / 2;
            var xmin = xstart > 0 ? xstart : 0;
            var ymin = ystart > 0 ? ystart : 0;
            var xmax = xend < Width ? xend : Width;
            var ymax = yend < Height ? yend : Height;
            var newWidth = xmax - xmin;
            var newHeight = ymax - ymin;
            for (int i = 0; i < newWidth; i++)
            {
                for (int j = 0; j < newHeight; j++)
                {
                    if (!(i == 0 || j == 0 || i == newWidth - 1 || j == newHeight - 1))
                    {
                        var medium = imageBitmap.GetPixel(i + xmin - 1, j + ymin - 1);
                        fragmentBitmap.SetPixel(i, j, medium);
                    }
                }
            }
            return fragmentBitmap;
        }

        private Bitmap InvertImage(Bitmap originalMap)
        {
            var invertedMap = new Bitmap(originalMap);
            for (int y = 0; (y <= (originalMap.Height - 1)); y++)
            {
                for (int x = 0; (x <= (originalMap.Width - 1)); x++)
                {
                    Color inv = originalMap.GetPixel(x, y);
                    inv = Color.FromArgb(255, (255 - inv.R), (255 - inv.G), (255 - inv.B));
                    invertedMap.SetPixel(x, y, inv);
                }
            }
            return invertedMap;
        }

        private Bitmap FindColor(Bitmap originalMap, List<string> nearColor, bool saveContour = false)
        {
            var coloredMap = new Bitmap(originalMap);
            var colorDictionary = new Dictionary<string, ColorInfo>();
            var width = coloredMap.Width;
            var height = coloredMap.Height;
            var fillColor = Color.Black;
            if (nearColor.Contains(fillColor.Name))
            {
                fillColor = Color.AliceBlue;
            }
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //if (!(i == 0 || j == 0 || i == width - 1 || j == height - 1))
                    //{
                        var medium = originalMap.GetPixel(i, j);
                        if (medium.ToArgb() != 0)
                        {
                            var colorValue = medium.ToArgb().ToString();
                            var near = ColorHelper.GetNearestColorName(ColorHelper.GetSystemDrawingColorFromHexString("#" + medium.Name.Substring(2)));
                            if (!nearColor.Contains(near))
                            {
                                coloredMap.SetPixel(i, j, fillColor);
                            }
                        }
                    //}
                }
            }
            return coloredMap;
        }


        private Mat FindContoursAndDraw(Bitmap originalMap, string objectName, int minArea = 500, int maxArea = 10000)
        {
            //var houghBitmap = HoughTransform(originalMap);
            //var invertedHoughBitmap = InvertImage(houghBitmap);
            Mat originalMat = BitmapConverter.ToMat(originalMap);
            //Mat invertedHoughMat = BitmapConverter.ToMat(invertedHoughBitmap);
            Mat blackWhiteMat = new Mat();
            Mat edgesMat = new Mat();

            Cv2.CvtColor(originalMat, blackWhiteMat, ColorConversionCodes.BGRA2GRAY);
            if (MapObjectsColors.GetInstance().Tight.Contains(objectName))
            {
                Bitmap edgesMap = BitmapConverter.ToBitmap(blackWhiteMat);
                edgesMap = ImageFilter.SobelFilter(edgesMap, grayscale: true);
                edgesMat = BitmapConverter.ToMat(edgesMap);
                Cv2.CvtColor(edgesMat, edgesMat, ColorConversionCodes.BGRA2GRAY);
            }
            else
            {
                Cv2.Canny(blackWhiteMat, edgesMat, 50, 100);
            }


            OpenCvSharp.Point[][] contours;
            HierarchyIndex[] hierarchyIndexes;
            Cv2.FindContours(
                edgesMat,
                out contours,
                out hierarchyIndexes,
                mode: RetrievalModes.CComp,
                method: ContourApproximationModes.ApproxSimple);



            var componentCount = 0;
            var contourIndex = 0;
            var objectDict = mapObjects.getObjectDictionary();
            if (contours.Length != 0)
            {
                if (objectDict.ContainsKey(objectName))
                {
                    objectDict[objectName] = contours;
                }
                else
                {
                    objectDict.Add(objectName, contours);
                }
                while ((contourIndex >= 0))
                {
                    var contour = contours[contourIndex];
                    var boundingRect = Cv2.BoundingRect(contour);
                    var boundingRectArea = boundingRect.Width * boundingRect.Height;
                    var ca = Cv2.ContourArea(contour) * Convert.ToDouble(scaleBox.SelectedItem) / 100;
                    var cal = Cv2.ArcLength(contour, closed: true) * Convert.ToDouble(scaleBox.SelectedItem) / 100;

                    //if (boundingRectArea > minArea)
                    //{

                        Cv2.PutText(originalMat, $"A:{ca.ToString("#.##")} km2", new OpenCvSharp.Point(boundingRect.X, boundingRect.Y + 10), HersheyFonts.HersheyPlain, 1, Scalar.White, 1);
                        Cv2.PutText(originalMat, $"L:{cal.ToString("#.##")} km", new OpenCvSharp.Point(boundingRect.X, boundingRect.Y + 25), HersheyFonts.HersheyPlain, 1, Scalar.White, 1);


                    //}


                    //Cv2.DrawContours(
                    //    originalMat,
                    //    contours,
                    //    contourIndex,
                    //    color: Scalar.All(componentCount + 1),
                    //    thickness: -1,
                    //    lineType: LineTypes.Link8,
                    //    hierarchy: hierarchyIndexes,
                    //    maxLevel: int.MaxValue);

                    componentCount++;


                    contourIndex = hierarchyIndexes[contourIndex].Next;
                }
            }

            return originalMat;
        }

        private void FragmentFindContours()
        {
            var imageFile = ImageFile.GetInstance();
            var imageBitmap = new Bitmap(imageFile.Image);
            if (!coordinatesAnalyze.IsEmpty)
            {
                var fragmentBitmap = FragmentBitmap(imageBitmap);
                var contoursMat = FindContoursAndDraw(fragmentBitmap, "Ponds");
                var contoursBitmap = BitmapConverter.ToBitmap(contoursMat);
                FilteredImageForm filteredImageForm = new FilteredImageForm();
                filteredImageForm.Image = contoursBitmap;
                filteredImageForm.Show();
            }
            else
            {
                MessageBox.Show("Выберите участок карты!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void DetectColors_Click(object sender, EventArgs e)
        {
            var imageFile = ImageFile.GetInstance();
            var imageBitmap = new Bitmap(imageFile.Image);
            var contoursBitmap = new Bitmap(imageBitmap);
            if (!coordinatesAnalyze.IsEmpty)
            {
                var xstart = coordinatesAnalyze.X - RectSide / 2;
                var ystart = coordinatesAnalyze.Y - RectSide / 2;
                var xend = coordinatesAnalyze.X + RectSide / 2;
                var yend = coordinatesAnalyze.Y + RectSide / 2;
                var xmin = xstart > 0 ? xstart : 0;
                var ymin = ystart > 0 ? ystart : 0;
                var xmax = xend < Width ? xend : Width;
                var ymax = yend < Height ? yend : Height;
                var newWidth = xmax - xmin;
                var newHeight = ymax - ymin;
                for (int i = 0; i < newWidth; i++)
                {
                    for (int j = 0; j < newHeight; j++)
                    {
                        if (!(i == 0 || j == 0 || i == newWidth - 1 || j == newHeight - 1))
                        {
                            var medium = imageBitmap.GetPixel(i + xmin - 1, j + ymin - 1);
                            contoursBitmap.SetPixel(i, j, medium);
                        }
                    }
                }


                contoursBitmap = ImageFilter.PrewittFilter(contoursBitmap, false);
                var colorDictionary = new Dictionary<string, ColorInfo>();

                for (int i = 0; i < newWidth; i++)
                {
                    for (int j = 0; j < newHeight; j++)
                    {
                        if (!(i == 0 || j == 0 || i == newWidth - 1 || j == newHeight - 1))
                        {
                            var medium = contoursBitmap.GetPixel(i - 1, j - 1);
                            if (medium.ToArgb() != 0)
                            {
                                var colorValue = medium.ToArgb().ToString();

                                var near = ColorHelper.GetNearestColorName(ColorHelper.GetSystemDrawingColorFromHexString("#" + medium.Name.Substring(2)));
                                if (near != "Black")
                                {

                                    if (!colorDictionary.ContainsKey(colorValue))
                                    {
                                        colorDictionary.Add(colorValue, new ColorInfo { Color = medium, ColorCount = 1, NearColor = near });
                                    }
                                    else
                                    {
                                        colorDictionary[colorValue].ColorCount += 1;
                                    }
                                }
                            }
                        }
                    }
                }

                using (StreamWriter writer = new StreamWriter($@"{ProjectDir}\fragmentColors.txt"))
                {
                    foreach (KeyValuePair<string, ColorInfo> keyValue in colorDictionary.OrderByDescending(elem => elem.Value.ColorCount))
                    {
                        Color col = Color.FromArgb(Convert.ToInt32(keyValue.Key));
                        var near = ColorHelper.GetNearestColorName(ColorHelper.GetSystemDrawingColorFromHexString("#" + col.Name.Substring(2)));

                        writer.WriteLine($"{keyValue.Key} : {keyValue.Value.ColorCount} , near color = {keyValue.Value.NearColor}");
                        writer.WriteLine(near);
                    }
                }

                FilteredImageForm filteredImageForm = new FilteredImageForm();
                filteredImageForm.Image = contoursBitmap;
                filteredImageForm.Show();
            }
        }

        private void AnalyzeImage()
        {
            try
            {
                var imageFile = ImageFile.GetInstance();
                var imageBitmap = new Bitmap(imageFile.Image);
                var mapObjectColors = MapObjectsColors.GetInstance();
                foreach (var elem in mapObjectColors.ColorsDict)
                {
                    var colorStrings = elem.Value.Select(color => color.NearColor).ToList();
                    var colorName = elem.Key;
                    var bitmap = FindColor(imageBitmap, colorStrings);
                    var contoursBitmap = BitmapConverter.ToBitmap(FindContoursAndDraw(bitmap, colorName));
                    contoursBitmap.Save($"{imageFile.FileNameWithoutExtension()}_contours_{colorName}{imageFile.FileInfo.Extension}");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.StackTrace, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FindColorbutton_Click(object sender, EventArgs e)
        {

            AnalyzeImage();

        }



        private void FragmentColorsButton_Click(object sender, EventArgs e)
        {
            var imageFile = ImageFile.GetInstance();
            var imageBitmap = new Bitmap(imageFile.Image);
            if (!coordinatesAnalyze.IsEmpty)
            {

                var fragmentBitmap = FragmentBitmap(imageBitmap);
                var mapObjectColors = MapObjectsColors.GetInstance();
                foreach (var elem in mapObjectColors.ColorsDict)
                {
                    var colorName = elem.Key;
                    var colorStrings = elem.Value.Select(color => color.NearColor).ToList();
                    var bitmap = FindColor(fragmentBitmap, colorStrings);
                    var contoursBitmap = BitmapConverter.ToBitmap(FindContoursAndDraw(bitmap, colorName));
                    contoursBitmap.Save($"{imageFile.FileNameWithoutExtension()}_fragment_contours_{colorName}{imageFile.FileInfo.Extension}");
                }
            }
            else
            {
                MessageBox.Show("Выберите участок карты!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void ChooseMainColorsButton_Click(object sender, EventArgs e)
        {
            MainColorsForm mainColorsForm = new MainColorsForm();
            mainColorsForm.Show();
        }

        private Dictionary<string, ColorInfo> AreaColors(int side)
        {
            var xstart = coordinatesAnalyze.X - side / 2;
            var ystart = coordinatesAnalyze.Y - side / 2;
            var xend = coordinatesAnalyze.X + side;
            var yend = coordinatesAnalyze.Y + side;
            var xmin = xstart > 0 ? xstart : 0;
            var ymin = ystart > 0 ? ystart : 0;
            var xmax = xend < Width ? xend : Width;
            var ymax = yend < Height ? yend : Height;
            var newWidth = xmax - xmin;
            var newHeight = ymax - ymin;
            var imageBitmap = new Bitmap(ImageFile.GetInstance().Image);
            var colorDictionary = new Dictionary<string, ColorInfo>();
            var offsetX = xmin == 0 ? 0 : xmin - 1;
            var offsetY = ymin == 0 ? 0 : ymin - 1;
            for (int i = 0; i < newWidth; i++)
            {
                for (int j = 0; j < newHeight; j++)
                {
                    var medium = imageBitmap.GetPixel(i + offsetX, j + offsetY);
                    if (medium.ToArgb() != 0)
                    {

                        var near = ColorHelper.GetNearestColorName(ColorHelper.GetSystemDrawingColorFromHexString("#" + medium.Name.Substring(2)));
                        if (!colorDictionary.ContainsKey(near))
                        {
                            colorDictionary.Add(near, new ColorInfo { Color = medium, ColorCount = 1, NearColor = near });
                        }
                        else
                        {
                            colorDictionary[near].ColorCount += 1;
                        }

                    }
                    //contoursBitmap.SetPixel(i, j, medium);
                }
            }
            return colorDictionary;
        }

        private Dictionary<string, ColorInfo> PointColors()
        {

            var pointDictionary = AreaColors(PointSide);
            return pointDictionary;
        }

        private Dictionary<string, bool> ColorsAnalyze(Dictionary<string, ColorInfo> colorsDict)
        {
            var mapColorsObject = MapObjectsColors.GetInstance();
            var defaultColors = mapColorsObject.ColorsDict;
            var objectsPresenceDict = new Dictionary<string, bool>();
            var matchColors = colorsDict.Select(color => color.Key).ToList();
            foreach (var elem in mapColorsObject.ColorsDict)
            {
                objectsPresenceDict.Add(elem.Key, false);
            }

            foreach (var matchColor in matchColors)
            {
                foreach (var keyValue in defaultColors)
                {
                    var objectName = keyValue.Key;
                    var hasMapObjectColor = defaultColors[objectName].Any(color => color.NearColor == matchColor);
                    if (hasMapObjectColor)
                    {
                        objectsPresenceDict[objectName] = hasMapObjectColor;
                    }
                }
            }
            return objectsPresenceDict;
        }

        private void ImageColors(Dictionary<string, bool> presenceObjectsDict)
        {
            var imageFile = ImageFile.GetInstance();
            var imageBitmap = new Bitmap(imageFile.Image);
            var mapObjectColors = MapObjectsColors.GetInstance();
            var relief = mapObjectColors.Relief;
            var tight = mapObjectColors.Tight;
            var presenceObjects = presenceObjectsDict.Where(obj => obj.Value).Where(obj => !relief.Contains(obj.Key)).Select(obj => obj.Key).ToList();
            foreach (var elem in presenceObjects)
            {
                var colors = mapObjectColors.ColorsDict[elem].Select(color => color.NearColor).ToList();
                var bitmap = FindColor(imageBitmap, colors);
                //var contoursBitmap = bitmap;
                var contoursBitmap = BitmapConverter.ToBitmap(FindContoursAndDraw(bitmap, elem));
                contoursBitmap.Save($"{imageFile.FileNameWithoutExtension()}_colors_{elem}{imageFile.FileInfo.Extension}");
            }
        }

        private Dictionary<string, List<ImageObjectParameters>> FragmentObjectsSizeAnalyze(Dictionary<string, bool> presenceObjectsDict)
        {
            var imageBitmap = new Bitmap(ImageFile.GetInstance().Image);
            var objectParameters = new Dictionary<string, List<ImageObjectParameters>>();
            if (!coordinatesAnalyze.IsEmpty)
            {
                var xstart = coordinatesAnalyze.X - RectSide / 2;
                var ystart = coordinatesAnalyze.Y - RectSide / 2;
                var xend = coordinatesAnalyze.X + RectSide / 2;
                var yend = coordinatesAnalyze.Y + RectSide / 2;
                var xmin = xstart > 0 ? xstart : 0;
                var ymin = ystart > 0 ? ystart : 0;
                var xmax = xend < Width ? xend : Width;
                var ymax = yend < Height ? yend : Height;
                var newWidth = xmax - xmin;
                var newHeight = ymax - ymin;
                var objectsDict = mapObjects.getObjectDictionary();
                var colorsObject = MapObjectsColors.GetInstance();
                var relief = colorsObject.Relief;
                var tight = colorsObject.Tight;
                var presenceObjects = presenceObjectsDict.Where(obj => obj.Value).Where(obj => !relief.Contains(obj.Key)).Select(obj => obj.Key).ToList();

                foreach (var objectName in presenceObjects)
                {
                    if (objectsDict.ContainsKey(objectName))
                    {
                        objectParameters.Add(objectName, new List<ImageObjectParameters>());
                        var contours = objectsDict[objectName];
                        foreach (var contour in contours)
                        {
                            var matchContour = contour.Any(point => point.X > xstart && point.X < xend && point.Y > ystart && point.Y < yend);

                            if (matchContour)
                            {
                                var cArea = Cv2.ContourArea(contour) * Convert.ToDouble(scaleBox.SelectedItem) / 100;
                                var cLength = Cv2.ArcLength(contour, closed: true) * Convert.ToDouble(scaleBox.SelectedItem) / 100;

                                objectParameters[objectName].Add(new ImageObjectParameters { ArcLength = cLength, Area = cArea });
                            }

                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите участок карты!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            using (StreamWriter writer = new StreamWriter($@"{ProjectDir}\matchPoints.txt"))
            {
                foreach (var elem in objectParameters)
                {
                    writer.WriteLine(elem.Key);
                    foreach (var item in elem.Value)
                    {
                        writer.WriteLine($"Area = {item.Area}, ArcLength = {item.ArcLength}");
                    }
                }
            }
            return objectParameters;
        }



        private void FragmentAnalyzeButton_Click(object sender, EventArgs e)
        {
            if (!coordinatesAnalyze.IsEmpty)
            {
                var colorDictionary = PointColors();
                using (StreamWriter writer = new StreamWriter($@"{ProjectDir}\pointColors.txt"))
                {
                    foreach (KeyValuePair<string, ColorInfo> keyValue in colorDictionary.OrderByDescending(elem => elem.Value.ColorCount))
                    {
                        writer.WriteLine($"{keyValue.Key} : {keyValue.Value.ColorCount}");
                    }
                }
                ColorsAnalyze(colorDictionary);
            }
            else
            {
                MessageBox.Show("Выберите участок карты!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string DetectObjectName (string objName)
        {
            string objRussianName = "";
            switch (objName) {
                case "Ponds":
                    objRussianName = "Обнаружены водоёмы следующих размеров: ";
                    break;
                case "Rivers":
                    objRussianName = "Обнаружены реки следующих размеров: ";
                    break;
                case "Forests":
                    objRussianName = "Обнаружены леса следующих размеров: ";
                    break;
                case "Roads":
                    objRussianName = "Обнаружены дороги следующих размеров: ";
                    break;
                case "Sand":
                    objRussianName = "Обнаружены пустыни следующих размеров: ";
                    break;
                case "Swamp":
                    objRussianName = "Обнаружены болота следующих размеров: ";
                    break;
                case "Ice":
                    objRussianName = "Обнаружены льды следующих размеров: ";
                    break;
                case "Culture":
                    objRussianName = "Обнаружены урожайные поля следующих размеров: ";
                    break;
            }
            return objRussianName;
        }

        private void AnalyzePlaceButton_Click(object sender, EventArgs e)
        {
            if (!coordinatesAnalyze.IsEmpty)
            {
                var pointDictionary = AreaColors(PointSide);
                using (StreamWriter writer = new StreamWriter($@"{ProjectDir}\pointColors.txt"))
                {
                    foreach (KeyValuePair<string, ColorInfo> keyValue in pointDictionary.OrderByDescending(elem => elem.Value.ColorCount))
                    {
                        writer.WriteLine($"{keyValue.Key} : {keyValue.Value.ColorCount}");
                    }
                }
                var pointAnalyze = ColorsAnalyze(pointDictionary);
                var fragmentDictionary = AreaColors(RectSide);
                var fragmentAnalyze = ColorsAnalyze(fragmentDictionary);
                var analyze = new StringBuilder();

                if (detectSizeRadio.Checked)
                {
                    ImageColors(fragmentAnalyze);
                    var imageObjectsParameters = FragmentObjectsSizeAnalyze(fragmentAnalyze);
                    var imageObjectsParametersPresence = imageObjectsParameters.Where(param => param.Value.Count > 0);
                    if (imageObjectsParametersPresence.Any())
                    {
                        foreach (var elem in imageObjectsParametersPresence)
                        {
                            var objectName = elem.Key;
                            analyze.AppendLine(DetectObjectName(objectName));
                            foreach (var objectParameters in elem.Value)
                            {
                                if (objectParameters.Area > 1)
                                {
                                    analyze.AppendLine($"Длина = {objectParameters.ArcLength}, площадь: {objectParameters.Area}");
                                }
                            }
                        }
                    }

                }
                var criteriaFragmentAnalyze = CriteriaObject.GetInstance().CalculateAnalyze(fragmentAnalyze);
                var criteriaPointAnalyze = CriteriaObject.GetInstance().CalculateAnalyze(pointAnalyze, true);
                analyze.AppendLine(criteriaFragmentAnalyze);
                analyze.AppendLine(criteriaPointAnalyze);
                AnalyzeForm analyzeForm = new AnalyzeForm();
                analyzeForm.SetPointGridView(pointAnalyze);
                analyzeForm.SetFragmentGridView(fragmentAnalyze);
                analyzeForm.AddAnalyze(analyze.ToString());
                analyzeForm.Show();

            }
            else
            {
                MessageBox.Show("Выберите участок карты!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RiversButton_Click(object sender, EventArgs e)
        {
            if (!coordinatesAnalyze.IsEmpty)
            {
                var fragmentDictionary = AreaColors(RectSide);
                var fragmentAnalyze = ColorsAnalyze(fragmentDictionary);
                FragmentColorsDetect(fragmentAnalyze);

            }
            else
            {
                MessageBox.Show("Выберите участок карты!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FragmentColorsDetect(Dictionary<string,bool> presenceObjectsDict)
        {
            var imageFile = ImageFile.GetInstance();
            var imageBitmap = new Bitmap(imageFile.Image);
            if (!coordinatesAnalyze.IsEmpty)
            {

                var fragmentBitmap = FragmentBitmap(imageBitmap);
                var mapObjectColors = MapObjectsColors.GetInstance();
                var relief = mapObjectColors.Relief;
                var presenceObjects = presenceObjectsDict.Where(obj => obj.Value).Where(obj => !relief.Contains(obj.Key)).Select(obj => obj.Key).ToList();
                
                foreach (var colorName in presenceObjects)
                {
                    var colors = mapObjectColors.ColorsDict[colorName].Select(color => color.NearColor).ToList();
                    var bitmap = FindColor(fragmentBitmap, colors);
                    var contoursBitmap = BitmapConverter.ToBitmap(FindContoursAndDraw(bitmap, colorName));
                    contoursBitmap.Save($"{imageFile.FileNameWithoutExtension()}_fragment_colors_{colorName}{imageFile.FileInfo.Extension}");
                }
            }
            else
            {
                MessageBox.Show("Выберите участок карты!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CriteriaButton_Click(object sender, EventArgs e)
        {
            CriteriaForm criteriaForm = new CriteriaForm();
            criteriaForm.Show();
        }
    }
}
