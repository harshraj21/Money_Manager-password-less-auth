using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;

namespace MoneyManager
{
    public partial class Form1 : MetroForm
    {
        string hwidlocal;
        string server;
        string usrnme;
        int food = 0;
        int housing = 0;
        int travel = 0;
        int entertainment = 0;
        int health = 0;
        int sports = 0;

        int moneyinc = 0;
        int moneytotal = 0;
        int moneysav = 0;
        int moneydep = 0;

        string total;
        string totala;
        string income;
        string incomea;
        string saving;
        string savinga;
        string deposit;
        string deposita;
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            hwidlocal = hardwareid.MachineID();
            server = "https://moneymanagerproject.000webhostapp.com/logtest.php";
            totala = "https://moneymanagerproject.000webhostapp.com/total.php";
            total = "https://moneymanagerproject.000webhostapp.com/totala.php";
            incomea = "https://moneymanagerproject.000webhostapp.com/income.php";
            income = "https://moneymanagerproject.000webhostapp.com/incomea.php";
            savinga = "https://moneymanagerproject.000webhostapp.com/saving.php";
            saving = "https://moneymanagerproject.000webhostapp.com/savinga.php";
            deposita = "https://moneymanagerproject.000webhostapp.com/deposit.php";
            deposit = "https://moneymanagerproject.000webhostapp.com/deposita.php";

        }

