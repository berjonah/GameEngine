using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK;

namespace GameEngine
{
    class PointData
    {
        private float[][] points;
        private int size;

        private Vector3[] vertData;

        public Vector3[] VertData
        {
            get { return vertData; }
            set { vertData = value; }
        }
        private Vector3[] colorData;

        public Vector3[] ColorData
        {
            get { return colorData; }
            set { colorData = value; }
        }

        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        public PointData(string filePath)
        {
            string[] lineData;

            size = 0;

            string[] lines = File.ReadAllLines(filePath);
            points = new float[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                lineData = lines[i].Split(',');
                points[i] = new float[lineData.Length];
                for (int j = 0; j < lineData.Length; j++)
                {
                    points[i][j] = float.Parse(lineData[j]);
                    size++;
                }
            }

            vertData = new Vector3[size];
            colorData = new Vector3[size];

            for(int i = 0; i < points.Length; i++)
            {
                for(int j = 0; j < points[i].Length; j++)
                {
                    vertData[(points.Length * i) + j] = new Vector3(i-(points.Length/2), points[i][j], j-(points[i].Length/2));
                    colorData[(points.Length * i) + j] = new Vector3(0f, 0f, 1f / points[i][j]);
                }
            }
        }



    }
}
