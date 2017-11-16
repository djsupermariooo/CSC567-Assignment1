using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1

{

    public partial class Form1 : Form
    {
        //Global Variables
        bool[] bits;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();

            // Check for valid input
            bool valid = validateInput(textBox1.Text);

            if (valid)
            {
                // Convert string to boolean values
                bits = textBox1.Text.Select(c => c == '1').ToArray();

                // Process Differential Manchester Encoding
                processDME(bits);

                // Process Amplitude Shift Keying
                processASK(bits);

                // Process Frequency Shift Keying
                processFSK(bits);
            }

            textBox1.Clear();
        }

        private bool validateInput(string s)
        {
            // Error checking: must ensure text is not empty
            if (s.Length == 0)
            {
                MessageBox.Show("Please enter bits!");
                return false;
            }

            // Error checking: must ensure entered text are valid bits
            bool isValidBits = true;
            foreach (char c in s)
            {
                if (c != '0' && c != '1')
                    isValidBits = false;
            }

            if (isValidBits == false)
            { 
                MessageBox.Show("Error: Must only enter a combination of 0's and 1's!");
                return false;
            }
            return true;
        }

        private void processDME(bool[] booleans)
        {
            // Local Variables
            List<float> xData = new List<float>();
            List<float> yData = new List<float>();
            float firstPointX = 0;
            float secondPointX = 0;
            float thirdPointX = 0;
            float fourthPointX = 0;
            float fifthPointX = 0;
            float firstPointY = 0;
            float secondPointY = 0;
            float thirdPointY = 0;
            float fourthPointY = 0;
            float fifthPointY = 0;

            // If the first bit is a 0
            if (booleans[0] == false)
            {
                firstPointX = 1;
                firstPointY = 1;
                secondPointX = firstPointX;
                secondPointY = -firstPointY;
                thirdPointX = secondPointX + 0.5f;
                thirdPointY = secondPointY;
                fourthPointX = thirdPointX;
                fourthPointY = -thirdPointY;
                fifthPointX = thirdPointX + 0.5f;
                fifthPointY = fourthPointY;

                xData.Add(firstPointX);
                xData.Add(secondPointX);
                xData.Add(thirdPointX);
                xData.Add(fourthPointX);
                xData.Add(fifthPointX);

                yData.Add(firstPointY);
                yData.Add(secondPointY);
                yData.Add(thirdPointY);
                yData.Add(fourthPointY);
                yData.Add(fifthPointY);
            }
            else // Otherwise, first bit is a 1
            {
                firstPointX = 1;
                firstPointY = 1;
                secondPointX = firstPointX + 0.5f;
                secondPointY = firstPointY;
                thirdPointX = secondPointX;
                thirdPointY = -secondPointY;
                fourthPointX = thirdPointX + 0.5f;
                fourthPointY = thirdPointY;

                xData.Add(firstPointX);
                xData.Add(secondPointX);
                xData.Add(thirdPointX);
                xData.Add(fourthPointX);

                yData.Add(firstPointY);
                yData.Add(secondPointY);
                yData.Add(thirdPointY);
                yData.Add(fourthPointY);
            }
            
            for (int i=1; i<booleans.Length; i++)
            {
                // If the current bit is a 1
                if (booleans[i] == true)
                {
                    if (booleans[i - 1] == false)
                    {
                        firstPointX = fifthPointX;
                        firstPointY = fifthPointY;
                    }
                    else
                    {
                        firstPointX = fourthPointX;
                        firstPointY = fourthPointY;
                    }

                    secondPointY = firstPointY;
                    secondPointX = firstPointX + 0.5f;
                    thirdPointX = secondPointX;
                    thirdPointY = -secondPointY;
                    fourthPointX = thirdPointX + 0.5f;
                    fourthPointY = thirdPointY;

                    xData.Add(firstPointX);
                    xData.Add(secondPointX);
                    xData.Add(thirdPointX);
                    xData.Add(fourthPointX);

                    yData.Add(firstPointY);
                    yData.Add(secondPointY);
                    yData.Add(thirdPointY);
                    yData.Add(fourthPointY);
                }
                else // Otherwise, the current bit is a 0
                {
                    if (booleans[i - 1] == false)
                    {
                        firstPointX = fifthPointX;
                        firstPointY = fifthPointY;
                    }
                    else
                    {
                        firstPointX = fourthPointX;
                        firstPointY = fourthPointY;
                    }

                    secondPointY = -firstPointY;
                    secondPointX = firstPointX;
                    thirdPointX = secondPointX + 0.5f;
                    thirdPointY = secondPointY;
                    fourthPointX = thirdPointX;
                    fourthPointY = -thirdPointY;
                    fifthPointX = fourthPointX + 0.5f;
                    fifthPointY = fourthPointY;

                    xData.Add(firstPointX);
                    xData.Add(secondPointX);
                    xData.Add(thirdPointX);
                    xData.Add(fourthPointX);
                    xData.Add(fifthPointX);

                    yData.Add(firstPointY);
                    yData.Add(secondPointY);
                    yData.Add(thirdPointY);
                    yData.Add(fourthPointY);
                    yData.Add(fifthPointY);
                }
            }

            // Add data to DME chart
            chart3.Series[0].Points.DataBindXY(xData, yData);
        }

        private void processASK(bool[] booleans)
        {
            for (int i=0; i < booleans.Length; i++)
            {
                if (booleans[i] == false)
                {
                    chart1.Series[0].Points.AddXY(i + 1, 0);
                    chart1.Series[0].Points.AddXY(i + 2, 0);
                }
                else
                {
                    for (double x = i+1; x < i+2; x += 0.001)
                    {
                        double yValue = Math.Sin(4 * x * 2 * Math.PI);
                        chart1.Series[0].Points.AddXY(x, yValue);
                    }
                }
            }
        }

        private void processFSK(bool[] booleans)
        {
            for (int i = 0; i < booleans.Length; i++)
            {
                if (booleans[i] == false)
                {
                    for (double x = i + 1; x < i + 2; x += 0.001)
                    {
                        double yValue = Math.Sin(2 * x * 2 * Math.PI);
                        chart2.Series[0].Points.AddXY(x, yValue);
                    }
                }
                else
                {
                    for (double x = i + 1; x < i + 2; x += 0.001)
                    {
                        double yValue = Math.Sin(4 * x * 2 * Math.PI);
                        chart2.Series[0].Points.AddXY(x, yValue);
                    }
                }
            }
        }
    }
}