        private void button2_Click(object sender, EventArgs e)//login main
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("https://moneymanagerproject.000webhostapp.com/"))
                    {

                    }
                }
            }
            catch
            {
                MessageBox.Show("Unable To Connect with server!\nTry Again!", "Connection Error!");
                return;
            }

            usrnme = username.Text;
            //LoggedIN();
            Login(username.Text, password.Text);

        }

        private void button3_Click(object sender, EventArgs e)//exit main
        {
            Thread.Sleep(50);
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)// exit/update
        {
            /*var webs = new WebClient();
           var result = webs.DownloadString(moneyupdt + "?username=" + usrnme + "&moneytotal=" + moneytotal.ToString());
           Thread.Sleep(300);*/
            var webs = new WebClient();
            var result = webs.DownloadString(totala + "?username=" + usrnme + "&total=" + moneytotal.ToString());

            var webs1 = new WebClient();
            var result1 = webs1.DownloadString(incomea + "?username=" + usrnme + "&income=" + moneyinc.ToString());

            var webs2 = new WebClient();
            var result2 = webs2.DownloadString(savinga + "?username=" + usrnme + "&saving=" + moneysav.ToString());

            var webs3 = new WebClient();
            var result3 = webs3.DownloadString(deposita + "?username=" + usrnme + "&deposit=" + moneydep.ToString());


            Thread.Sleep(50);
            Application.Exit();
        }

        private void Login(string username, string password)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var stringChars = new char[5];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string token = new String(stringChars);

            var web = new WebClient();
            var result = web.DownloadString(server + "?username=" + username + "&password=" + password + "&hwid=" + hwidlocal + "&token=" + token);
            if (result != null && result.Contains("p1"))//correct password
            {
                if (result.Contains("g4") || result.Contains("g2"))//admin-user
                {
                    if (result.Contains("h1"))//hwid correct
                    {
                        if (result.Contains(token))
                        {
                            var webs = new WebClient();
                            var results = webs.DownloadString(total + "?username=" + username);

                            var web2 = new WebClient();
                            var result2 = web2.DownloadString(income + "?username=" + username);

                            var web3 = new WebClient();
                            var result3 = web3.DownloadString(saving + "?username=" + username);

                            var web4 = new WebClient();
                            var result4 = web4.DownloadString(deposit + "?username=" + username);


                            moneytotal = Convert.ToInt32(results.ToString());
                            moneyinc = Convert.ToInt32(result2.ToString());
                            moneysav = Convert.ToInt32(result3.ToString());
                            moneydep = Convert.ToInt32(result4.ToString());
                            LoggedIN();
                        }
                        else
                        {
                            MessageBox.Show("DNS Spoofing Detected...","ERROR");
                        }
                    }
                    else if(result.Contains("h9"))
                    {
                        MessageBox.Show("New HWID Set sucessful");
                    }
                    else if(result.Contains("h8"))
                    {
                        MessageBox.Show("HWID Update failed!","HWID");
                    }
                    else//all other
                    {
                        MessageBox.Show("HWID Missmatched!", "HWID");
                    }
                }
                else if (result.Contains("g5"))// awaiting activation
                {
                    MessageBox.Show("Verify Your Email to Continue", "VERIFICATION");
                }
                else//all other
                {
                    MessageBox.Show("Acess Denied!!", "DENIED");
                }
            }
            else if (result.Contains("p0"))//wrong password
            {
                MessageBox.Show("Wrong Password Detected!", "FAILED");
            }
            else//user dont exist
            {
                MessageBox.Show("User Doesnt Exist!", "NO USER");
            }
        }


        private void LoggedIN()
        {
            //panel1.Visible = true;
            panel2.Visible = true;
            button4.Visible = true;
            panel4.Visible = true;

            moneytotal = moneyinc + moneysav + moneydep;

            label8.Text = moneytotal.ToString() + " Rs";
            label16.Text = moneytotal.ToString() + " Rs";

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable To Visit,Try later!","ERROR");
            }
        }

        private void VisitLink()
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("https://moneymanagerproject.000webhostapp.com/member.php?action=register");
        }

        private void overview_Click(object sender, EventArgs e)
        {

            panel4.Visible = true;
            panel3.Visible = false;
            panel1.Visible = false;
        }

        private void earning_Click(object sender, EventArgs e)
        {
            label8.Text = moneytotal.ToString();
            panel1.Visible = true;
            panel3.Visible = false;
            panel4.Visible = false;
        }

        private void expence_Click(object sender, EventArgs e)
        {
            label16.Text = moneytotal.ToString();
            panel1.Visible = false;
            panel3.Visible = true;
            panel4.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e) // income update ----------------
        {
            moneytotal = moneyinc + moneysav + moneydep; 
            label8.Text = moneytotal.ToString() + " Rs";

            var webs = new WebClient();
            var result = webs.DownloadString(totala + "?username=" + usrnme + "&total=" + moneytotal.ToString());

            var webs1 = new WebClient();
            var result1 = webs1.DownloadString(incomea + "?username=" + usrnme + "&income=" + moneyinc.ToString());

            var webs2 = new WebClient();
            var result2 = webs2.DownloadString(savinga + "?username=" + usrnme + "&saving=" + moneysav.ToString());

            var webs3 = new WebClient();
            var result3 = webs3.DownloadString(deposita + "?username=" + usrnme + "&deposit=" + moneydep.ToString());


            Thread.Sleep(50);
        }

        private void button5_Click(object sender, EventArgs e) // expence update --------------------
        {

            label16.Text = Convert.ToString((moneyinc + moneysav + moneydep) - (food + housing + travel + entertainment + health + sports));
            moneytotal = ((moneyinc + moneysav + moneydep ) - (food + housing + travel + entertainment + health + sports));

            var webs = new WebClient();
            var result = webs.DownloadString(totala + "?username=" + usrnme + "&total=" + moneytotal.ToString());

            if (metroRadioButton1.Checked == true)
            {
                moneyinc -= food;
                moneyinc -= entertainment;
                moneyinc -= travel;
                moneyinc -= health;
                moneyinc -= sports;
                moneyinc -= housing;

            }
            else if (metroRadioButton2.Checked == true)
            {
                moneysav -= food;
                moneysav -= entertainment;
                moneysav -= travel;
                moneysav -= health;
                moneysav -= sports;
                moneysav -= housing;
            }
            else if (metroRadioButton3.Checked == true)
            {
                moneydep -= food;
                moneydep -= entertainment;
                moneydep -= travel;
                moneydep -= health;
                moneydep -= sports;
                moneydep -= housing;
            }

            Thread.Sleep(50);

        }

        private void plus_Click(object sender, EventArgs e) //income add
        {
            moneyinc += Convert.ToInt32(incometext.Text);
        }

        private void minus_Click(object sender, EventArgs e) //income subs
        {
            moneyinc -= Convert.ToInt32(incometext.Text);
        }

        private void plus1_Click(object sender, EventArgs e) // saving add
        {
            moneysav += Convert.ToInt32(savingstext.Text);
        }

        private void minus1_Click(object sender, EventArgs e) // savings subs
        {
            moneysav -= Convert.ToInt32(savingstext.Text);
        }

        private void plus2_Click(object sender, EventArgs e) //deposit add
        {
            moneydep += Convert.ToInt32(deposittext.Text);
        }

        private void minus2_Click(object sender, EventArgs e) //deposit subs
        {
            moneydep -= Convert.ToInt32(deposittext.Text);
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e) //food plus
        {
            food += Convert.ToInt32(textBox1.Text);
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)// food minus
        {
            food -= Convert.ToInt32(textBox1.Text);
        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)// housng plus
        {
            housing += Convert.ToInt32(textBox2.Text);
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e) //housing minus
        {
            housing -= Convert.ToInt32(textBox2.Text);
        }

        private void bunifuImageButton6_Click(object sender, EventArgs e)//travel plus
        {
            travel += Convert.ToInt32(textBox3.Text);
        }

        private void bunifuImageButton5_Click(object sender, EventArgs e) //trvel minus
        {
            travel -= Convert.ToInt32(textBox3.Text);
        }

        private void bunifuImageButton12_Click(object sender, EventArgs e) //enter plus
        {
            entertainment += Convert.ToInt32(textBox6.Text);
        }

        private void bunifuImageButton11_Click(object sender, EventArgs e)// enter minus
        {
            entertainment -= Convert.ToInt32(textBox6.Text);
        }

        private void bunifuImageButton10_Click(object sender, EventArgs e)//health plus
        {
            health += Convert.ToInt32(textBox5.Text);
        }

        private void bunifuImageButton9_Click(object sender, EventArgs e)//health minus
        {
            health -= Convert.ToInt32(textBox5.Text);
        }

        private void bunifuImageButton8_Click(object sender, EventArgs e) //sports plus
        {
            sports += Convert.ToInt32(textBox4.Text);
        }

        private void bunifuImageButton7_Click(object sender, EventArgs e) // sports minus
        {
            sports -= Convert.ToInt32(textBox4.Text);
        }
    }
}

//panel 2 - earning -- hidden
//panel 3 - expence -- hidden
//panel 4 - overview -- hidden
