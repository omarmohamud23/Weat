using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Weat
{
    public partial class Form1 : Form
    {
        readonly string BaseUrl = "https://weather-csharp.herokuapp.com/";
        public Form1()
        {
            InitializeComponent();
        }
        private bool GetWeatherText(string city, string state, out string weatherText, out string errorMessage)
        {
        http://weather-csharp.herokuapp.com/text?city=houston&state=tx

            string weatherTextUrl = string.Format("{0}text?city={1}&state={2}", BaseUrl, city, state);
          
            Debug.WriteLine(weatherTextUrl);

            errorMessage = null;
            weatherText =null;

            try
            {
                using (WebClient client = new WebClient())
                {
                    weatherText = client.DownloadString(weatherTextUrl);
                }
                Debug.WriteLine(weatherText);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                errorMessage = e.Message;
                return false;
            }
           
        }

        private bool LocationDataValid (string city, string state)
        {

            
            if (string.IsNullOrWhiteSpace(city))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(state))
            {
                return false;
            }
            return true;
        }
       
        private bool GetWeatherImage (string city, string state, out Image weatherImage, out string errorMessage)
        {
            weatherImage = null;
            errorMessage = null;

            try
            {
                using (WebClient client = new WebClient())
                {
                http://weather-csharp.herokuapp.com/photo?city=houston&state=tx
                    string weatherPhotoUrl = string.Format("{0}photo?city={1}&state{2}", BaseUrl, city, state);
                    string tempFileDirectory = Path.GetTempPath().ToString();
                    string weatherFilePath = Path.Combine(tempFileDirectory, "weather_image.jpeg");
                    Debug.WriteLine(weatherFilePath);
                    client.DownloadFile(weatherPhotoUrl, weatherFilePath);
                    weatherImage = Image.FromFile(weatherFilePath);

                }
                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                errorMessage = e.Message;
                return false;
            }
        }
        private void btnGetWeather_Click(object sender, EventArgs e)
        {
            btnGetWeather.Enabled = false;

            string city = txtCity.Text;
            string state = txtState.Text;
            
            if (LocationDataValid(city, state))
            {
                if (GetWeatherText(city, state, out string weather, out string textErrorMessage))
                {
                    lblWeather.Text = weather;
                }
                else
                {
                    MessageBox.Show(textErrorMessage, "Error");
                }

            }
            else
            {
                MessageBox.Show("Enter both city and state", "Error");
            }

            if(pictureBox1.Image !=null)
            {
                pictureBox1.Image.Dispose();
            }
            if (GetWeatherImage(city, state, out Image image, out string imageErrorMessage))
            {
                pictureBox1.Image = image;
            }
            btnGetWeather.Enabled = true;
        }
    }
}
