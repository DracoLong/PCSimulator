using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MDPSimulator
{
    /// <summary>
    /// Interaction logic for settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public int timeLimit;
        public int coverageLimit;
        public int speed;
        public Settings()
        {
            InitializeComponent();
            this.speedBox.Text = UserSetting.Speed.ToString();
            this.timeLimitBox.Text = UserSetting.TimeLimit.ToString();
            this.coverageBox.Text = UserSetting.CoverageLimit.ToString();

        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            timeLimit = 10;
            Int32.TryParse(this.timeLimitBox.Text, out timeLimit);

            coverageLimit = 100;
            Int32.TryParse(this.coverageBox.Text, out coverageLimit);

            speed = 3;
            Int32.TryParse(this.speedBox.Text, out speed);
            this.DialogResult = true;
            this.Close();
        }

        public int getTimeLimit() { return timeLimit; }
        public int getCoverageLimit() { return coverageLimit; }
        public int getSpeed() { return speed; }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void coverageBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
