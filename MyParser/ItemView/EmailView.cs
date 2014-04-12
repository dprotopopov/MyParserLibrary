using System.Drawing;

namespace MyParser.ItemView
{
    public class EmailView
    {
        public EmailView()
        {
            Icon = Defaults.EmailIcons[0];
        }

        public Bitmap Icon { get; set; }

        public string Address { get; set; }
    }
}