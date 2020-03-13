using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Diagnostics;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using FireSharp.Extensions;


namespace Cypherz
{

    public partial class Form1 : Form
    {
        
        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "uaV8UxYoFmLHWMBDB3n7rLPvecjKqCJLVBjBodhn",
            BasePath = "https://bcas-d6975.firebaseio.com/"

        };
        
        IFirebaseClient client;

        public Form1()
        {
            InitializeComponent();
            client = new FireSharp.FirebaseClient(config);
        }
        //submit button checks to see if the name fields are empty
        private async void submitButton_Click(object sender, EventArgs e)
        {

            foreach (Panel pnl in Controls.OfType<Panel>())
            {
                foreach (TextBox tb in pnl.Controls.OfType<TextBox>())
                {
                    if (string.IsNullOrEmpty(tb.Text.Trim()))
                    {
                        MessageBox.Show("Ensure all data fields are filled");
                        return;
                    }
                }
            }

            string Creds="";
            foreach(Panel pnl in Controls.OfType<Panel>())
            {
                foreach(CheckBox chk in pnl.Controls.OfType<CheckBox>())
                {
                    if (chk.Checked)
                    {
                        Creds += "\'"+chk.Text+"\'";
                    }
                }
            }
            //
            var data = new Teacher
            {
                credentials = Creds,
                lastName = lastNameText.Text,
                middleName = middleNameText.Text,
                firstName = firstNameText.Text,
            };
            SetResponse response = await client.SetTaskAsync("Teachers/" + firstNameText.Text+" "+lastNameText.Text,data);
            Teacher result = response.ResultAs<Teacher>();
            MessageBox.Show("Data Inserted: " + result.firstName);

    }

        private async void resetButton_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.GetTaskAsync("Teachers/"+firstNameText.Text+" "+lastNameText.Text);
            Teacher teacher = response.ResultAs<Teacher>();
            MessageBox.Show("Teacher: "+teacher.firstName+teacher.lastName+"\nCredentials: "+teacher.credentials);
            
        }
    }
}
